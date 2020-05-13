using System;
using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;

namespace Dyndle.Modules.Core.Providers.Content
{
    /// <summary>
    /// Interface IContentProvider
    /// Provides a generic way to Query the broker for dynamic components, pages or keywords.
    /// </summary>
    public interface IContentProvider
    {
        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>IPage.</returns>
        IPage GetPage(string url);

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <returns>IPage.</returns>
        IPage GetPage(TcmUri pageId);

        /// <summary>
        /// Gets the page last published date.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>DateTime.</returns>
        DateTime GetPageLastPublishedDate(string url);

        /// <summary>
        /// Gets the content of the page.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        string GetPageContent(string url);

        /// <summary>
        /// Gets the component presentation.
        /// </summary>
        /// <param name="componentId">The component identifier.</param>
        /// <param name="templateId">The template identifier.</param>
        /// <returns>IComponentPresentation.</returns>
        IComponentPresentation GetComponentPresentation(string componentId, string templateId = "");

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="preferredModelType">Type of the preferred model.</param>
        /// <returns>IViewModel.</returns>
        IViewModel BuildViewModel(TcmUri Id, Type preferredModelType = null);

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>IViewModel.</returns>
        IViewModel BuildViewModel(IModel model);

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="preferredModelType">Type of the preferred model.</param>
        /// <returns>IViewModel.</returns>
        IViewModel BuildViewModel(string url, Type preferredModelType = null);
    }
}