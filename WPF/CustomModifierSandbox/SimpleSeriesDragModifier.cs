// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SimpleSeriesDragModifier.cs is part of SCICHART®, High Performance Scientific Charts
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
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Wpf.TestSuite.ExampleSandbox.CustomModifiers
{
    /// <summary>
    /// Custom modifier which demonstrates how we can select & drag a series in the Y direction
    /// </summary>
    public class SimpleSeriesDragModifier : SeriesSelectionModifier
    {
        private IRenderableSeries _selectedSeries;
        private Point _lastMousePoint;

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {            
            // Deliberately call OnModifierMouseUp in MouseDown handler to perform selection
            base.OnModifierMouseUp(e);

            var selectedSeries = this.ParentSurface.RenderableSeries.FirstOrDefault(x => x.IsSelected);

            if (selectedSeries == null)
            {
                e.Handled = false;
                return;
            }

            _selectedSeries = selectedSeries;
            _lastMousePoint = e.MousePoint;
        }

        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            base.OnModifierMouseMove(e);

            if (_selectedSeries == null)
            {
                e.Handled = false;
                return;
            };

            var dataSeries = _selectedSeries.DataSeries as XyDataSeries<DateTime, double>;
            if (dataSeries == null)
            {
                throw new InvalidOperationException("This modifier is only configured to work with XyDataSeries<double,double>");
            }

            var currentMousePoint = e.MousePoint;
            double currentYValue = _selectedSeries.YAxis.GetCurrentCoordinateCalculator().GetDataValue(currentMousePoint.Y);
            double startYValue = _selectedSeries.YAxis.GetCurrentCoordinateCalculator().GetDataValue(_lastMousePoint.Y);

            var deltaY = currentYValue - startYValue;

            OffsetSeries(dataSeries, deltaY);

            _lastMousePoint = currentMousePoint;
        }

        public void OffsetSeries(XyDataSeries<DateTime, double> series, double offset)
        {
            var originalXData = series.XValues.ToArray();
            var originalYData = series.YValues.ToArray();

            // To Offset the series, clear and recreate data
            using (series.SuspendUpdates())
            {
                series.Clear();
                series.Append(originalXData, originalYData.Select(y => y + offset));
            }
        }

        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            _selectedSeries = null;
            base.DeselectAll();
        }
    }
}
