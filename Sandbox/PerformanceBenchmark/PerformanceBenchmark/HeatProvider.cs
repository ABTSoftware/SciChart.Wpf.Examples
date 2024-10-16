using System.Windows.Media;

namespace PerformanceBenchmark
{
    public class HeatProvider
    {
        private readonly double _minimum;
        private readonly double _maximum;

        public LinearGradientBrush ColorMap { get; set; }

        public HeatProvider(double minimum, double maximum, LinearGradientBrush colorMap = null)
        {
            _minimum = minimum;
            _maximum = maximum;

            var defaultColorMap = new LinearGradientBrush(new GradientStopCollection
            {
                new GradientStop { Color = Colors.Red, Offset = 0},
                new GradientStop { Color = Colors.Blue, Offset = 1}

            }, 0);

            ColorMap = colorMap ?? defaultColorMap;
        }

        public Brush GetHeat(double fps)
        {
            Color color = DoubleToArgbColor(fps, GetMappingSettings());
            return new SolidColorBrush(color);
        }

        private struct DoubleToColorMappingSettings
        {
            public GradientStopCollection GradientStops;
            public double Minimum, ScaleFactor;
        }

        private DoubleToColorMappingSettings GetMappingSettings()
        {
            return new DoubleToColorMappingSettings()
            {
                GradientStops = ColorMap.GradientStops,
                Minimum = _minimum,
                ScaleFactor = 1 / (_maximum - _minimum)
            };
        }

        private static Color DoubleToArgbColor(double x, DoubleToColorMappingSettings mappingSettings)
        {
            x -= mappingSettings.Minimum;
            x *= mappingSettings.ScaleFactor;

            for (int i = 1; i < mappingSettings.GradientStops.Count; i++)
            {
                if (x < mappingSettings.GradientStops[i].Offset)
                {
                    var offset1 = mappingSettings.GradientStops[i - 1].Offset;
                    var color1 = mappingSettings.GradientStops[i - 1].Color;

                    var offset2 = mappingSettings.GradientStops[i].Offset;
                    var color2 = mappingSettings.GradientStops[i].Color;

                    return DoubleToArgbColor((x - offset1) / (offset2 - offset1),
                                             color1.R, color1.G, color1.B,
                                             color2.R, color2.G, color2.B);
                }
            }

            return mappingSettings.GradientStops[mappingSettings.GradientStops.Count - 1].Color;
        }

        private static Color DoubleToArgbColor(double x, int r1, int g1, int b1, int r2, int g2, int b2)
        {
            if (x > 1) x = 1;
            if (x < 0) x = 0;

            int r = r1 + (int)((r2 - r1) * x);
            int g = g1 + (int)((g2 - g1) * x);
            int b = b1 + (int)((b2 - b1) * x);

            return Color.FromArgb(255, (byte)r, (byte)g, (byte)b);
        }
    }
}