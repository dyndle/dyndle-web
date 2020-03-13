namespace Dyndle.Modules.Core.Environment
{
    /// <summary>
    /// Interface ISiteContext
    /// Used for getting settings and mappings
    /// </summary>
    public interface ISiteContext
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="nullIfMissing">If true return null if label missing, otherwise return missing label message</param>
        /// <returns>System.String.</returns>
        string GetLabel(string key, bool nullIfMissing = false);

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
        /// <returns>System.String.</returns>
        string GetApplicationSetting(string key, bool isMandatory = false);

        /// <summary>
        /// Gets the schema identifier by title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>TcmUri.</returns>
        TcmUri GetSchemaIdByTitle(string title);

        /// <summary>
        /// Gets the name of the schema identifier by root element.
        /// </summary>
        /// <param name="rootElementName">Name of the root element.</param>
        /// <returns>TcmUri.</returns>
        TcmUri GetSchemaIdByRootElementName(string rootElementName);

        /// <summary>
        /// Gets the name of the category title by XML.
        /// </summary>
        /// <param name="xmlName">Name of the XML.</param>
        /// <returns>System.String.</returns>
        string GetCategoryTitleByXmlName(string xmlName);

        /// <summary>
        /// Gets the name of the category identifier by XML.
        /// </summary>
        /// <param name="xmlName">Name of the XML.</param>
        /// <returns>System.String.</returns>
        string GetCategoryIdByXmlName(string xmlName);

        /// <summary>
        /// Gets the template identifier by title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>TcmUri.</returns>
        TcmUri GetTemplateIdByTitle(string title);

        /// <summary>
        /// Indicates whether this is a staging site or not
        /// </summary>
        /// <returns>true if the site is staging</returns>
        bool IsStaging();

        /// <summary>
        /// Gets a raw configuration value (including prefix).
        /// </summary>
        /// <param name="key">The key to retrieve.</param>
        /// <returns>Configuration value.</returns>
        string GetRawConfigurationValue(string key);
    }
}