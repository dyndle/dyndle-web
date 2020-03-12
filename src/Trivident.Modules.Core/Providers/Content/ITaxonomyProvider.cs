using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Providers.Content
{
    public interface ITaxonomyProvider
    {
        IEnumerable<DD4T.ContentModel.Keyword> GetKeywords(string categoryId);
    }
}
