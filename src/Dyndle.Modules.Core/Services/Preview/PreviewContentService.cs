using DD4T.ContentModel.Factories;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Core.Services.Preview
{
    /// <summary>
    /// Create Viewmodel based on json data.
    /// </summary>
    public class PreviewContentService : IPreviewContentService
    {
        private readonly IPageFactory _pageFactory;
        private readonly IContentProvider _contentProvider;
        /// <summary>
        /// Initializes a new instance of the <see cref="DyndleOutputCacheAttribute"/> class.
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