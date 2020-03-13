namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// holds common properties of a Entity (Tridion Component or ComponentPresentation)
    /// </summary>
    public interface IEntityModel : IRenderable
    {
        string Url { get; }
    }
}