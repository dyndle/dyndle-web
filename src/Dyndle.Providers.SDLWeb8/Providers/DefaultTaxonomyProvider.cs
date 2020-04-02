using System.Collections.Generic;
using System.Linq;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Providers.Content;
using Tridion.ContentDelivery.Taxonomies;
using TridionKeyword = Tridion.ContentDelivery.Taxonomies.Keyword;
using DD4TKeyword = DD4T.ContentModel.Keyword;


namespace Dyndle.Providers.SDLWeb8.Providers
{
    public class DefaultTaxonomyProvider : ITaxonomyProvider
    {
        private static readonly TaxonomyFactory TaxonomyFactory = new TaxonomyFactory();

        public IEnumerable<DD4T.ContentModel.Keyword> GetKeywords(string categoryId)
        {
            var taxonomy = TaxonomyFactory.GetTaxonomyKeywords(categoryId);

            if (taxonomy.IsNull())
            {
                return Enumerable.Empty<DD4TKeyword>();
            }
            else
            {
                return taxonomy.KeywordChildren.OfType<TridionKeyword>().Select(k => ConvertToDD4T(k));
            }

        }
        private static DD4TKeyword ConvertToDD4T(TridionKeyword tridionKeyword)
        {
            return new DD4TKeyword()
            {
                Id = tridionKeyword.KeywordUri,
                Title = tridionKeyword.KeywordName,
                Description = tridionKeyword.KeywordDescription,
                Key = tridionKeyword.KeywordKey,
                TaxonomyId = tridionKeyword.TaxonomyUri
            };
        }
    }
}
