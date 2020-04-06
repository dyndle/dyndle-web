namespace Dyndle.Modules.Navigation
{
    /// <summary>
    /// Constants used in the navigation module
    /// </summary>
    public static class NavigationConstants
    {
        /// <summary>
        /// Names of settings in the config file
        /// </summary>
        public static class Settings
        {
            /// <summary>
            /// Setting that determines whether to include all pages in sitemap
            /// </summary>
            public readonly static string IncludeAllPagesInSitemap = "Navigation.IncludeAllPagesInSitemap";

            /// <summary>
            /// Setting that determines the url of the navigation source page
            /// </summary>
            public readonly static string NavigationPath = "Navigation.SourceUrl";

            /// <summary>
            /// Setting that determines the navigation base url with which the URLs of the pages are prefixed in the sitemap.xml
            /// </summary>
            public readonly static string NavigationBaseUrl = "Navigation.BaseUrl";

            /// <summary>
            /// If true, the subnavigation is read from the root for pages whose level falls below the start level
            /// </summary>
            public readonly static string SubNavDefaultsToMainNav = "Navigation.SubNavDefaultsToMainNav";
        }

        /// <summary>
        /// Possible entries in the RouteValues table
        /// </summary>
        public static class RouteValues
        {
            /// <summary>
            /// The navigation type
            /// </summary>
            public readonly static string NavType = "navType";

            /// <summary>
            /// The navigation levels
            /// </summary>
            public readonly static string NavLevels = "navLevels";

            /// <summary>
            /// The navigation start level
            /// </summary>
            public readonly static string NavStartLevel = "navStartLevel";

            /// <summary>
            /// The navigation subtype (e.g. to distinguish primary and secondary navigation)
            /// </summary>
            public readonly static string NavSubtype = "navSubtype";
        }

        /// <summary>
        /// The default
        /// </summary>
        /// <summary>
        /// The children
        /// </summary>
        /// <summary>
        /// The path
        /// </summary>
        /// <summary>
        /// The sitemap
        /// </summary>
        public enum NavigationType { Default, Children, Path, Sitemap }

    }
}
