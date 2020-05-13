using System.Collections.Generic;

namespace Dyndle.Modules.Search.Contracts
{
    public interface ISearchResultItem
    {
        List<string> Title { get; set; }

        List<string> Body { get; set; }

        string Id { get; set; }

        List<long> Type { get; set; }

        List<string> Urls { get; set; }

        List<string> ImageUrl { get; set; }

        List<string> Summary { get; set; }

        List<string> ItemTypes { get; set; }

        string Url { get; set; }

        string HrefTarget { get; set; }

        int ItemType { get; }
    }
}