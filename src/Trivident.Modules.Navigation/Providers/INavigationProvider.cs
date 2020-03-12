using Trivident.Modules.Navigation.Models;

namespace Trivident.Modules.Navigation.Providers
{
    /// <summary>
    /// Interface INavigationProvider
    /// Creates a model to use in different types of navigation
    /// </summary>
    public interface INavigationProvider
    {
        /// <summary>
        /// Gets the Navigation Model (Sitemap) for a given Localization.
        /// </summary>
        /// <param name="levels">The number of levels to return.</param>
        /// <param name="navSubtype">Type of the nav sub.</param>
        /// <returns>
        /// The Navigation Model (Sitemap root Item).
        /// </returns>
        ISitemapItem GetAll(int levels = 0, string navSubtype = null);

        /// <summary>
        /// Gets the children starting from an ancestor at specified level.
        /// If startLevel -1 is provided the current page level is used.
        /// </summary>
        /// <param name="requestUrlPath">The request URL path.</param>
        /// <param name="levels">The number of levels to return.</param>
        /// <param name="startLevel">The start level. If startLevel -1 is provided the current page level is used
        /// Otherwise starting from an ancestor of the current page at specified level.</param>
        /// <param name="navSubtype">Navigation subtype.</param>
        /// <returns>
        /// ISitemapItem.
        /// </returns>
        ISitemapItem GetChildren(string requestUrlPath, int levels = 0, int startLevel = 0, string navSubtype = null);

        /// <summary>
        /// Gets the path for the current URL.
        /// </summary>
        /// <param name="requestUrlPath">The request URL path.</param>
        /// <returns>ISitemapItem.</returns>
        ISitemapItem GetPath(string requestUrlPath);

        /// <summary>
        /// Loads complete sitemap from Tridion
        /// </summary>
        /// <returns>ISitemapItem.</returns>
        ISitemapItem GetFullSitemap();
    }
}