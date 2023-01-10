// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VitalSignsPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby
// granted to modify, create derivative works, distribute and publish any part of this
// source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.VitalSignsMonitor
{
    public class VitalSignsPaletteProvider : IStrokePaletteProvider
    {
        private readonly byte _defaultAlpha;
        private readonly byte _diffAlpha;

        public Values<Color> Colors { get; } = new Values<Color>();

        public VitalSignsPaletteProvider(byte defaultAlpha = 50)
        {
            _defaultAlpha = defaultAlpha;
            _diffAlpha = (byte)(255 - defaultAlpha);
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            var defaultColor = rSeries.Stroke;
            var count = rSeries.DataSeries.Count;

            if (Colors.Count != count)
            {
                Colors.Count = count;

                for (int i = 0; i < count; i++)
                {
                    var color = defaultColor;
                    color.A = (byte)(_defaultAlpha + _diffAlpha * i / count);
                    Colors.Items[i] = color;
                }
            }
        }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            return Colors.Items[index];
        }
    }
}