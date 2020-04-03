using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Core.Binders
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Class TypedModelBinder. Used to bind all models assignable to <see cref="T:DD4T.Core.Contracts.ViewModels.IViewModel" /> used in the application.
    /// Uses the url to get the model data from the broker, looks for the correct model type and creates it.
    /// </summary>
    /// <seealso cref="T:System.Web.Mvc.IModelBinder" />
    /// <seealso cref="T:System.Web.Mvc.IModelBinderProvider" />
    public class TypedModelBinder : IModelBinder, IModelBinderProvider
    {
        /// <summary>
        /// The content provider
        /// </summary>
        private readonly IEnumerable<IContentByUrlProvider> _contentByUrlProviders;
        private readonly IPublicationResolver _publicationResolver;
        private readonly ILogger _logger;

        /// <summary>
        /// The model key format, used to store the model in context
        /// </summary>
        private const string _cacheKeyFormat = "Model_{0}_{1}";

        private const string _cacheRegion = "Model";
        private readonly ICacheAgent _cacheAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedModelBinder" /> class.
        /// </summary>
        /// <param name="contentByUrlProviders">List of content providers by url.</param>
        /// <param name="cacheAgent">The cache agent.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        public TypedModelBinder(IEnumerable<IContentByUrlProvider> contentByUrlProviders,
            ICacheAgent cacheAgent,
            IPublicationResolver publicationResolver, ILogger logger)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            contentByUrlProviders.ThrowIfNull(nameof(contentByUrlProviders));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));
            logger.ThrowIfNull((nameof(logger)));

            _cacheAgent = cacheAgent;
            _contentByUrlProviders = contentByUrlProviders;
            _publicationResolver = publicationResolver;
            _logger = logger;
        }

        /// <inheritdoc />
        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>The bound object.</returns>
        public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IViewModel model = null;

            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null) return (IViewModel)null;

            var passedModel = value.RawValue;

            if (passedModel != null && bindingContext.ModelType.IsInstanceOfType(passedModel))
            {
                //route already available and of the correct type so return it.
                return passedModel;
            }

            string modelData = value.AttemptedValue;
            //return null if modeldata is not set.
            if (modelData == null || (modelData.IsNullOrEmpty() && !typeof(IWebPage).IsAssignableFrom(bindingContext.ModelType))) return null;


            //generate a Cache key for the current model.
            var pubId = _publicationResolver.ResolvePublicationId();
            var cacheKey = _cacheKeyFormat.FormatString(pubId, modelData);
            _logger.Debug($"ModelBinder is looking for page with key {cacheKey}");
            model = _cacheAgent.Load(cacheKey) as IViewModel;

            if (model == null)
            {
                // check if the action method is requesting a concrete implementation of IWebPage
                // if so, pass that in to the content provider as 'preferred model' type
                var preferredModelType = (!(bindingContext.ModelType.IsInterface || bindingContext.ModelType.IsAbstract)) ? bindingContext.ModelType : null;

                //select the first provider that returns a IViewModel
                model = _contentByUrlProviders
                    .Select(p => p.Retrieve(modelData, preferredModelType))
                    .FirstOrDefault(p => !p.IsNull());
                _logger.Debug($"ModelBinder found page with key {cacheKey} using content provider");

                if (model != null && !bindingContext.ModelType.IsInstanceOfType(model))
                {
                    //Model is incorrect, so remove
                    model = null;
                }
                if (model != null)
                {
                    _cacheAgent.Store(cacheKey, _cacheRegion, model, new List<string> { model.ModelData.GetId() });
                }
            }
            else
            {
                _logger.Debug($"ModelBinder found page with key {cacheKey} in the cache");
            }

            if (model is IRenderable renderable && !controllerContext.IsChildAction)
            {
                controllerContext.RouteData.DataTokens["area"] = renderable.MvcData.Area;
            }

            return model;
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns the model binder for the specified type if assignable to <see cref="T:DD4T.Core.Contracts.ViewModels.IViewModel" />
        /// </summary>
        /// <param name="modelType">The type of the model.</param>
        /// <returns>The model binder for the specified type.</returns>
        public IModelBinder GetBinder(Type modelType)
        {
            //Can bind to IViewModel
            return typeof(IViewModel).IsAssignableFrom(modelType) && modelType.Name != "ISitemapItem" ? this : null;
        }
    }
}