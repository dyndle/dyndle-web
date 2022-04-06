using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DD4T.Core.Contracts.ViewModels;

namespace Dyndle.Modules.Navigation.Models
{
    /// <summary>
    /// Interface ISitemapItem
    /// Used for describing one level in the sitemap hierarchy
    /// </summary>
    public interface ISitemapItem : IViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        List<SitemapItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        DateTime LastMod { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISitemapItem" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets Prefix used for the Title of this item in the CMS
        /// </summary>
        /// <value>The Prefix.</value>
        string TitlePrefix { get; set; }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>The change frequency.</value>
        string ChangeFrequency { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        string Priority { get; set; }

        /// <summary>
        /// Gets or sets the meta robot value ((no)index, (no)follow).
        /// </summary>
        /// <value>The meta robot.</value>
        string MetaRobot { get; set; }


        /// <summary>
        /// Removes the site map.
        /// </summary>
        /// <param name="maxLevels">The maximum levels.</param>
        void CleanUpSitemap(int maxLevels);

        /// <summary>
        /// Finds the by URL.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns>ISitemapItem.</returns>
        ISitemapItem FindByUrl(string requestUrl);

        /// <summary>
        /// Prepares the breadcrumb.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="parentUrl">The parent URL.</param>
        void PrepareBreadcrumb(string requestUrl, string parentUrl = null);

        /// <summary>
        /// Return a XMLDocument wiht sitemap nodes
        /// </summary>
        /// <param name="baseUri">base uri for links in the sitemap</param>
        /// <returns>XDocument</returns>
        XDocument GetSitemapXmlDocument(Uri baseUri);


        /// <summary>
        /// Return a the groupid based on TitlePrefix value
        /// </summary>
        /// <returns>string</returns>
        string GetGroupId();

        /// <summary>
        /// Filters the sitemap by the specified subtype.
        /// </summary>
        /// <param name="navSubtype">Type of the nav sub.</param>
        void FilterBySubtype(string navSubtype);

        /// <summary>
        /// Cleans all urls.
        /// </summary>
        /// <param name="defaultFileName">Default name of the file.</param>
        void CleanAllUrls(string defaultFileName);
    }
}