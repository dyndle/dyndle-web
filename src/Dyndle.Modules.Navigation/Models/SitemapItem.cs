using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Navigation.Models
{
    /// <summary>
    /// Class SitemapItem.
    /// Used for describing one level in the sitemap hierarchy
    /// </summary>
    /// <seealso cref="ISitemapItem" />
    public class SitemapItem : EntityModel, ISitemapItem
    {
        private static readonly Regex ReHasSubtype = new Regex("^\\[[a-zA-Z]\\] (?=\\w)");

        private readonly XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>The change frequency.</value>
        [TextField]
        public string ChangeFrequency { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [TextField]
        public new string Id { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [EmbeddedSchemaField(FieldName = "childItems")]
        public virtual List<SitemapItem> Items { get; set; }

        /// <inheritdoc />
        [DateField(FieldName = "lastMod")]
        public DateTime LastMod { get; set; }

        /// <summary>
        /// Gets or sets the meta robot value ((no)index, (no)follow).
        /// </summary>
        /// <value>The meta robot.</value>
        [TextField]
        public string MetaRobot { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        [TextField]
        public string Priority { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [TextField]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Prefix used for the Title of this item in the CMS
        /// </summary>
        /// <value>The Prefix.</value>
        [TextField]
        public string TitlePrefix { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Id) && Id.EndsWith("-64") ? "page" : "structuregroup";
            }
            set
            {
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [TextField]
        public string Url { get; set; }

        //                fieldSet.Add(FieldNames.changeFrequency.ToString(), CreateField(FieldNames.changeFrequency.ToString(), DEFAULT_CHANGE_FREQUENCY));
        //                Log.Debug(string.Format("Added field {0} with default value {1}", FieldNames.changeFrequency.ToString(), DEFAULT_CHANGE_FREQUENCY));

        //                fieldSet.Add(FieldNames.priority.ToString(), CreateField(FieldNames.priority.ToString(), DEFAULT_PRIORITY));
        //                Log.Debug(string.Format("Added field {0} with value {1}", FieldNames.priority.ToString(), DEFAULT_PRIORITY));

        //                fieldSet.Add(FieldNames.metaRobot.ToString(), CreateField(FieldNames.metaRobot.ToString(), DEFAULT_META_ROBOT));
        //                Log.Debug(string.Format("Added field {0} with value {1}", FieldNames.metaRobot.ToString(), DEFAULT_META_ROBOT));
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISitemapItem" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        [TextField(IsBooleanValue = true)]
        public bool Visible { get; set; }

        /// <summary>
        /// Cleans all urls.
        /// </summary>
        /// <param name="defaultFileName">Default name of the file.</param>
        public void CleanAllUrls(string defaultFileName)
        {
            this.Url = this.Url.CleanUrl(defaultFileName);

            Items?.ForEach(i => i.CleanAllUrls(defaultFileName));
        }

        /// <summary>
        /// Cleans up the site map by removing empty structure groups and removing deeper levels (based on maximum levels specified).
        /// </summary>
        /// <param name="maxLevels">The maximum levels. (value of 0 or less means: do not apply a maximum)</param>
        public void CleanUpSitemap(int maxLevels)
        {
            // this.Items.RemoveAll(i => i.Type.Equals("structuregroup", StringComparison.InvariantCultureIgnoreCase) && !i.Items.Any());

            if (maxLevels == 1)
            {
                this.Items?.ForEach(i => i.Items?.Clear());
                return;
            }
            maxLevels--;
            this.Items?.ForEach(i => i.CleanUpSitemap(maxLevels));
        }

        /// <summary>
        /// Filters the sitemap by the specified subtype.
        /// </summary>
        /// <param name="navSubtype">Navigation subtype</param>
        public void FilterBySubtype(string navSubtype)
        {
            if (navSubtype == "none")
            {
                this.Items?.RemoveAll(i => (ReHasSubtype.IsMatch(i.Title)));
            }
            else
            {
                this.Items?.RemoveAll(i => !(i.Title.StartsWith("[" + navSubtype + "] ")));
            }
            this.Items?.ForEach(i => i.Title = ReHasSubtype.Replace(i.Title, ""));
        }

        /// <summary>
        /// Finds the by URL.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns>ISitemapItem.</returns>
        public ISitemapItem FindByUrl(string requestUrl)
        {
            if (this.Url.Equals(requestUrl, StringComparison.InvariantCultureIgnoreCase))
                return this;

            if (!requestUrl.StartsWith(this.Url, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return this.Items
                .Select(a => a.FindByUrl(requestUrl))
                .FirstOrDefault(a => !a.IsNull());
        }

        /// <summary>
        /// Return a the groupid based on TitlePrefix value
        /// </summary>
        /// <returns>string</returns>
        public string GetGroupId()
        {
            var match = Regex.Match(this.TitlePrefix, @"\d(\d)\d");
            return match.Success ? match.Groups[1].Value : "0";
        }

        /// <summary>
        /// Return a XMLDocument with sitemap nodes
        /// </summary>
        /// <param name="baseUri">base uri for links in the sitemap</param>
        /// <returns>XDocument</returns>
        public XDocument GetSitemapXmlDocument(Uri baseUri)
        {
            XElement root = new XElement(xmlns + "urlset");
            var baseUrl = $"{baseUri.Scheme}://{baseUri.Host}";
            var rootNode = this.CreateSitemapNodeElement(this, baseUrl);
            root.Add(rootNode);
            this.GenerateSitemapNodes(root, this, baseUrl);

            XDocument document = new XDocument(root);
            return document;
        }

        /// <summary>
        /// Prepares the breadcrumb.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="parentUrl">The parent URL.</param>
        public void PrepareBreadcrumb(string requestUrl, string parentUrl = null)
        {
            var item = Items?.FirstOrDefault(a => requestUrl.StartsWith(a.Url, StringComparison.InvariantCultureIgnoreCase)
                                                  && a.Url != parentUrl);

            if (item == null && Items != null && Items.Any())
            {
                Items.RemoveAll(a => true);
                return;
            }

            if (item == null)
            {
                return;
            }

            Items.RemoveAll(a => a != item);
            item.PrepareBreadcrumb(requestUrl, item.Url);
        }

        /// <summary>
        /// Create XElement node for a sitemap item
        /// </summary>
        /// <param name="sitemapNode"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private XElement CreateSitemapNodeElement(SitemapItem sitemapNode, string baseUrl)
        {
            return new XElement(
                xmlns + "url",
                new XElement(xmlns + "loc", $"{baseUrl}{sitemapNode.Url}"),
                new XElement(
                    xmlns + "lastmod",
                    sitemapNode.LastMod.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                new XElement(
                    xmlns + "changefreq",
                    sitemapNode.ChangeFrequency),
                new XElement(
                    xmlns + "priority",
                    sitemapNode.Priority));
        }

        /// <summary>
        /// recursive method to construct sitemap items
        /// </summary>
        /// <param name="root"></param>
        /// <param name="sitemap"></param>
        /// <param name="baseUrl"></param>
        private void GenerateSitemapNodes(XElement root, SitemapItem sitemap, string baseUrl)
        {
            // show all types except pages. it could be that an overload implementation add different type's to the nav.
            // i.e. Product type
            foreach (var sitemapNode in sitemap.Items)
            {
                var urlElement = this.CreateSitemapNodeElement(sitemapNode, baseUrl);
                root.Add(urlElement);
                if (sitemapNode.Items != null && sitemapNode.Items.Any())
                {
                    this.GenerateSitemapNodes(root, sitemapNode, baseUrl);
                }
            }
        }
    }
}