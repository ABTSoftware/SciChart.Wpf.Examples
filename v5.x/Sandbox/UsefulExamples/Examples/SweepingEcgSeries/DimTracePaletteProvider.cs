using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace Abt.Controls.SciChart.Wpf.TestSuite.ExampleSandbox.SweepingEcg
{
    public class DimTracePaletteProvider : IStrokePaletteProvider
    {

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {            
        }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var defaultColor = rSeries.Stroke;

            if (rSeries.DataSeries == null)
                return defaultColor;

            var xyzSeries = ((XyzDataSeries<double, double, double>)rSeries.DataSeries);

            double actualTime = xyzSeries.ZValues[index];
            double latestTime = (double) xyzSeries.Tag;

            // how old is the sample? 1.0 = New, 0.0 = Oldest
            double sampleAge = (actualTime - latestTime) / 10.0;

            // Clamp in ten steps, e.g.  0.1, 0.2 .... 0.9, 1.0
            // Why? Creating a new Pen for each single sample will slow down SciChart significantly
            sampleAge = Math.Round(sampleAge * 10.0, 0) * 0.1;

            // Ensure in the range 0.3 ... 1.0 always 
            sampleAge = Math.Max(0.3, sampleAge);
            sampleAge = Math.Min(1.0, sampleAge);

            // Compute the Alpha based on sample age
            var modifiedColor = Color.FromArgb((byte)(sampleAge * 0xFF), defaultColor.R, defaultColor.G, defaultColor.B);
            return modifiedColor;
        }
    }
}
