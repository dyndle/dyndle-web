namespace Dyndle.Modules.ImageEnhancement.Models
{
    public class EnhancementSettings : IEnhancementSettings
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? CropX { get; set; }
        public int? CropY { get; set; }
        public int? CropXP { get; set; }
        public int? CropYP { get; set; }
        public CropStyle CropStyle { get; set; } = CropStyle.Greedy;


        public bool RequiresCropping => RequiresAbsoluteCropping || RequiresPercentageCropping;

        public bool RequiresResizing => Width != null || Height != null;

        public bool RequiresPercentageCropping => CropXP != null || CropYP != null;

        public bool RequiresAbsoluteCropping => CropX != null || CropY != null;
    }
}
