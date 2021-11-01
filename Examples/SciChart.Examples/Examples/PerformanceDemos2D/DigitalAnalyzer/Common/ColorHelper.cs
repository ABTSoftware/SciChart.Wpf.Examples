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
                Colors.DarkGoldenrod,
                Colors.DarkCyan,
                Colors.DarkGray,
                Colors.DarkKhaki,
                Colors.DarkGreen,
                Colors.DarkMagenta,
                Colors.DarkOliveGreen,
                Colors.DarkSalmon
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
