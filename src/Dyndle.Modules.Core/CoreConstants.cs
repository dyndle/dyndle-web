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

        public static class Configuration
        {
            public const string EnableCache = "Dyndle.EnableCache";
            public const string PublicationId = "DD4T.PublicationId";
            public const string PublicationBaseUrl = "DD4T.PublicationBaseUrl";
            public const string RedirectsUrl = "Dyndle.RedirectsUrl";
            public const string ViewModelNamespaces = "Dyndle.ViewModelNamespaces";
            public const string ControllerNamespaces = "Dyndle.ControllerNamespaces";
            public const string StagingSite = "Dyndle.StagingSite";
            public const string SiteConfigUrl = "Dyndle.SiteConfigUrl";
            public const string IncludesUrl = "Dyndle.IncludesUrl";
            public const string IncludesUrlMask = "Dyndle.IncludesUrlMask";
            public const string DefaultIncludesView = "Dyndle.DefaultIncludesView";
            public const string DefaultRegionView = "Dyndle.DefaultRegionView";
            public const string ContentManagerUrl = "Dyndle.ContentManagerUrl";
            public const string DirectorySegmentsUsedForPublicationMapping = "Dyndle.DirectorySegmentsUsedForPublicationMapping";
            public const string DefaultEntityTypeName = "Dyndle.DefaultEntityTypeName";
            public const string EnableRedirects = "Dyndle.EnableRedirects";
            public const string EnableSectionErrors = "Dyndle.EnableSectionErrors";
            public const string ThrowNotFound = "Dyndle.ThrowNotFound";
            public const string ErrorPageMask = "Dyndle.ErrorPages.{0}";
            public const string LogRegistrationsOnStartup = "Dyndle.LogRegistrationsOnStartup";
            public const string BinaryCacheFolder = "Dyndle.BinaryCacheFolder";
            public const string BinaryUrlPattern = "Dyndle.BinaryUrlPattern";
            public const string DisableOutputCachingForUrls = "Dyndle.DisableOutputCachingForUrls";            
        }


        public static class General
        {
            public const string ECLExtensionMeta = "ECL-ExternalMetadata";
            public const string CDNPathKey = "CDNPaths";
            public const string XpmPageTagKey = "XpmPageTag";
            public const string DependentOnUris = "DependentOnUris";
        }
    }
}