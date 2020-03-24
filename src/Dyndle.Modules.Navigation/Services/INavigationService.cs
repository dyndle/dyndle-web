using Dyndle.Modules.Navigation.Models;

namespace Dyndle.Modules.Navigation.Services
{
    public interface INavigationService
    {
        ISitemapItem GetNavigationModel(
            string requestUrlPath = "",
            NavigationConstants.NavigationType navType = NavigationConstants.NavigationType.Default,
            string navSubtype = "none",
            int navLevels = 0,
            int navStartLevel = -1);
    }
}