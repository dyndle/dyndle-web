using System;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Providers;

namespace Tridion.TestProviders
{
    public class TridionTaxonomyProvider : BaseProvider, ITaxonomyProvider, IDisposable
    {
        public IKeyword GetKeyword(string categoryUriToLookIn, string keywordName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
