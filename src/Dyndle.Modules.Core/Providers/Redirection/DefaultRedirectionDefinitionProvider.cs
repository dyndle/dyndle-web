using System;
using System.Collections.Generic;
using System.Linq;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models.System;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Core.Providers.Redirection
{
    /// <summary>
    /// Class DefaultRedirectionDefinitionProvider.
    /// Implements logic to get all configured redirects from Tridion and converts them to the appropriate type of redirect definition
    /// </summary>
    /// <seealso cref="IRedirectionDefinitionProvider" />
    public class DefaultRedirectionDefinitionProvider : IRedirectionDefinitionProvider
    {
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;
        private readonly IExtendedPublicationResolver _publicationResolver;
        private readonly IDD4TConfiguration _configuration;
        private const string URL = "system/redirects"; //Todo: should this be configurable?

        private string _contextPath;
        private string _defaultFileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRedirectionDefinitionProvider"/> class.
        /// TODO: use caching mechanism here?
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        /// <param name="configuration"></param>
        /// <param name="logger">The logger.</param>
        public DefaultRedirectionDefinitionProvider(IContentProvider contentProvider,
                                                    IExtendedPublicationResolver publicationResolver,
                                                    IDD4TConfiguration configuration,
                                                    ILogger logger)
        {
            contentProvider.ThrowIfNull(nameof(contentProvider));
            logger.ThrowIfNull(nameof(logger));
            configuration.ThrowIfNull(nameof(configuration));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));

            _contentProvider = contentProvider;
            _configuration = configuration;
            _publicationResolver = publicationResolver;
            _logger = logger;
        }

        /// <summary>
        /// Gets the definitions from Tridion and analyses the from string
        /// to determine the type of redirect definition to use.
        /// </summary>
        /// <param name="cleanUrls"><c>true</c> if the URLs should be cleaned; otherwise, <c>false</c></param>
        /// <returns>List&lt;IRedirectionDefinition&gt;.</returns>
        public List<IRedirectionDefinition> GetDefinitions(bool cleanUrls)
        {
            var definitions = new List<IRedirectionDefinition>();
            var page = _contentProvider.GetPage(this.GetRedirectsPath);
            if (page == null)
            {
                _logger.Error("Redirects page is not published. Url : {0}".FormatString(this.GetRedirectsPath));
                return definitions;
            }

            foreach (var cp in page.ComponentPresentations)
            {
                var model = _contentProvider.BuildViewModel(cp) as Redirects;
                if (model == null)
                    continue;

                foreach (var rd in model.Redirect)
                {
                    var permanent = rd.Type.StartsWith("Permanent");
                    var from = (cleanUrls) ? cleanUrl(rd.From) : rd.From;
                    var to = (cleanUrls) ? cleanUrl(rd.To) : rd.To;

                    // compare using uncleaned from URL here to maintain correct regexp starting with ^ and not with /^ that is prefixed 
                    // after cleanUrl procssing
                    if (rd.From.StartsWith("^") && rd.From.EndsWith("$"))
                    {
                        // keep from the same
                        // clean to URL
                        definitions.Add(new RegExRedirectionDefinition(rd.From, to, permanent));
                    }
                    else
                    {
                        // use cleaned URLs in other cases
                        if (from.Contains('?') || from.Contains('*'))
                        {
                            definitions.Add(new WildCardRedirectionDefinition(from, to, permanent));
                        }
                        else
                        {
                            definitions.Add(new ExactMatchRedirectionDefinition(from, to, permanent));
                        }
                    }
                }
            }

            return definitions;
        }

        private string cleanUrl(string url)
        {
            if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                return url;

            return url.Trim().ParseUrl(ContextPath, false, DefaultFileName).CleanUrl(DefaultFileName);
        }

        private string GetRedirectsPath
        {
            get
            {
                string path = DyndleConfig.RedirectsUrl;
                return path.IsNullOrEmpty() ? URL : path;
            }
        }

        private string ContextPath
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_contextPath))
                {
                    _contextPath = _publicationResolver.GetBaseUri().AbsolutePath;
                }

                return _contextPath;
            }
        }
        private string DefaultFileName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_defaultFileName))
                {
                    _defaultFileName = _configuration.WelcomeFile;
                }

                return _defaultFileName;
            }
        }
    }
}