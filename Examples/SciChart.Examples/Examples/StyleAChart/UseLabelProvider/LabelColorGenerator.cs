using System.Windows.Media;

namespace SciChart.Examples.Examples.StyleAChart.UseLabelProvider
{
    public class LabelColorGenerator
    {
        private readonly Color _colorFrom;
        public readonly Color _colorTo;

        public SolidColorBrush DefaultColor { get; } = Brushes.Gray;

        public LabelColorGenerator(Color colorFrom, Color colorTo)
        {
            _colorFrom = colorFrom;
            _colorTo = colorTo;
        }

        public SolidColorBrush GetColor(double labelValue)
        { 
            return Interpolate(_colorFrom, _colorTo, (float)labelValue / 10);
        }

        private SolidColorBrush Interpolate(Color colorFrom, Color colorTo, float ratio)
        {
            return new SolidColorBrush(FromUInt(Interpolate(ToUInt(colorFrom), ToUInt(colorTo), ratio)));
        }

        private uint Interpolate(uint colorFrom, uint colorTo, float ratio)
        {
            const uint mask1 = 0x00FF00FF;
            const uint mask2 = 0xFF00FF00;

            int f2 = (int)(256 * ratio);
            int f1 = 256 - f2;

            return (uint)((((((colorFrom & mask1) * f1) + ((colorTo & mask1) * f2)) >> 8) & mask1) | 
                          (((((colorFrom & mask2) * f1) + ((colorTo & mask2) * f2)) >> 8) & mask2));
        }

        private uint ToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B));
        }

        private Color FromUInt(uint color)
        {
            return Color.FromArgb((byte)(color >> 24), (byte)(color >> 16), (byte)(color >> 8), (byte)color);
        }
    }
}