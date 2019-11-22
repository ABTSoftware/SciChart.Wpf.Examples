using System.Linq;
using System.Windows;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Axes.AxisBandProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Utility.Mouse;

namespace CustomAxisBandsProviderExample
{
    public class AxisBandsInfo
    {
        public IAxisBandInfo BandInfo { get; set; }
        public AxisInfo HitTestInfo { get; set; }
    }

    public class ChartBandsInfo
    {
        public AxisBandsInfo XAxisBandInfo { get; set; }
        public AxisBandsInfo YAxisBandInfo { get; set; }
    }

    public class AxisBandsInfoModifier : ChartModifierBase
    {
        // Defines the AxisBandsInfo DependencyProperty.
        public static readonly DependencyProperty ChartBandsInfoProperty =
            DependencyProperty.Register("ChartBandsInfo", typeof(ChartBandsInfo), typeof(AxisBandsInfoModifier), new PropertyMetadata(null));

        public AxisBandsInfoModifier()
        {
            SetCurrentValue(ChartBandsInfoProperty, new ChartBandsInfo
            {
                XAxisBandInfo = new AxisBandsInfo(),
                YAxisBandInfo = new AxisBandsInfo()
            });
        }

        /// <summary>
        /// Gets or sets a <see cref="ChartBandsInfo"/> instance that contains hit-test result of X and Y axis.
        /// </summary>
        public ChartBandsInfo ChartBandsInfo
        {
            get { return (ChartBandsInfo)GetValue(ChartBandsInfoProperty); }
            set { SetValue(ChartBandsInfoProperty, value); }
        }

        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            base.OnModifierMouseMove(e);

            var pt = GetPointRelativeTo(e.MousePoint, ModifierSurface);

            var xAxis = XAxes.FirstOrDefault();
            var yAxis = YAxes.FirstOrDefault();

            var xAxisBandInfo = GetAxisBandInfo(xAxis, pt, out var xAxisInfo);
            var yAxisBandInfo = GetAxisBandInfo(yAxis, pt, out var yAxisInfo);

            ChartBandsInfo.XAxisBandInfo.BandInfo = xAxisBandInfo;
            ChartBandsInfo.XAxisBandInfo.HitTestInfo = xAxisInfo;

            ChartBandsInfo.YAxisBandInfo.BandInfo = yAxisBandInfo;
            ChartBandsInfo.YAxisBandInfo.HitTestInfo = yAxisInfo;
        }

        private IAxisBandInfo GetAxisBandInfo(IAxis axis, Point hitTestPt, out AxisInfo axisInfo)
        {
            IAxisBandInfo bandInfo = null;
            axisInfo = null;

            if (axis is AxisBase axisBase
                && axisBase.AxisBandsProvider is IAxisBandsProviderBase bandsProvider)
            {
                axisInfo = axis.HitTest(hitTestPt);
                var res = axisInfo.DataValue;

                bandInfo = bandsProvider.AxisBands.FirstOrDefault(bi =>
                    bi.BandRange.Min.CompareTo(res) <= 0 && bi.BandRange.Max.CompareTo(res) >= 0);
            }

            return bandInfo;
        }
    }
}
