using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Trivident.Modules.Core.Resolver
{
    /// <summary>
    /// Class knows how to get a certain URL for a given ECL variant type from a list of CDN URLs for an asset
    /// </summary>
    public class EclCdnUrlResolver
    {
        private const string DefaultType = "Original";

        private readonly Dictionary<string, string> _mappingWithFileExtensions;
        private readonly Dictionary<string, string> _mappingWithoutFileExtensions;

        /// <summary>
        /// Constructor to initialize path mapping
        /// </summary>
        /// <param name="urlMapping"></param>
        public EclCdnUrlResolver(Dictionary<string, string> urlMapping)
        {
            _mappingWithFileExtensions = urlMapping ?? new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            _mappingWithoutFileExtensions = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            InitializeMappingWithoutExtensions();
        }

        /// <summary>
        /// Initialize mapping of [filename-without-preferred-extension -> URL]
        /// </summary>
        private void InitializeMappingWithoutExtensions()
        {
            foreach (var item in _mappingWithFileExtensions)
            {
                var fileNameWithExtension = item.Key;
                var lastUnderscore = fileNameWithExtension.LastIndexOf('_');
                var eclVariantType = lastUnderscore > 0 ? fileNameWithExtension.Remove(lastUnderscore) : fileNameWithExtension;

                // only add the mapping if the ECL variant type is not present yet
                if (!_mappingWithoutFileExtensions.ContainsKey(eclVariantType))
                {
                    _mappingWithoutFileExtensions[eclVariantType] = item.Value;
                }
            }
        }

        /// <summary>
        /// Get the CDN URL for a specific ECL variant type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="preferredFileExtension"></param>
        /// <returns></returns>
        public string GetUrlFor<T>(T type, string preferredFileExtension = "") where T : IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Type parameter {nameof(T)} must be an enumerated type");
            }

            var enumType = type as Enum;
            var eclVariantType = GetEnumMemberValue(enumType);
            return TryToGetRequestedVariantUrl(eclVariantType, preferredFileExtension) ?? TryToGetGetDefaultVariantUrl();
        }

        /// <summary>
        /// Return URL for requested variant type if present, or null otherwise
        /// </summary>
        /// <param name="eclVariantType"></param>
        /// <param name="preferredFileExtension"></param>
        /// <returns></returns>
        private string TryToGetRequestedVariantUrl(string eclVariantType, string preferredFileExtension)
        {
            var key = $"{eclVariantType}_{preferredFileExtension}";
            if (_mappingWithFileExtensions.ContainsKey(key))
            {
                return _mappingWithFileExtensions[key];
            }
            return _mappingWithoutFileExtensions.ContainsKey(eclVariantType) ? _mappingWithoutFileExtensions[eclVariantType] : null;
        }

        /// <summary>
        /// Return URL for default variant type if present, or null otherwise
        /// </summary>
        /// <returns></returns>
        private string TryToGetGetDefaultVariantUrl()
        {
            // Fallback to default type 
            var value = string.Empty;
            if (_mappingWithFileExtensions.TryGetValue(DefaultType, out value))
            {
                return value;
            }
            return _mappingWithoutFileExtensions.TryGetValue(DefaultType, out value) ? value : null;
        }

        /// <summary>
        /// Helper method to get strng value of an enum
        /// TODO: move this to some extension method class?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetEnumMemberValue(Enum value)
        {
            var attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .SingleOrDefault() as EnumMemberAttribute;

            return attribute == null ? value.ToString() : attribute.Value;
        }
    }
}