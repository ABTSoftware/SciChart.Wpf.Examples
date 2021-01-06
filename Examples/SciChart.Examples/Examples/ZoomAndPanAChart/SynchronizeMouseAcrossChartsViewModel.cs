// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SynchronizeMouseAcrossChartsViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.ZoomAndPanAChart
{
    public class SynchronizeMouseAcrossChartsViewModel : BaseViewModel
    {
        private bool _mouseWheelEnabled;
        private bool _panEnabled;
        private bool _rolloverEnabled;
        private bool _cursorEnabled;
        private bool _zoomEnabled;
        private IRange _sharedXVisibleRange;
        private readonly IDataSeries _chartData0;
        private readonly IDataSeries _chartData1;

        public SynchronizeMouseAcrossChartsViewModel()
        {
            // Create two DataSeries' which we bind to in the view
            _chartData0 = CreateDataSeries();
            _chartData1 = CreateDataSeries();

            // Set default ChartModifier state
            this.MouseWheelEnabled = false;
            this.PanEnabled = true;
            this.CursorEnabled = true;

            // Set shared initial XAxis.VisibleRange. 
            // Note in code-behind we call sciChartSurface.AnimateZoomExtents(), which requires a
            // valid initial range, hence this binding
            SharedXVisibleRange = new DoubleRange(0,1);
        }

        public IDataSeries ChartData0 { get { return _chartData0; } }
        public IDataSeries ChartData1 { get { return _chartData1; } }

        public IRange SharedXVisibleRange
        {
            get { return _sharedXVisibleRange; }
            set
            {
                if (_sharedXVisibleRange == value) return;
                _sharedXVisibleRange = value;
                OnPropertyChanged("SharedXVisibleRange");
            }
        }
        
        public bool MouseWheelEnabled
        {
            get { return _mouseWheelEnabled; }
            set
            {
                if (_mouseWheelEnabled == value) return;
                _mouseWheelEnabled = value;
                OnPropertyChanged("MouseWheelEnabled");
            }
        }

        public bool PanEnabled
        {
            get { return _panEnabled; }
            set
            {
                if (_panEnabled == value) return;
                _panEnabled = value;
                OnPropertyChanged("PanEnabled");

                // Toggle Zoom off
                ZoomEnabled = !PanEnabled;
            }
        }

        public bool ZoomEnabled
        {
            get { return _zoomEnabled; }
            set
            {
                if (_zoomEnabled == value) return;
                _zoomEnabled = value;
                OnPropertyChanged("ZoomEnabled");

                // Toggle pan off
                PanEnabled = !ZoomEnabled;
            }
        }

        public bool CursorEnabled
        {
            get { return _cursorEnabled; }
            set
            {
                if (_cursorEnabled == value) return;
                _cursorEnabled = value;
                OnPropertyChanged("CursorEnabled");

                // Toggle RolloverEnabled off
                RolloverEnabled = !CursorEnabled;
            }
        }

        public bool RolloverEnabled
        {
            get { return _rolloverEnabled; }
            set
            {
                if (_rolloverEnabled == value) return;
                _rolloverEnabled = value;
                OnPropertyChanged("RolloverEnabled");

                // Toggle RolloverEnabled off
                CursorEnabled = !RolloverEnabled;
            }
        }

        private IDataSeries CreateDataSeries()
        {
            var ds0 = new XyDataSeries<double, double>();

            const int count = 1000;
            for (int i = 0; i < count; i++)
            {
                var y = count * Math.Sin(Math.Round(i * Math.PI, 3) * 0.1) / i;
                ds0.Append(i, y);
            }

            return ds0;
        }
    }
}
