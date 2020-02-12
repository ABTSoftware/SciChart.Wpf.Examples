using SciChart.Core.Extensions;
using System;
using System.Windows.Media;

namespace LabelIndividualStylingColoring
{
    /// <summary>
    /// This class generates a colour from DataValue for demonstration purposes only
    /// </summary>
    public class ColorGenerator
    {
        private readonly Color _from;
        private readonly Color _to;

        public ColorGenerator(Color from, Color to)
        {
            _from = from;
            _to = to;
        }

        public Color GetColor(IComparable dataValue)
        {
            double dataValueDouble = dataValue.ToDouble();
            // from a scale of 0 to 10 return red to blue
            return Lerp(_from, _to, (float)dataValueDouble / 10);
        }

        public static Color Lerp(Color from, Color to, float ratio)
        {
            return FromUInt(Lerp(ToUInt(from), ToUInt(to), ratio));
        }

        /// <summary>
        /// Linearly interpolates between two colors based on the ratio passed in. E.g. Ratio = 0.0f returns From color, ratio = 1.0f returns To Color. Ratio = 0.5f returns a mix of the two
        /// </summary>
        /// <param name="from">The From color</param>
        /// <param name="to">The Two color</param>
        /// <param name="ratio">The ratio of the two colors to mix</param>
        /// <returns>A new color formed by (ratio * from) + ((1.0-ratio) * to)</returns>
        public static uint Lerp(uint from, uint to, float ratio)
        {
            const uint mask1 = 0x00ff00ff;
            const uint mask2 = 0xff00ff00;

            int f2 = (int)(256 * ratio);
            int f1 = 256 - f2;

            return (uint)((((((@from & mask1) * f1) + ((to & mask1) * f2)) >> 8) & mask1)
                          | (((((@from & mask2) * f1) + ((to & mask2) * f2)) >> 8) & mask2));
        }

        public static uint ToUInt(Color color)
        {
            return FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Color FromUInt(uint color)
        {
            return Color.FromArgb((byte)(color >> 24), (byte)(color >> 16), (byte)(color >> 8), (byte)color);
        }

        public static uint FromArgb(byte alpha, byte red, byte green, byte blue)
        {
            return (uint)((alpha << 24) | (red << 16) | (green << 8) | (blue));
        }
    }
}