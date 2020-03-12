using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DD4T.ContentModel.Factories;
using Trivident.Modules.Core.Models;
using Trivident.Modules.Core.Providers.Content;

namespace Trivident.Modules.Core.Services.Preview
{
    /// <summary>
    /// Create Viewmodel based on json data.
    /// </summary>
    public class PreviewContentService : IPreviewContentService
    {
        private readonly IPageFactory _pageFactory;
        private readonly IContentProvider _contentProvider;
        /// <summary>
        /// creates a new instance of PreviewContentService
        /// </summary>
        /// <param name="pageFactory"></param>
        /// <param name="contentProvider"></param>
        public PreviewContentService(IPageFactory pageFactory, IContentProvider contentProvider)
        {
            pageFactory.ThrowIfNull(nameof(pageFactory));
            contentProvider.ThrowIfNull(nameof(contentProvider));

            _pageFactory = pageFactory;
            _contentProvider = contentProvider;
        }

        /// <summary>
        /// use _pageFactory to descialize data into a IViewModel.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public IWebPage GetPage(string data, string url)
        {
            var page = _pageFactory.GetIPageObject(data);
            var model = _contentProvider.BuildViewModel(page) as IWebPage;
            return model;
        }
    }
}