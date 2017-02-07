using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Wpf.TestSuite.ExampleSandbox.CustomModifiers
{
    public class SimpleFreeDrawModifier : ChartModifierBase
    {
        private XyDataSeries<DateTime, double> _dataSeries;

        public override void OnModifierDoubleClick(ModifierMouseArgs e)
        {
            base.OnModifierDoubleClick(e);

            // First double-click, start drawing
            if (_dataSeries == null)
            {
                _dataSeries = new XyDataSeries<DateTime, double>() { AcceptsUnsortedData = true };
                var renderableSeries = new FastLineRenderableSeries()
                {
                    StrokeThickness = 2,
                    Stroke = Colors.LimeGreen,
                    PointMarker = new EllipsePointMarker() {Fill = Colors.White,}
                };
                renderableSeries.DataSeries = _dataSeries;
                ParentSurface.RenderableSeries.Add(renderableSeries);

                // Start point
                AppendPoint(e.MousePoint);

                // (Dynamic) end point 
                AppendPoint(e.MousePoint);
            }
            // Last double-click, complete drawing
            else
            {
                _dataSeries = null;
            }
        }

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);

            // Mouse down - a new point 
            AppendPoint(e.MousePoint);
        }

        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            base.OnModifierMouseMove(e);

            UpdatePoint(e.MousePoint);
        }

        private void UpdatePoint(Point mousePoint)
        {
            // On Mouse-move, update the latest point
            if (_dataSeries != null)
            {
                _dataSeries.XValues[_dataSeries.Count - 1] = (DateTime)XAxis.GetDataValue(mousePoint.X);
                _dataSeries.YValues[_dataSeries.Count - 1] = (double)YAxis.GetDataValue(mousePoint.Y);
                _dataSeries.InvalidateParentSurface(RangeMode.None);
            }
        }

        private void AppendPoint(Point mousePoint)
        {
            // On Mouse-down, append another point
            if (_dataSeries != null)
            {
                _dataSeries.Append(
                    (DateTime)XAxis.GetDataValue(mousePoint.X),
                    (double)YAxis.GetDataValue(mousePoint.Y));
            }
        }
    }
}