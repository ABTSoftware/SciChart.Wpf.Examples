using System.Windows.Media;

namespace LabelIndividualStylingColoring
{
    /// <summary>
    /// This class generates a colour from DataValue for demonstration purposes only
    /// </summary>
    public class ColorGenerator
    {
        private readonly Color _colorFrom;
        private readonly Color _colorTo;

        private readonly double _valueFrom;
        private readonly double _valueTo;

        public static Color DefaultColor { get; } = Colors.Gray;

        public ColorGenerator(Color colorFrom, Color colorTo, double valueFrom, double valueTo)
        {
            _colorFrom = colorFrom;
            _colorTo = colorTo;

            _valueFrom = valueFrom;
            _valueTo = valueTo;
        }

        public Color GetColor(double labelValue)
        {
            if (labelValue >= _valueFrom && labelValue <= _valueTo)
            {
                return Interpolate(_colorFrom, _colorTo, (float) labelValue / 10);
            }
            return DefaultColor;
        }

        public static Color Interpolate(Color colorFrom, Color colorTo, float ratio)
        {
            return FromUInt(Interpolate(ToUInt(colorFrom), ToUInt(colorTo), ratio));
        }

        /// <summary>
        /// Linearly interpolates between two colors based on the ratio passed in.
        /// E.g. Ratio = 0.0f returns From color, ratio = 1.0f returns To Color. Ratio = 0.5f returns a mix of the two
        /// </summary>
        /// <param name="colorFrom">The From color</param>
        /// <param name="colorTo">The Two color</param>
        /// <param name="ratio">The ratio of the two colors to mix</param>
        /// <returns>A new color formed by (ratio * from) + ((1.0-ratio) * to)</returns>
        public static uint Interpolate(uint colorFrom, uint colorTo, float ratio)
        {
            const uint mask1 = 0x00FF00FF;
            const uint mask2 = 0xFF00FF00;

            int f2 = (int) (256 * ratio);
            int f1 = 256 - f2;

            return (uint) ((((((colorFrom & mask1) * f1) + ((colorTo & mask1) * f2)) >> 8) & mask1) |
                           (((((colorFrom & mask2) * f1) + ((colorTo & mask2) * f2)) >> 8) & mask2));
        }

        public static uint ToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B));
        }

        public static Color FromUInt(uint color)
        {
            return Color.FromArgb((byte)(color >> 24), (byte)(color >> 16), (byte)(color >> 8), (byte)color);
        }
    }
}