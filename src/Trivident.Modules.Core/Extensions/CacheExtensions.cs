using DD4T.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Extensions
{
    public static class CacheExtensions
    {
        public static bool SkipKeyWhilePreviewing(this string key)
        {
            return key.StartsWith("model_", StringComparison.InvariantCultureIgnoreCase) ||
                key.StartsWith("output_", StringComparison.InvariantCultureIgnoreCase) ||
                key.StartsWith("page_", StringComparison.InvariantCultureIgnoreCase) ||
                key.StartsWith("componentpresentation_", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Return the Tridion ID (TCM Uri) of this model. Models can be IPage or IComponentPresentation.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ID of the page or ID of the component in the component presentation</returns>
        public static string GetId(this IModel model)
        {
            if (model is IPage)
            {
                return ((IPage)model).Id;
            }
            if (model is IComponentPresentation)
            {
                return ((IComponentPresentation)model).Component.Id;
            }
            return string.Empty;
        }
    }
}
