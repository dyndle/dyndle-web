using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Management.Models
{
    public class CacheListInfo
    {
        public int Total { get; set; }
        public int MaxPageSize { get; set; }
        public int CurrentPageSize { get; set; }
        public int CurrentPageNr { get; set; }
        public int NrOfPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public bool PaginationEnabled { get; set; }
        public IEnumerable<CacheItem> Items { get; set; }
        public string SearchQuery { get; set; }
        public IEnumerable<int> PagesToShow
        {
            get
            {
                // returns a list of pages to show in the pagination
                // there might be hundreds of pages, so listing them all is not an option
                // instead, we will show the first 2 and last 2, plus the 2 around the current page, with ellipsis
                // symbols (...) in between
                // Ellipsis is indicated by a vaue of -1
                List<int> listOfPages = new List<int>();
                if (NrOfPages <= 9)
                {
                    for (int i = 0; i < NrOfPages; i++)
                    {
                        listOfPages.Add(i);
                    }
                }
                else
                {
                    for (int i = 0; i < NrOfPages; i++)
                    {
                        if (i <= 1 || Math.Abs(i - CurrentPageNr) <= 2 || i >= NrOfPages - 2)
                        {
                            listOfPages.Add(i);
                            continue;
                        }
                        if (i == 2 || i == NrOfPages - 3)
                        {
                            listOfPages.Add(-1);
                        }
                    }
                }
                return listOfPages;
            }
        }
    }
}
