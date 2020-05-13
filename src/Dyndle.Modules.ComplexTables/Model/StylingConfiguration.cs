namespace Dyndle.Modules.ComplexTables.Model
{
    public class StylingConfiguration
    {
        public bool TextAlign { get; set; }
        public bool Fill { get; set; }
        public bool Borders { get; set; }
        public bool Width { get; set; }
        public bool FontFamily { get; set; }
        public bool FontSize { get; set; }
        public bool FontWeight { get; set; }
        public bool FontColor { get; set; }

        public static StylingConfiguration Default
        {
            get
            {
                return new StylingConfiguration()
                {
                    TextAlign = true,
                    Fill = true,
                    Width = true,
                    FontFamily = true,
                    FontSize = true,
                    FontWeight = true,
                    FontColor = true
                };
            }
        }
    }
}
