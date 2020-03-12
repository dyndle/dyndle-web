namespace Trivident.Modules.Core.Models
{
    /// <summary>
    /// Abstract representation of a content filter (for example a Target Group on a Component Presentation)
    /// </summary>
    public interface IFilter
    {
        string Id { get; set; }
        string Title { get; set; }
    }
}