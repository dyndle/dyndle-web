using System;
using DD4T.ContentModel;

namespace Dyndle.Modules.Core.Extensions
{
    /// <summary>
    /// Cache extension methods.
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Skips the key while previewing.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SkipKeyWhilePreviewing(this string key)
        {
            return key.StartsWith("model_", StringComparison.InvariantCultureIgnoreCase) ||
                key.StartsWith("output/", StringComparison.InvariantCultureIgnoreCase) ||
                key.StartsWith("page_", StringComparison.InvariantCultureIgnoreCase) ||
                key.StartsWith("componentpresentation_", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Return the Tridion ID (TCM Uri) of this model. Models can be IPage or IComponentPresentation.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ID of the page or ID of the component in the component presentation</returns>
        public static string GetId(this IModel model)
        {
            if (model is IPage page)
            {
                return page.Id;
            }
            if (model is IComponentPresentation componentPresentation)
            {
                return componentPresentation.Component.Id;
            }
            return string.Empty;
        }
    }
}
