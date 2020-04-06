using System.Configuration;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Configuration
{
    /// <summary>
    /// Wrapper around all Dyndle configuration settings
    /// </summary>
    public static class DyndleConfig
    {
        /// <summary>
        /// Returns configuration appSetting 'ThrowNotFound' as boolean
        /// </summary>
        public static bool ThrowNotFound => CoreConstants.Configuration.ThrowNotFound.GetConfigurationValueAsBoolean();
        /// <summary>
        /// Returns configuration appSetting 'EnableCache' as boolean
        /// </summary>
        public static bool IsCacheEnabled => CoreConstants.Configuration.EnableCache.GetConfigurationValueAsBoolean();
        /// <summary>
        /// Returns configuration appSetting 'StagingSite' as boolean
        /// </summary>
        public static bool IsStagingSite => CoreConstants.Configuration.StagingSite.GetConfigurationValueAsBoolean();
        /// <summary>
        /// Returns configuration appSetting 'PublicationBaseUrl' as string
        /// </summary>
        public static string PublicationBasePath => CoreConstants.Configuration.PublicationBasePath.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'BinaryUrlPattern' as string
        /// </summary>
        public static string BinaryUrlPattern => CoreConstants.Configuration.BinaryUrlPattern.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'RedirectsUrl' as string
        /// </summary>
        public static string RedirectsUrl => CoreConstants.Configuration.RedirectsUrl.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'ViewModelNamespace' as string
        /// </summary>
        public static string ViewModelNamespaces => CoreConstants.Configuration.ViewModelNamespaces.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'ControllerNamespace' as string
        /// </summary>
        public static string ControllerNamespaces => CoreConstants.Configuration.ControllerNamespaces.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'SiteConfigUrl' as string
        /// </summary>
        public static string SiteConfigUrl => CoreConstants.Configuration.SiteConfigUrl.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'IncludesUrl' as string
        /// </summary>
        public static string IncludesUrl => CoreConstants.Configuration.IncludesUrl.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'IncludesUrlMask' as string
        /// </summary>
        public static string IncludesUrlMask => CoreConstants.Configuration.IncludesUrlMask.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'DefaultIncludesView' as string
        /// </summary>
        public static string DefaultIncludesView => CoreConstants.Configuration.DefaultIncludesView.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'DefaultRegionView' as string
        /// </summary>
        public static string DefaultRegionView => CoreConstants.Configuration.DefaultRegionView.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'ContentManagerUrl' as string
        /// </summary>
        public static string ContentManagerUrl => CoreConstants.Configuration.ContentManagerUrl.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'DefaultEntityTypeName' as string
        /// </summary>
        public static string DefaultEntityTypeName => CoreConstants.Configuration.DefaultEntityTypeName.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'DefaultWebPageTypeName' as string
        /// </summary>
        public static string DefaultWebPageTypeName => CoreConstants.Configuration.DefaultWebPageTypeName.GetConfigurationValue();
        /// <summary>
        /// Returns configuration appSetting 'EnableRedirects' as boolean
        /// </summary>
        public static bool EnableRedirects => CoreConstants.Configuration.EnableRedirects.GetConfigurationValueAsBoolean();
        /// <summary>
        /// Returns configuration appSetting 'EnableSectionErrors' as boolean
        /// </summary>
        public static bool EnableSectionErrors => CoreConstants.Configuration.EnableSectionErrors.GetConfigurationValueAsBoolean();
        /// <summary>
        /// Returns configuration appSetting 'LogRegistrationsOnStartup' as boolean
        /// </summary>
        public static bool LogRegistrationsOnStartup => CoreConstants.Configuration.LogRegistrationsOnStartup.GetConfigurationValueAsBoolean();

        /// <summary>
        /// Returns configuration appSetting 'DisableOutputCachingForUrls' as string
        /// </summary>
        public static string DisableOutputCachingForUrls => CoreConstants.Configuration.DisableOutputCachingForUrls.GetConfigurationValue();

        /// <summary>
        /// Gets or sets a value indicating whether [disable output caching].
        /// </summary>
        /// <value><c>true</c> if [disable output caching]; otherwise, <c>false</c>.</value>
        public static bool DisableOutputCaching
        {
            get; set;
        }


        /// <summary>
        /// Returns configuration appSetting 'ErrorPage' for specified status code
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>System.String.</returns>
        public static string GetErrorPageUrl(int statusCode)
        {
            var configuredUrl =   ConfigurationManager.AppSettings[string.Format(CoreConstants.Configuration.ErrorPageMask, statusCode)];
            if (string.IsNullOrEmpty(configuredUrl))
            {
                return Defaults.GetErrorPageUri(statusCode);
            }
            return configuredUrl;
        }

        /// <summary>
        /// Class Defaults.
        /// </summary>
        public static class Defaults
        {
            /// <summary>
            /// Gets the error page URI.
            /// </summary>
            /// <param name="statusCode">The status code.</param>
            /// <returns>System.String.</returns>
            public static string GetErrorPageUri(int statusCode)
            {
                return "/system/errors/" + statusCode;
            }
        }
    }
}
