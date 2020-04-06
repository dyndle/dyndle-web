namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Implementation of IFilter to represent Target Group Condition on a Component Presentation
    /// </summary>
    public class SegmentFilter : IFilter
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is exclusion.
        /// </summary>
        /// <value><c>true</c> if this instance is exclusion; otherwise, <c>false</c>.</value>
        public bool IsExclusion { get; set; }
    }
}
