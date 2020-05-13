using Dyndle.Modules.Navigation.Models;

namespace Dyndle.Modules.Navigation.Services
{
    /// <summary>
    /// Interface INavigationService
    /// Used to describe a service that can retrive navigation information
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Gets the navigation model.
        /// </summary>
        /// <param name="requestUrlPath">The request path to use when fetching the navigation</param>
        /// <param name="navType">Type of navigation that is requested</param>
        /// <param name="navSubtype">Subtype of the navigation that is being requested (default is "none")</param>
        /// <param name="navLevels">The number of levels of the navigation to fetch</param>
        /// <param name="navStartLevel">The starting level of the navigation</param>
        /// <returns>ISitemapItem</returns>
        ISitemapItem GetNavigationModel(
            string requestUrlPath = "",
            NavigationConstants.NavigationType navType = NavigationConstants.NavigationType.Default,
            string navSubtype = "none",
            int navLevels = 0,
            int navStartLevel = -1);
    }
}