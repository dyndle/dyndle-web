using System.Collections.Generic;

namespace Dyndle.Modules.Core.Providers.Content
{
    public interface ITaxonomyProvider
    {
        IEnumerable<DD4T.ContentModel.Keyword> GetKeywords(string categoryId);
    }
}
