using System;
using System.Collections.Generic;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Factories;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Providers.Content
{
    /// <summary>
    /// Class DefaultContentProvider.
    /// Provides easy methods for model mapping based on DD4T models
    /// </summary>
    /// <seealso cref="IContentProvider" />
    public class DefaultContentProvider : IContentProvider
    {
        private const string _cacheKeyFormat = "Model_{0}";
        private const string _cacheRegion = "Model";
        private const string _cacheKeyFormatWithPubId = "Model_{0}_{1}";
        private readonly ILogger _logger;
        private readonly IPageFactory _pageFactory;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IComponentPresentationFactory _componentPresentationFactory;
        private readonly IDD4TConfiguration _configuration;
        private readonly IExtendedPublicationResolver _publicationResolver;
        private readonly ICacheAgent _cacheAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContentProvider" /> class.
        /// </summary>
        /// <param name="pageFactory">The page factory.</param>
        /// <param name="componentPresentationFactory">The component presentation factory.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cacheAgent">The cache agent.</param>
        public DefaultContentProvider(IPageFactory pageFactory,
                                   IComponentPresentationFactory componentPresentationFactory,
                                   IViewModelFactory viewModelFactory,
                                   ILogger logger,
                                   IExtendedPublicationResolver publicationResolver,
                                   IDD4TConfiguration configuration, ICacheAgent cacheAgent)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            pageFactory.ThrowIfNull(nameof(pageFactory));
            viewModelFactory.ThrowIfNull(nameof(viewModelFactory));
            logger.ThrowIfNull(nameof(logger));
            componentPresentationFactory.ThrowIfNull(nameof(componentPresentationFactory));
            configuration.ThrowIfNull(nameof(configuration));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));

            _cacheAgent = cacheAgent;
            this._publicationResolver = publicationResolver;
            this._pageFactory = pageFactory;
            this._viewModelFactory = viewModelFactory;
            this._componentPresentationFactory = componentPresentationFactory;
            this._logger = logger;
            this._configuration = configuration;
        }

        /// <summary>
        /// Gets the component presentation.
        /// </summary>
        /// <param name="componentId">The component identifier.</param>
        /// <param name="templateId">The template identifier.</param>
        /// <returns>IComponentPresentation.</returns>
        public IComponentPresentation GetComponentPresentation(string componentId, string templateId = "")
        {
            return _componentPresentationFactory.GetComponentPresentation(componentId, templateId);
        }

        /// <summary>
        /// Tries the get component presentation.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="componentId">The component identifier.</param>
        /// <param name="templateId">The template identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGetComponentPresentation(out IComponentPresentation component, string componentId, string templateId = "")
        {
            return _componentPresentationFactory.TryGetComponentPresentation(out component, componentId, templateId);
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>IPage.</returns>
        public IPage GetPage(string url)
        {
            _logger.Debug("GetPage - Processing request for page: {0}", url);
            var currentUrl = url.ParseUrl(_publicationResolver.GetBaseUri().AbsolutePath, false, _configuration.WelcomeFile);
            IPage page;
            if (_pageFactory.TryFindPage(Uri.EscapeUriString(currentUrl), out page))
            {
                return page;
            }

            var alternativeUrl = url.ParseUrl(_publicationResolver.GetBaseUri().AbsolutePath, true, _configuration.WelcomeFile);
            if (currentUrl != alternativeUrl)
            {
                if (_pageFactory.TryFindPage(Uri.EscapeUriString(alternativeUrl), out page))
                {
                    return page;
                }
            }
            return null;
        }

        /// <summary>
        /// Temporary solution to pass the preferred model type to the ViewModelFactory indirectly
        /// The better way is to extend the ViewModelFactory interface, but this requires a DD4T change
        /// We will do this in 2.5
        /// </summary>
        /// <param name="page"></param>
        /// <param name="preferredModelType"></param>
        private void AddModelTypeToPage(IPage page, Type preferredModelType)
        {
            if (page == null || preferredModelType == null)
            {
                return;
            }
            if (page.PageTemplate.MetadataFields == null || ! page.PageTemplate.MetadataFields.ContainsKey("PreferredModelType"))
            {
                lock (locker)
                {
                    if (page.PageTemplate.MetadataFields == null || !page.PageTemplate.MetadataFields.ContainsKey("PreferredModelType"))
                    {
                        ((PageTemplate)page.PageTemplate).MetadataFields = new FieldSet();
                        page.PageTemplate.MetadataFields.Add("PreferredModelType", new Field() { FieldType = FieldType.Text, Name = "PreferredModelType" });
                    }
                }
            }
            ((Field)page.PageTemplate.MetadataFields["PreferredModelType"]).Values = new List<string>() { preferredModelType.FullName };
        }
        private static object locker = new object();

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <returns>IPage.</returns>
        public IPage GetPage(TcmUri pageId)
        {
            var page = _pageFactory.GetPage(pageId.ToString());
            return page;
        }

        /// <summary>
        /// Gets the content of the page.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        /// <remarks>This call is cached but the cached version is never invalidated if the page is unpublished or republished</remarks>
        public string GetPageContent(string url)
        {
            _logger.Debug("GetPageContent - Processing request for page: {0}", url);
            var currentUrl = url.ParseUrl(_publicationResolver.GetBaseUri().AbsolutePath, false, _configuration.WelcomeFile);
            string pageContent;
            if (_pageFactory.TryFindPageContent(currentUrl, out pageContent))
            {
                return pageContent;
            }
            var alternativeUrl = url.ParseUrl(_publicationResolver.GetBaseUri().AbsolutePath, true, _configuration.WelcomeFile);
            if (alternativeUrl != currentUrl)
            {
                if (_pageFactory.TryFindPageContent(currentUrl, out pageContent))
                {
                    return pageContent;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the page last published date.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>DateTime.</returns>
        public DateTime GetPageLastPublishedDate(string url)
        {
            return _pageFactory.GetLastPublishedDateByUrl(url);
        }

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>IViewModel.</returns>
        public IViewModel BuildViewModel(IModel model)
        {
            if (model == null)
                return null;
            string cacheKey = null;
            List<string> dependencies = new List<string>();
            bool itemIsCachable = true;
            if (model is IPage)
            {
                if (string.IsNullOrEmpty(((IPage)model).Id))
                {
                    itemIsCachable = false; // page without a unique identifier cannot be cached
                }
                else
                {
                    cacheKey = String.Format(_cacheKeyFormat, ((IPage)model).Id);
                    dependencies.Add(((IPage)model).Id);
                }
            }
            else if (model is IComponentPresentation)
            {
                var componentId = ((IComponentPresentation)model).Component.Id;
                var templateId = ((IComponentPresentation)model).ComponentTemplate.Id;
                if (string.IsNullOrEmpty(componentId))
                {
                    itemIsCachable = false; // component presentation without a unique identifier cannot be cached
                }
                else
                {
                    cacheKey = String.Format(_cacheKeyFormat, componentId + "_" + templateId);
                    dependencies.Add(((IComponentPresentation)model).Component.Id);
                }
            }
            if (!itemIsCachable)
            {
                return _viewModelFactory.BuildViewModel(model);
            }

            IViewModel viewModel = (IViewModel)_cacheAgent.Load(cacheKey);

            if (viewModel != null)
                return viewModel;

            viewModel = _viewModelFactory.BuildViewModel(model);

            if (viewModel != null)
            {
                _cacheAgent.Store(cacheKey, _cacheRegion, viewModel, dependencies);
            }

            return viewModel;

        }

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>IViewModel.</returns>
        public IViewModel BuildViewModel(string url, Type preferredModelType = null)
        {
            var page = GetPage(url);
            AddModelTypeToPage(page, preferredModelType);
            return BuildViewModel(page);
        }

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IViewModel.</returns>
        public IViewModel BuildViewModel(TcmUri Id, Type preferredModelType = null)
        {
            if (Id.ItemTypeId == 64)
            {
                var page = GetPage(Id);
                AddModelTypeToPage(page, preferredModelType);
                return BuildViewModel(page);
            }
            else
            {
                IComponentPresentation component;
                if (TryGetComponentPresentation(out component, Id.ToString()))
                {
                    return BuildViewModel(component);
                }
            }
            return null;
        }
    }
}