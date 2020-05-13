namespace Dyndle.Modules.ImageEnhancement.Models
{
    public enum CropStyle { Greedy, NonGreedy }

    public interface IEnhancementSettings
    {
        int? Width { get; set; }
        int? Height { get; set; }
        int? CropX { get; set; }
        int? CropY { get; set; }
        int? CropXP { get; set; }
        int? CropYP { get; set; }
        CropStyle CropStyle { get; set; }

        bool RequiresCropping { get; }
        bool RequiresPercentageCropping { get; }
        bool RequiresAbsoluteCropping { get; }
        bool RequiresResizing { get; }
    }
}
