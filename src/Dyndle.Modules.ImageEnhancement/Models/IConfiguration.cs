namespace Dyndle.Modules.ImageEnhancement.Models
{
    public interface IConfiguration
    {
        string LocalPath { get; }
        string BackgroundColor { get; }
        int CacheSeconds { get; }
    }
}
