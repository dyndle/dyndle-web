namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Abstract representation of a content filter (for example a Target Group on a Component Presentation)
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }
    }
}