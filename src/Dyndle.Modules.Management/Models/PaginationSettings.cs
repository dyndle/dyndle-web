using System;
using System.Collections.Generic;
using System.Linq;

namespace Dyndle.Modules.Management.Models
{
    public class PaginationSettings
    {
        public PaginationSettings(int total, int pageNr, int pageSize)
        {
            this.Total = total;
            this.CurrentPageNr = pageNr;
            this.MaxPageSize = pageSize;

            HasPrevious = CurrentPageNr > 0;
            HasNext = (CurrentPageNr + 1) * MaxPageSize < Total;
            CurrentPageSize = Math.Min(Total - CurrentPageNr * MaxPageSize, MaxPageSize);
            NrOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Total) / Convert.ToDouble(MaxPageSize)));

            if (pageNr > NrOfPages || pageNr < 0)
            {
                throw new DataMisalignedException("The page number you specified is outside the range of pages for this data set");
            }
        }
        public int Total { get; set; }
        public int MaxPageSize { get; set; }
        public int CurrentPageNr { get; set; }

        public int ShowBeforeCurrent { get; set; } = -1;
        public int ShowAfterCurrent { get; set; } = -1;

        public int AlwaysShowOnLeftSide { get; set; } = 0;
        public int AlwaysShowOnRightSide { get; set; } = 0;
        public bool ShowEllipsisButtons { get; set; } = false;

        public SpecialButtonOption SpecialButtons { get; set; } = PaginationSettings.SpecialButtonOption.Previous | PaginationSettings.SpecialButtonOption.Next;

        public int CurrentPageSize { get; private set; }
        public int NrOfPages { get; private set; }
        public bool HasNext { get; private set; }
        public bool HasPrevious { get; private set; }
        public int HighestPageNumber => NrOfPages - 1;
        public bool PaginationRequired => HasNext || HasPrevious;

        public IEnumerable<T> GetItems<T>(IEnumerable<T> list)
        {
            if (Total > CurrentPageNr * MaxPageSize)
            {
                return list.Skip((CurrentPageNr) * MaxPageSize).Take(CurrentPageSize).Cast<T>();
            }
            else
            {
                return new List<T>();
            }
        }

        public IEnumerable<PageButton> GetPageButtons()
        {
            IEnumerable<int> listOfNumbers;
            if (ShowBeforeCurrent == -1)
            {
                listOfNumbers = GeneratePageNumbers(0, HighestPageNumber);
            }
            else
            {
                var island = GeneratePageNumbers(CurrentPageNr - ShowBeforeCurrent, CurrentPageNr + ShowAfterCurrent);
                var leftCape = GeneratePageNumbers(0, AlwaysShowOnLeftSide - 1);
                var rightCape = GeneratePageNumbers(HighestPageNumber - AlwaysShowOnRightSide + 1, HighestPageNumber);
                listOfNumbers = leftCape.Concat(island).Concat(rightCape).Distinct();
            }

            var list = new List<PageButton>();

            if (SpecialButtons.HasFlag(SpecialButtonOption.First) && PaginationRequired)
            {
                list.Add(new PageButton(PageButtonType.First));
            }
            if (SpecialButtons.HasFlag(SpecialButtonOption.Previous) && HasPrevious)
            {
                list.Add(new PageButton(PageButtonType.Previous));
            }
            bool previousWasEllipse = false;
            for (var i = 0; i <= HighestPageNumber; i++)
            {
                if (listOfNumbers.Contains(i))
                {
                    list.Add(new PageButton(i) { Active = i == CurrentPageNr });
                    previousWasEllipse = false;
                }
                else
                {
                    if (SpecialButtons.HasFlag(SpecialButtonOption.Ellipsis) && i > 0 && listOfNumbers.Contains(i - 1) && !previousWasEllipse)
                    {
                        list.Add(new PageButton(PageButtonType.Ellipsis));
                        previousWasEllipse = true;
                    }
                    else if (SpecialButtons.HasFlag(SpecialButtonOption.Ellipsis) && i < HighestPageNumber && listOfNumbers.Contains(i + 1) && !previousWasEllipse)
                    {
                        list.Add(new PageButton(PageButtonType.Ellipsis));
                        previousWasEllipse = true;
                    }
                }
            }
            if (SpecialButtons.HasFlag(SpecialButtonOption.Next) && HasNext)
            {
                list.Add(new PageButton(PageButtonType.Next));
            }
            if (SpecialButtons.HasFlag(SpecialButtonOption.Last) && PaginationRequired)
            {
                list.Add(new PageButton(PageButtonType.Last));
            }
            return list;
        }

        private IEnumerable<int> GeneratePageNumbers(int v1, int v2)
        {
            List<int> list = new List<int>();
            for (var i = Math.Max(0, v1); i <= Math.Min(v2, HighestPageNumber); i++)
            {
                list.Add(i);
            }
            return list;
        }

        [Flags]
        public enum SpecialButtonOption
        {
            None = 0,
            First = 1,
            Previous = 2,
            Next = 4,
            Last = 8,
            Ellipsis = 16
        }

        public enum PageButtonType { Regular, Previous, Next, First, Last, Ellipsis }
        public class PageButton
        {
            public PageButton(int i)
            {
                PageButtonType = PageButtonType.Regular;
                Number = i;
                VisibleNumber = i + 1;
            }
            public PageButton(PageButtonType pbt)
            {
                PageButtonType = pbt;
            }

            public PageButton()
            {

            }

            public PageButtonType PageButtonType { get; set; }
            public bool Active { get; set; }
            public int Number { get; set; }
            public int VisibleNumber { get; set; }
        }
    }
}
