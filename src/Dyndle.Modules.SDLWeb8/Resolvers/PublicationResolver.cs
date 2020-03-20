﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;
using Tridion.ContentDelivery.DynamicContent;

namespace Dyndle.Modules.SDLWeb8.Resolvers
{
    /// <summary>
    /// Resolve publication id by querying Tridion DiscoveryService;
    /// </summary>
    /// <seealso cref="IExtendedPublicationResolver" />
    public class PublicationResolver : IExtendedPublicationResolver
    {
        private readonly DynamicMappingsRetriever _mappingsRetriever;
        private readonly ILogger _logger;

        /// <summary>
        /// holds a list of Resolved IPublicationMappings
        /// </summary>
        private ConcurrentDictionary<string, IPublicationMapping> _publicationMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationResolver" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PublicationResolver(ILogger logger)
        {
            logger.ThrowIfNull(nameof(logger));

            _logger = logger;
            _publicationMappings = new ConcurrentDictionary<string, IPublicationMapping>();
            _mappingsRetriever = new DynamicMappingsRetriever();
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
            //TODO: Fix issue with empty mappings on SDL Web 8.1
            var mapping = FindMapping();
            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = !mapping.Protocol.IsNullOrEmpty() ? mapping.Protocol : HttpContext.Current.Request.Url.Scheme,
                Host = !mapping.Domain.IsNullOrEmpty() ? mapping.Domain : HttpContext.Current.Request.Url.Host,
                Path = !mapping.Path.IsNullOrEmpty() ? mapping.Path : DyndleConfig.PublicationBasePath,
                Port = !mapping.Port.IsNullOrEmpty() ? int.Parse(mapping.Port) : HttpContext.Current.Request.Url.Port
            };
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Finds the mapping.
        /// </summary>
        /// <returns>IPublicationMapping.</returns>
        private IPublicationMapping FindMapping()
        {
            // limit to the schema + hostname + port

            //            var url = HttpContext.Current.Request.Url.OriginalString.ToLower();

            Uri uriToResolve = GetUriToResolve(HttpContext.Current.Request.Url);
            IPublicationMapping result;
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
        /// <exception cref="Exception">
        /// No matching Localization found for URL
        /// </exception>
        private IPublicationMapping Resolve(Uri uri)
        {
            var url = uri.ToString();
            try
            {
                // NOTE: we're not using UrlToLocalizationMapping here, because we may match too eagerly on a base URL when there is a matching mapping with a more specific URL.
                var mapping = _mappingsRetriever.GetPublicationMapping(url);

                if (mapping == null || (!mapping.Port.IsNullOrEmpty() && mapping.Port != uri.Port.ToString()))
                {
                    throw new Exception(string.Format("No matching Localization found for URL '{0}'", url));
                }
                return mapping;
            }
            catch (Exception ex)
            {
                // CIL throws Sdl.Web.Delivery.Service.InvalidResourceException if the mapping cannot be resolved.
                // We don't have a direct reference to Sdl.Web.Delivery.Service, so we just check the type name
                _logger.Debug("Exception occurred in DynamicMappingsRetriever.GetPublicationMapping('{0}'):\n{1}", url, ex.ToString());
                throw new Exception(string.Format("No matching Localization found for URL '{0}'", url), ex);
            }
        }

        private Uri GetUriToResolve(Uri fullUri)
        {
            var url = fullUri.GetLeftPart(UriPartial.Authority);
            var segments = fullUri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (DirectorySegmentsUsedForPublicationMapping < 1)
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
    }
}