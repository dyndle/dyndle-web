namespace Dyndle.Modules.Core
{
    public static class CoreConstants
    {
        public static class Labels
        {
            //public readonly static string Sample = "Sample";
        }

        public static class Settings
        {
            //public readonly static string Sample = "Sample";
        }

        /// <summary>
        /// Class which contains the Dyndle configuration keys
        /// </summary>
        public static class Configuration
        {
            /// <summary>
            /// Configuration key to enable the cache (values: true or false)
            /// </summary>
            public const string EnableCache = "Dyndle.EnableCache";
            /// <summary>
            /// Configuration key containing the publication you want to serve pages and other content from. If left out, topology manager mappings are used.
            /// </summary>
            public const string PublicationId = "DD4T.PublicationId";
            /// <summary>
            /// Configuration key containing the base path of the publication (used only in combination with the DD4T.PublicationId appSetting)
            /// </summary>
            public const string PublicationBasePath = "Dyndle.PublicationBasePath";
            /// <summary>
            /// Configuration key containing the namespaces which contain viewmodel classes (multiple namespaces can be comma-separated)
            /// </summary>
            public const string ViewModelNamespaces = "Dyndle.ViewModelNamespaces";
            /// <summary>
            /// Configuration key containing the namespaces which contain controller classes (multiple namespaces can be comma-separated)
            /// </summary>
            public const string ControllerNamespaces = "Dyndle.ControllerNamespaces";
            /// <summary>
            /// Configuration key indicating whether or not we are dealing with a staging website (set to true or false, this will enable/disable the debugging and XPM functionality)
            /// </summary>
            public const string StagingSite = "Dyndle.StagingSite";
            /// <summary>
            /// Configuration key containing the path to the site configuration page (e.g. /system/site-configuration.html)
            /// </summary>
            public const string SiteConfigUrl = "Dyndle.SiteConfigUrl";
            /// <summary>
            /// Configuration key containing the path(s) to include pages (e.g. /system/header.html, /system/footer.html)
            /// </summary>
            public const string IncludesUrl = "Dyndle.IncludesUrl";
            /// <summary>
            /// Configuration key containing the url mask used in case you are calling Html.RenderIncludes with an include name
            /// </summary>
            public const string IncludesUrlMask = "Dyndle.IncludesUrlMask";
            /// <summary>
            /// Configuration key containing the name of the default view to use when Html.RenderIncludes is called
            /// </summary>
            public const string DefaultIncludesView = "Dyndle.DefaultIncludesView";
            /// <summary>
            /// Configuration key containing the name of the default view to use when Html.RenderRegion or Html.RenderRegions is called
            /// </summary>
            public const string DefaultRegionView = "Dyndle.DefaultRegionView";
            /// <summary>
            /// Configuration key containing the url of the content manager (used for Experience Manager)
            /// </summary>
            public const string ContentManagerUrl = "Dyndle.ContentManagerUrl";
            /// <summary>
            /// Configuration key containing the number of segments used to detect the publication we are in, based on the topology manager mappings 
            /// </summary>
            public const string DirectorySegmentsUsedForPublicationMapping = "Dyndle.DirectorySegmentsUsedForPublicationMapping";
            /// <summary>
            /// Full type name (including the namespace) of the default entity (which is returned if no matching entity model is found). Defaults to Dyndle.Modules.Core.Models.Defaults.DefaultEntity) 
            /// </summary>
            public const string DefaultEntityTypeName = "Dyndle.DefaultEntityTypeName";
            /// <summary>
            /// Full type name (including the namespace) of the default webpage (which is returned if no matching page model is found). Defaults to Dyndle.Modules.Core.Models.Defaults.DefaultWebPage) 
            /// </summary>
            public const string DefaultWebPageTypeName = "Dyndle.DefaultWebPageTypeName";
            /// <summary>
            /// Configuration key to enable or disable redirect functionality
            /// </summary>
            public const string EnableRedirects = "Dyndle.EnableRedirects";
            /// <summary>
            /// Configuration key containing the path to the Dyndle redirects page managed in Tridion (e.g. /system/redirects.html)
            /// </summary>
            public const string RedirectsUrl = "Dyndle.RedirectsUrl";
            /// <summary>
            /// Configuration key to indicate whether or not you want to see errors per region or entity (rather than one big error message per page). Values: true or false
            /// </summary>
            public const string EnableSectionErrors = "Dyndle.EnableSectionErrors";
            /// <summary>
            /// Obsolete
            /// </summary>
            public const string ThrowNotFound = "Dyndle.ThrowNotFound";
            /// <summary>
            /// Mask to specify different configuration keys to allow you to configure a different error page per http status
            /// Currently only 404 and 500 are supported. You need to create appSettings with key 'Dyndle.ErrorPages.404' and/or 'Dyndle.ErrorPages.500'
            /// </summary>
            public const string ErrorPageMask = "Dyndle.ErrorPages.{0}";
            /// <summary>
            /// Log all DI registrations at application startup
            /// </summary>
            public const string LogRegistrationsOnStartup = "Dyndle.LogRegistrationsOnStartup";
            /// <summary>
            /// Configuration key containing the name of the folder where binaries are to be cached on the file system (default = binarydata)
            /// </summary>
            public const string BinaryCacheFolder = "Dyndle.BinaryCacheFolder";
            /// <summary>
            /// Configuration key indicating the regular expression (pattern) to match all binary files. Matching URLs will be handled by the BinaryController instead of the PageController
            /// </summary>
            public const string BinaryUrlPattern = "Dyndle.BinaryUrlPattern";
            /// <summary>
            /// Configuration key containing the urls (or rather, paths) for which output caching is disabled. Multiple urls can be entered, separated by a comma
            /// Example: /search,/some/other/path
            /// </summary>
            public const string DisableOutputCachingForUrls = "Dyndle.DisableOutputCachingForUrls";            
        }


        /// <summary>
        /// Class containing general settings used by this application
        /// </summary>
        public static class General
        {
            /// <summary>
            /// Name of the metadata field containing ECL extension metadata
            /// </summary>
            public const string ECLExtensionMeta = "ECL-ExternalMetadata";
            /// <summary>
            /// Name of the ECL metadata field containing CDNPaths
            /// </summary>
            public const string CDNPathKey = "CDNPaths";
            /// <summary>
            /// Name of the HttpContext variable containing the URIs of Tridion items on which the current page depends (used as an internal mechanism in Dyndle)
            /// </summary>
            public const string DependentOnUris = "DependentOnUris";
        }
    }
}