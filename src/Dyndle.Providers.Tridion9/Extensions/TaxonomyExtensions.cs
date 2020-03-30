using TridionKeyword = Tridion.ContentDelivery.Taxonomies.Keyword;
using DD4TKeyword = DD4T.ContentModel.Keyword;

namespace Dyndle.Providers.Tridion9.Extensions
{
    public static class TaxonomyExtensions
    {
        public static DD4TKeyword ToDD4TKeyword (this TridionKeyword tridionKeyword)
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
