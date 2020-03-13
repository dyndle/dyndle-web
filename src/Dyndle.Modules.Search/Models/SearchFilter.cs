namespace Dyndle.Modules.Search.Models
{
    /// <summary>
    /// Generic search filter
    /// </summary>
    public class SearchFilter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public int NumberOfHits { get; set; }
        public bool Selected { get; set; }
    }
}