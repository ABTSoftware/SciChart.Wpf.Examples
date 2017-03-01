// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SimpleDataPointEditModifier.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Wpf.TestSuite.ExampleSandbox.CustomModifiers
{
    /// <summary>
    /// Custom Modifier demonstrating selection of series and editing of data-points in the Y-Direction
    /// </summary>
    public class SimpleDataPointEditModifier : SeriesSelectionModifier
    {
        private struct PointEditInfo
        {
            public HitTestInfo HitTestInfo;
            public int IndexOfDataSeries;
            public IDataSeries DataSeries;
        }

        private IRenderableSeries _selectedSeries;
        private Point _lastMousePoint;
        private PointEditInfo? _pointBeingEdited;
        private Ellipse _markerEllipse;
        private readonly double markerSize = 15;

        public SimpleDataPointEditModifier()
        {
            _markerEllipse = new Ellipse()
            {
                Width = markerSize,
                Height = markerSize,
                Fill = new SolidColorBrush(Colors.White),
                Opacity = 0.7,                
            };
            Panel.SetZIndex(_markerEllipse, 9999);
        }

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            // Deliberately call OnModifierMouseUp in MouseDown handler to perform selection on mouse-down
            base.OnModifierMouseUp(e);

            // Get the selected series
            var selectedSeries = this.ParentSurface.RenderableSeries.FirstOrDefault(x => x.IsSelected);

            if (selectedSeries == null)
            {
                e.Handled = false;
                return;
            }

            // Perform a hit-test. Was the mouse over a data-point?
            var pointHitTestInfo = selectedSeries.HitTest(e.MousePoint, 10.0, false);
            if (pointHitTestInfo.IsHit)
            {
                // Store info about point selected
                int dataIndex = pointHitTestInfo.DataSeriesIndex; // Gets the index to the DataSeries. Same as selectedSeries.DataSeries.FindIndex(pointHitTestInfo.XValue);
                _pointBeingEdited = new PointEditInfo() { HitTestInfo = pointHitTestInfo, IndexOfDataSeries = dataIndex, DataSeries = selectedSeries.DataSeries };

                Canvas.SetLeft(_markerEllipse, pointHitTestInfo.HitTestPoint.X - markerSize * 0.5);
                Canvas.SetTop(_markerEllipse, pointHitTestInfo.HitTestPoint.Y - markerSize * 0.5);
                ModifierSurface.Children.Add(_markerEllipse);                
            }

            _selectedSeries = selectedSeries;
            _lastMousePoint = e.MousePoint;
        }

        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            base.OnModifierMouseMove(e);

            // Are we editing a data-point? 
            if (_pointBeingEdited.HasValue == false)
            {
                e.Handled = false;
                return;
            };

            var dataSeries = _pointBeingEdited.Value.DataSeries;

            // Compute how far we want to drag the point
            var currentMousePoint = e.MousePoint;
            double currentYValue = _selectedSeries.YAxis.GetCurrentCoordinateCalculator().GetDataValue(currentMousePoint.Y);
            double startYValue = _selectedSeries.YAxis.GetCurrentCoordinateCalculator().GetDataValue(_lastMousePoint.Y);

            var deltaY = currentYValue - startYValue;

            Canvas.SetLeft(_markerEllipse, _pointBeingEdited.Value.HitTestInfo.HitTestPoint.X - markerSize * 0.5);
            Canvas.SetTop(_markerEllipse, currentMousePoint.Y - markerSize * 0.5);

            // Offset the point
            OffsetPointAt(dataSeries, _pointBeingEdited.Value.IndexOfDataSeries, deltaY);

            _lastMousePoint = currentMousePoint;
        }

        public void OffsetPointAt(IDataSeries series, int index, double offset)
        {
            double yValue = ((double)series.YValues[index]);
            yValue += offset;
            series.YValues[index] = yValue;

            series.InvalidateParentSurface(RangeMode.None);
        }

        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            _selectedSeries = null;
            _pointBeingEdited = null;
            ModifierSurface.Children.Remove(_markerEllipse);
            base.DeselectAll();
        }
    }
}
