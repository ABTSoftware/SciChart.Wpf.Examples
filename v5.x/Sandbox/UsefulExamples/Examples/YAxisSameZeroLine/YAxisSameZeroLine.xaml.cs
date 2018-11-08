using System;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Services;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples
{
    // This test case is not functional / finished yet 

    //[TestCase("Two Charts, YAxis with same zero line")]
    public partial class YAxisSameZeroLine : Window
    {
        public YAxisSameZeroLine()
        {
            InitializeComponent();

            scs0.RenderableSeries.Add(new FastLineRenderableSeries() { DataSeries = GetData(5, 0.01)});
            scs1.RenderableSeries.Add(new FastLineRenderableSeries() { DataSeries = GetData(2, 0.2) });
        }

        private IDataSeries GetData(double amplitude, double damping)
        {
            var xyDataSeries = new XyDataSeries<double>();
            for (int i = 0; i < 1000; i++)
            {
                xyDataSeries.Append(i, Math.Sin(i*0.1)*amplitude);
                amplitude *= (1.0 - damping);
            }

            return xyDataSeries;
        }
    }

    public class CustomViewportManager : DefaultViewportManager
    {
        protected override IRange OnCalculateNewYRange(IAxis yAxis, RenderPassInfo renderPassInfo)
        {
            var range = base.OnCalculateNewYRange(yAxis, renderPassInfo).AsDoubleRange();
            return range;
        }
    }
}
