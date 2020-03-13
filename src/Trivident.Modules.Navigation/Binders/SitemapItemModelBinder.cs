using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Logging;
using Trivident.Modules.Navigation.Models;
using Trivident.Modules.Navigation.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dyndle.Modules.Core.Cache;
using Dyndle.Modules.Core.Extensions;

namespace Trivident.Modules.Navigation.Binders
{
    /// <summary>
    /// Class SitemapItemModelBinder.
    /// Used to provide the <see cref="ISitemapItem"/>  models to an controller action. 
    /// Will be automatically used for all model instances implementing <see cref="ISitemapItem"/> when registered.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.DefaultModelBinder" />
    /// <seealso cref="System.Web.Mvc.IModelBinderProvider" />
    public class SitemapItemModelBinder : DefaultModelBinder, IModelBinderProvider
    {
        private readonly INavigationProvider _navigationProvider;
        private const string _cacheKeyFormat = "Navigation({0}-{1}-{2}-{3}-{4})";
        private const string _cacheRegion = "Navigation";
        private readonly ISerializedCacheAgent _cacheAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapItemModelBinder" /> class.
        /// </summary>
        /// <param name="navigationProvider">The navigation provider.</param>
        /// <param name="cacheAgent">The cache agent.</param>
        public SitemapItemModelBinder(INavigationProvider navigationProvider, ISerializedCacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            navigationProvider.ThrowIfNull(nameof(navigationProvider));
            _cacheAgent = cacheAgent;
            _navigationProvider = navigationProvider;
        }

        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>The bound object.</returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ISitemapItem model = null;
            
            string navType = null;
            string navSubtype = null;
            int navLevels = 0;
            int navStartLevel = -1;

            // navType & navLevels & navStartLevel are set on the 'RouteData' Metadata field on the component template.
            // and passed as routeparameters to the action.
            ValueProviderResult navTypeResult = bindingContext.ValueProvider.GetValue(NavigationConstants.RouteValues.NavType);
            ValueProviderResult navLevelsResult = bindingContext.ValueProvider.GetValue(NavigationConstants.RouteValues.NavLevels);
            ValueProviderResult navStartLevelResult = bindingContext.ValueProvider.GetValue(NavigationConstants.RouteValues.NavStartLevel);
            ValueProviderResult navSubtypeResult = bindingContext.ValueProvider.GetValue(NavigationConstants.RouteValues.NavSubtype);

            if (!navTypeResult.IsNull())
            {
                navType = navTypeResult.AttemptedValue;
            }

           string requestUrlPath = controllerContext.HttpContext.Request.Url.LocalPath;

            if (requestUrlPath.ToLowerInvariant().Contains("sitemap.xml"))
            {
                navType = "sitemap";
            }

            if (navLevelsResult != null)
                int.TryParse(navLevelsResult.AttemptedValue, out navLevels);
            if (navStartLevelResult != null)
                int.TryParse(navStartLevelResult.AttemptedValue, out navStartLevel);
            if (navSubtypeResult != null)
                navSubtype = navSubtypeResult.AttemptedValue;


            //generate a Cache key for the current model.
            //try to get model out of HttpContext.Items[], incase the same model is request multiple times during
            //a single request.
            string key = _cacheKeyFormat.FormatString(navType, requestUrlPath, navLevels, navStartLevel, navSubtype);

            model = _cacheAgent.Load(key) as ISitemapItem;

            if (model == null)
            {
                switch (navType?.ToLower())
                {
                    case "children":
                        model = _navigationProvider.GetChildren(requestUrlPath, navLevels, navStartLevel, navSubtype);
                        break;

                    case "path":
                        model = _navigationProvider.GetPath(requestUrlPath);
                        break;

                    case "sitemap":
                        model = _navigationProvider.GetFullSitemap();
                        break;

                    default:
                        model = _navigationProvider.GetAll(navLevels, navSubtype);
                        break;
                }
                if (model == null)
                {
                    model = new SitemapItem();
                }

                _cacheAgent.Store(key, _cacheRegion, model);
            }

            return model;
        }

        /// <summary>
        /// Returns the model binder for the specified type.
        /// </summary>
        /// <param name="modelType">The type of the model.</param>
        /// <returns>The model binder for the specified type.</returns>
        public IModelBinder GetBinder(Type modelType)
        {
            //Can bind to ISitemapItem
            if (typeof(ISitemapItem).IsAssignableFrom(modelType)) return this;
            return null;
        }
    }
}