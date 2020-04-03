using System;
using System.Collections.Generic;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Providers.Configuration;

namespace Dyndle.Modules.Core.Environment
{
    /// <summary>
    /// Class SiteContext.
    /// Implements the ISiteContext and gets all needed information from Tridion
    /// </summary>
    /// <seealso cref="ISiteContext" />
    public class SiteContext : ISiteContext
    {
        /// <summary>
        /// The site configuration provider
        /// </summary>
        private readonly ISiteConfigurationProvider _siteConfigurationProvider;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        private const string LABELPREFIX = "Label.{0}";
        private const string SCHEMATITLEPREFIX = "SchemaTitle.{0}";
        private const string TEMPLATETITLEPREFIX = "View.{0}";
        private const string SCHEMAROOTELEMENTNAMEPREFIX = "SchemaRootElementName.{0}";
        private const string CATEGORYTITLEPREFIX = "CategoryTitle.{0}";
        private const string CATEGORYIDPREFIX = "CategoryId.{0}";
        private const string APPLICATIONSETTINGPREFIX = "Config.{0}";
        private const string LabelNotFound = "Label with the key: {0} not found.";

        private const string StagingTargetSuffix = "_Staging";

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteContext"/> class.
        /// </summary>
        /// <param name="siteConfigurationProvider">The site configuration provider.</param>
        /// <param name="logger">The logger.</param>
        public SiteContext(ISiteConfigurationProvider siteConfigurationProvider, ILogger logger)
        {
            siteConfigurationProvider.ThrowIfNull(nameof(siteConfigurationProvider));
            logger.ThrowIfNull(nameof(logger));

            _logger = logger;
            _siteConfigurationProvider = siteConfigurationProvider;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        private IDictionary<string, string> Configuration => _siteConfigurationProvider.RetrieveConfiguration();

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="isMandatory">if set to <c>true</c> [is mandatory] Throws an exception when the value is missing.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Exception"></exception>
        public string GetApplicationSetting(string key, bool isMandatory = false)
        {
            var tryTargetSpecific = true;    // could be passed as parameter in future, for now hardcoded so no change to interface method
            var baseKey = APPLICATIONSETTINGPREFIX.FormatString(key);
            if (tryTargetSpecific && IsStaging())
            {
                // first try to use specific key
                var targetSpecificValue = GetConfigurationValue(baseKey + StagingTargetSuffix);
                if (!string.IsNullOrEmpty(targetSpecificValue))
                {
                    return targetSpecificValue;
                }
            }

            // if we arrived here, try to use base key
            var value = GetConfigurationValue(baseKey);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            // no value could be found, if mandatory throw exception
            if (isMandatory)
            {
                throw new Exception($"Missing Application Setting: {key}");
            }
            return null;
        }

        public string GetRawConfigurationValue(string key)
        {
            return GetConfigurationValue(key);
        }

        /// <summary>
        /// Returns the value for the specified configuration key
        /// if not found, returns empty string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigurationValue(string key)
        {
            string value;
            if (Configuration.TryGetValue(key, out value) && !string.IsNullOrWhiteSpace(value))
            {
                _logger.Debug($"found application setting  key: {key}, value: {value}");
                return value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="nullIfMissing">If true return null if label missing, otherwise return missing label message</param>
        /// <returns>System.String.</returns>
        public string GetLabel(string key, bool nullIfMissing = false )
        {
            _logger.Debug("about to get label value for key: {0}".FormatString(key));
            var localKey = LABELPREFIX.FormatString(key);
            string value;
            if (Configuration.TryGetValue(localKey, out value))
            {
                _logger.Debug("found label  key: {0}, value: {1}".FormatString(key, value));
                return value;
            }
            return nullIfMissing ? null : LabelNotFound.FormatString(key);
        }

        /// <summary>
        /// Gets the name of the schema identifier by root element.
        /// </summary>
        /// <param name="rootElementName">Name of the root element.</param>
        /// <returns>TcmUri.</returns>
        public TcmUri GetSchemaIdByRootElementName(string rootElementName)
        {
            _logger.Debug("about to get schema rootElementName for key: {0}".FormatString(rootElementName));
            var localRootElementName = SCHEMAROOTELEMENTNAMEPREFIX.FormatString(rootElementName);
            string value;
            if (Configuration.TryGetValue(localRootElementName, out value))
                return new TcmUri(value);

            return null;
        }

        /// <summary>
        /// Gets the schema identifier by title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>TcmUri.</returns>
        public TcmUri GetSchemaIdByTitle(string title)
        {
            _logger.Debug("about to get schema Title for key: {0}".FormatString(title));
            var localTitle = SCHEMATITLEPREFIX.FormatString(title);
            string value;
            if (Configuration.TryGetValue(localTitle, out value))
                return new TcmUri(value);

            return null;
        }

        /// <summary>
        /// Gets the template identifier by title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>TcmUri.</returns>
        public TcmUri GetTemplateIdByTitle(string title)
        {
            _logger.Debug("about to get Template Title for key: {0}".FormatString(title));
            var localTitle = TEMPLATETITLEPREFIX.FormatString(title);
            string value;
            if (Configuration.TryGetValue(localTitle, out value))
                return new TcmUri(value);

            return null;
        }

        /// <summary>
        /// Gets the name of the category title by XML.
        /// </summary>
        /// <param name="xmlName">Name of the XML.</param>
        /// <returns>System.String.</returns>
        public string GetCategoryTitleByXmlName(string xmlName)
        {
            _logger.Debug("about to get category Title for xml name: {0}".FormatString(xmlName));
            var localTitle = CATEGORYTITLEPREFIX.FormatString(xmlName);
            string value;
            if (Configuration.TryGetValue(localTitle, out value))
                return value;

            return null;
        }

        /// <summary>
        /// Gets the name of the category identifier by XML.
        /// </summary>
        /// <param name="xmlName">Name of the XML.</param>
        /// <returns>System.String.</returns>
        public string GetCategoryIdByXmlName(string xmlName)
        {
            _logger.Debug("about to get category ID for xml name: {0}".FormatString(xmlName));
            var localTitle = CATEGORYIDPREFIX.FormatString(xmlName);
            string value;
            if (Configuration.TryGetValue(localTitle, out value))
                return value;

            return null;
        }

        /// <summary>
        /// Determine if the current site is staging
        /// </summary>
        /// <returns>true if this is a staging site</returns>
        public bool IsStaging()
        {
            return DyndleConfig.IsStagingSite;
        }
    }
}