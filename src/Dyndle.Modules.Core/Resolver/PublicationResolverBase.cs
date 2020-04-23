using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Exceptions;
using Dyndle.Modules.Core.Extensions;

namespace Dyndle.Modules.Core.Resolvers
{
    /// <summary>
    /// Resolve publication id by querying Tridion DiscoveryService;
    /// </summary>
    /// <seealso cref="IExtendedPublicationResolver" />
    public abstract class PublicationResolverBase : IExtendedPublicationResolver
    {
        private readonly ILogger _logger;

        /// <summary>
        /// holds a list of Resolved IPublicationMappings
        /// </summary>
        private readonly ConcurrentDictionary<string, PublicationMapping> _publicationMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationResolverBase" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected PublicationResolverBase(ILogger logger)
        {
            logger.ThrowIfNull(nameof(logger));

            _logger = logger;
            _publicationMappings = new ConcurrentDictionary<string, PublicationMapping>();
        }

        /// <summary>
        /// Resolves the publication identifier.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int ResolvePublicationId()
        {
            return this.FindMapping().PublicationId;
        }

        /// <summary>
        /// Gets the base URI of the current publication.
        /// </summary>
        /// <returns>Uri.</returns>
        public Uri GetBaseUri()
        {
            var _mapping = this.FindMapping();
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = _mapping.Protocol;
            uriBuilder.Host = _mapping.Domain;
            uriBuilder.Path = _mapping.Path;
            uriBuilder.Port = int.Parse(_mapping.Port);
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Finds the mapping.
        /// </summary>
        /// <returns>IPublicationMapping.</returns>
        private PublicationMapping FindMapping()
        {
            // limit to the schema + hostname + port

            //            var url = HttpContext.Current.Request.Url.OriginalString.ToLower();

            Uri uriToResolve = GetUriToResolve(HttpContext.Current.Request.Url);
            PublicationMapping result;
            if (!_publicationMappings.TryGetValue(uriToResolve.ToString(), out result))
            {
                result = Resolve(uriToResolve);
                if (!result.IsNull())
                {
                    _publicationMappings.TryAdd(uriToResolve.ToString(), result);
                }
            }
            return result;
        }



        /// <summary>
        /// Resolves the publication using specified URI from Tridion.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>IPublicationMapping.</returns>
        /// <exception cref="LocalizationNotFoundException">
        /// No matching Localization found for URL
        /// </exception>
        private PublicationMapping Resolve(Uri uri)
        {
            var url = uri.ToString();
            try
            {
                // NOTE: we're not using UrlToLocalizationMapping here, because we may match too eagerly on a base URL when there is a matching mapping with a more specific URL.
                var mapping = RetrieveMapping(url);

                if (mapping == null || (!mapping.Port.IsNullOrEmpty() && mapping.Port != uri.Port.ToString()))
                {
                    throw new LocalizationNotFoundException(string.Format("No matching Localization found for URL '{0}'", url));
                }
                return mapping;
            }
            catch (Exception ex)
            {
                // CIL throws Sdl.Web.Delivery.Service.InvalidResourceException if the mapping cannot be resolved.
                // We don't have a direct reference to Sdl.Web.Delivery.Service, so we just check the type name
                _logger.Debug("Exception occurred in DynamicMappingsRetriever.GetPublicationMapping('{0}'):\n{1}", url, ex.ToString());
                throw new LocalizationNotFoundException(string.Format("No matching Localization found for URL '{0}'", url), ex);
            }
        }

        /// <summary>Retrieves the mapping.</summary>
        /// <param name="url">The URL.</param>
        /// <returns>PublicationMapping.</returns>
        protected abstract PublicationMapping RetrieveMapping(string url);

        private Uri GetUriToResolve(Uri fullUri)
        {
            var url = fullUri.GetLeftPart(UriPartial.Authority);
            var segments = RemoveExtensions(fullUri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            if (segments.FirstOrDefault() == BinaryCacheFolder)
            {
                segments = segments.Skip(1).ToArray();
            }
            if (string.IsNullOrEmpty(fullUri.AbsolutePath) || fullUri.AbsolutePath == "/" || DirectorySegmentsUsedForPublicationMapping < 1)
            {
                return new Uri(url);
            }
            return new Uri(MakeTridionSafe(url + "/" + String.Join("/", segments.Take(DirectorySegmentsUsedForPublicationMapping))));
        }

        /// <summary>
        /// This is a work-around for a bug in SDL Web 8 Publication Mapping: URLs with escaped characters don't resolve properly (CRQ-1585).
        /// Therefore we truncate the URL at the first escaped character for now (assuming that the URL is still specific enough to resolve the right Publication).
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string MakeTridionSafe(string url)
        {
            int espaceIndex = url.IndexOf("%");
            if (espaceIndex > 0)
            {
                return url.Substring(0, espaceIndex);
            }
            return url;
        }

        /// <summary>
        /// Removes url segments that contain a dot extension
        /// </summary>
        /// <param name="segments"></param>
        /// <returns></returns>
        private string[] RemoveExtensions(string[] segments)
        {
            if (segments.Last().Contains("."))
                return segments.Take(segments.Count() - 1).ToArray();
            else
                return segments;
        }

        private int _directorySegmentsUsedForPublicationMapping = -1;
        private int DirectorySegmentsUsedForPublicationMapping
        {
            get
            {
                if (_directorySegmentsUsedForPublicationMapping == -1)
                {
                    var nrSegments = CoreConstants.Configuration.DirectorySegmentsUsedForPublicationMapping.GetConfigurationValue();
                    if (string.IsNullOrEmpty(nrSegments))
                    {
                        _directorySegmentsUsedForPublicationMapping = 1;
                    }
                    else
                    {
                        _directorySegmentsUsedForPublicationMapping = Convert.ToInt32(nrSegments);
                    }
                }
                return _directorySegmentsUsedForPublicationMapping;
            }
        }
        private string BinaryCacheFolder => CoreConstants.Configuration.BinaryCacheFolder.GetConfigurationValue("BinaryData").Replace("/", "").Replace("~", "");

        /// <summary>
        /// Class PublicationMapping.
        /// </summary>
        public class PublicationMapping
        {
            /// <summary>
            /// Gets or sets the namespace identifier.
            /// </summary>
            /// <value>The namespace identifier.</value>
            public int NamespaceId { get; set; }
            /// <summary>
            /// Gets or sets the publication identifier.
            /// </summary>
            /// <value>The publication identifier.</value>
            public int PublicationId { get; set; }
            /// <summary>
            /// Gets or sets the protocol.
            /// </summary>
            /// <value>The protocol.</value>
            public string Protocol { get; set; }
            /// <summary>
            /// Gets or sets the domain.
            /// </summary>
            /// <value>The domain.</value>
            public string Domain { get; set; }
            /// <summary>
            /// Gets or sets the port.
            /// </summary>
            /// <value>The port.</value>
            public string Port { get; set; }
            /// <summary>
            /// Gets or sets the path.
            /// </summary>
            /// <value>The path.</value>
            public string Path { get; set; }
            /// <summary>
            /// Gets or sets the path scan depth.
            /// </summary>
            /// <value>The path scan depth.</value>
            public int PathScanDepth { get; set; }
        }
    }
}