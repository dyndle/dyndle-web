using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Attributes.ViewModels;
using Trivident.Modules.Core.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Trivident.Modules.Core.Resolver;

namespace Trivident.Modules.Core.Models.Entities
{
    [ContentModel("ExternalContentLibraryStubSchema-cel", true)]
    public class EclMedia : EntityModel, IEclMedia
    {
        private readonly IEnumerable<string> _videoFileExtensions;

        /// <summary>
        /// Lazy initialization for the CDN Url resolver, only create it once with the CDN paths present
        /// </summary>
        private readonly Lazy<EclCdnUrlResolver> _cdnUrlResolver;

        public EclMedia()
        {
            _videoFileExtensions = new List<string> { "mov", "mp4", "avi" };
            _cdnUrlResolver = new Lazy<EclCdnUrlResolver>(() => new EclCdnUrlResolver(CdnPaths?.FirstOrDefault()));
        }

        public string GetUrl<TEnum>(TEnum type) where TEnum : struct, IConvertible
        {
            return GetUrl(type, string.Empty);
        }

        public string GetUrl<TEnum>(TEnum type, string preferredFileExtension) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return _cdnUrlResolver.Value.GetUrlFor(type, preferredFileExtension);
        }

        [TextMetaEcl]
        public string FileExtension { get; set; }

        [TextMetaEcl]
        public string DisplayName { get; set; }

        [NumericMetaEcl]
        public int Width { get; set; }

        [NumericMetaEcl]
        public int Height { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        [NumericMetaEcl(FieldName = "OriginalFileSize")]
        public int FileSize { get; set; }

        //this is a list, due to a "bug" in dd4t's viewmodel mapper.
        [CDNPathsEcl]
        public List<Dictionary<string, string>> CdnPaths { get; set; }

        public bool IsVideo => _videoFileExtensions.Contains(FileExtension, StringComparer.InvariantCultureIgnoreCase);
    }
}