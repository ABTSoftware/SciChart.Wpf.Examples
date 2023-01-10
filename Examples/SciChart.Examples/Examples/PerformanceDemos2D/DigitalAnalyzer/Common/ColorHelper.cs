using System.Windows.Media;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public static class ColorHelper
    {
        public static Color[] DimColors { get; }

        static ColorHelper()
        {
            DimColors = new[]
            {
                Color.FromArgb(0xAA, 0x27, 0x4b, 0x92),
                Color.FromArgb(0xAA, 0x47, 0xbd, 0xe6),
                Color.FromArgb(0xAA, 0xa3, 0x41, 0x8d),
                Color.FromArgb(0xAA, 0xe9, 0x70, 0x64),
                Color.FromArgb(0xAA, 0x68, 0xbc, 0xae),
                Color.FromArgb(0xAA, 0x63, 0x4e, 0x96),
            };
        }
        
        public static Color GetDimColor(int index, double opacity = 1)
        {
            var offset = index / DimColors.Length;

            var color = DimColors[index - offset * DimColors.Length];

            return SetColorTransparency(color, (byte)((opacity * 255) % 255));
        }

        public static Color SetColorTransparency(Color color, byte A)
        {
            return Color.FromArgb(A, color.R, color.G, color.B);
        }
    }
}
