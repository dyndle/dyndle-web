namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Implementation of IFilter to represent Target Group Condition on a Component Presentation
    /// </summary>
    public class SegmentFilter : IFilter
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsExclusion { get; set; }
    }
}
