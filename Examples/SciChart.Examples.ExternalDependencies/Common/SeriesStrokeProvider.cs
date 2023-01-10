using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;
using SciChart.Charting3D.RenderableSeries;

namespace SciChart.Examples.ExternalDependencies.Common
{
    /// <summary>
    /// Helper class that generates stroke / fill colours that match the examples app theme when there are
    /// many series to generate
    /// </summary>
    public class SeriesStrokeProvider
    {
        private List<uint> strokeGradient;

        static readonly double Precision = 100;

        public SeriesStrokeProvider()
        {
        }


        public Color[] StrokePalette { get; set; }

        /// <summary>
        /// Returns a stroke colour from the palette based on index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Color GetStroke(int index, int max)
        {
            if (StrokePalette == null || StrokePalette.Length == 0)
            {
                throw new InvalidOperationException("StrokePalette must be defined and length > 0");
            }

            if (index > max)
            {
                throw new InvalidOperationException("index must be less than or equal to max");
            }
            if (max <= StrokePalette.Length)
            {
                return this.StrokePalette[index];
            }

            if (StrokePalette.Length < 2)
            {
                return this.StrokePalette[0];
            }

            if (strokeGradient == null)
            {
                strokeGradient = this.CreateStrokeGradient(this.StrokePalette.Select((color, index) =>
                {
                    return new GradientStop(color, index / (StrokePalette.Length - 1.0));
                }).ToArray());
            }

            double lerpFactor = index / (double)max;
            int mapIndex = (int)Math.Round(lerpFactor * (strokeGradient.Count - 1));
            mapIndex = mapIndex < 0 ? 0 : mapIndex;
            mapIndex = mapIndex >= strokeGradient.Count ? strokeGradient.Count : mapIndex;
            uint result = strokeGradient[mapIndex];
            var color = ColorUtil.FromUInt(result);
            return color;
        }

        /// <summary>
        /// Input is an array of GradientStop[] with colour and offset
        /// Output is a color map - a uint[] size 500 in length with the gradient stops interpolated into one continuous array
        /// Used to pick colours on a gradient for strokes/fills
        /// </summary>
        /// <param name="gradientStops"></param>
        /// <returns></returns>
        private List<uint> CreateStrokeGradient(GradientStop[] gradientStops)
        {
            var colorMap = new List<uint>();
            var count = gradientStops.Length;
            var first = gradientStops[0].Offset;
            var last = gradientStops[gradientStops.Length - 1].Offset;
            double diff = last - first;
            double change = diff / (Precision - 1);

            var prevColor = ColorUtil.ToUInt(gradientStops[0].Color);
            var prevOffset = gradientStops[0].Offset;
            var nextColor = prevColor;
            var nextOffset = prevOffset;

            if (count > 1)
            {
                nextColor = ColorUtil.ToUInt(gradientStops[1].Color);
                nextOffset = gradientStops[1].Offset;
            }
            diff = nextOffset - prevOffset;

            var offsetInd = 0;

            for (var i = 0; i < Precision; ++i)
            {
                var offset = first + i * change;
                if (offset >= nextOffset)
                {
                    offsetInd++;
                    prevOffset = nextOffset;
                    prevColor = nextColor;

                    if (offsetInd + 1 < count)
                    {
                        nextColor = ColorUtil.ToUInt(gradientStops[offsetInd + 1].Color);
                        nextOffset = gradientStops[offsetInd + 1].Offset;
                    }

                    diff = nextOffset - prevOffset;
                }

                uint color = 0;
                if (prevColor == nextColor || diff <= 0.00000000001)
                {
                    color = nextColor;
                }
                else
                {
                    double coef = (offset - prevOffset) / diff;
                    color = LinearInterpolateI(prevColor, nextColor, coef);
                }

                colorMap.Add(color);
            }
            return colorMap;
        }

        /// <summary>
        /// Linearly interpolates two colours in ARGB format depending on the ratio 0..1
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        private uint LinearInterpolateI(uint from, uint to, double ratio)
        {
            int af = (int)(from >> 24);
            int rf = (int)((from >> 16) & 0xFF);
            int gf = (int)((from >> 8) & 0xFF);
            int bf = (int)(from & 0xFF);
            
            int at = (int)(to >> 24);
            int rt = (int)((to >> 16) & 0xFF);
            int gt = (int)((to >> 8) & 0xFF);
            int bt = (int)(to & 0xFF);

            int aResult = (int)(af + ((at - af) * ratio));
            int rResult = (int)(rf + ((rt - rf) * ratio));
            int gResult = (int)(gf + ((gt - gf) * ratio));
            int bResult = (int)(bf + ((bt - bf) * ratio));

            return (uint)((aResult << 24) | (rResult << 16) | (gResult << 8) | bResult);
        }
    }
}
