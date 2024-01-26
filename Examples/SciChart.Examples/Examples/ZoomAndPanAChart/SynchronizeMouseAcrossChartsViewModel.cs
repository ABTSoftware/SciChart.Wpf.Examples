// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SynchronizeMouseAcrossChartsViewModel.cs is part of the SCICHART® Examples. Permission
// is hereby granted to modify, create derivative works, distribute and publish any part
// of this source code whether for commercial, private or personal use. 
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
        private bool _panEnabled;
        private bool _rolloverEnabled;
        private bool _cursorEnabled;
        private bool _zoomEnabled;
        private IRange _sharedXVisibleRange;

        public SynchronizeMouseAcrossChartsViewModel()
        {
            // Create two DataSeries' which we bind to in the view
            ChartData0 = CreateDataSeries();
            ChartData1 = CreateDataSeries();

            // Set default ChartModifier state
            PanEnabled = true;
            CursorEnabled = true;

            // Set shared initial XAxis.VisibleRange. 
            // Note in code-behind we call sciChartSurface.AnimateZoomExtents(), which requires a
            // valid initial range, hence this binding
            SharedXVisibleRange = new DoubleRange(0d, 1d);
        }

        public IDataSeries ChartData0 { get; private set; }
        public IDataSeries ChartData1 { get; private set; }

        public IRange SharedXVisibleRange
        {
            get => _sharedXVisibleRange;
            set
            {
                if (_sharedXVisibleRange != value)
                {
                    _sharedXVisibleRange = value;
                    OnPropertyChanged("SharedXVisibleRange");
                }
            }
        }

        public bool PanEnabled
        {
            get => _panEnabled;
            set
            {
                if (_panEnabled != value)
                {
                    _panEnabled = value;
                    OnPropertyChanged("PanEnabled");
                }
            }
        }

        public bool ZoomEnabled
        {
            get => _zoomEnabled;
            set
            {
                if (_zoomEnabled != value)
                {
                    _zoomEnabled = value;
                    OnPropertyChanged("ZoomEnabled");
                }
            }
        }

        public bool CursorEnabled
        {
            get => _cursorEnabled;
            set
            {
                if (_cursorEnabled != value)
                {
                    _cursorEnabled = value;
                    OnPropertyChanged("CursorEnabled");
                }
            }
        }

        public bool RolloverEnabled
        {
            get => _rolloverEnabled;
            set
            {
                if (_rolloverEnabled != value)
                {
                    _rolloverEnabled = value;
                    OnPropertyChanged("RolloverEnabled");
                }
            }
        }

        private IDataSeries CreateDataSeries()
        {
            const int count = 1000;

            var ds = new UniformXyDataSeries<double>();

            for (int i = 0; i < count; i++)
            {
                ds.Append(count * Math.Sin(Math.Round(i * Math.PI, 3) * 0.1) / i);
            }

            return ds;
        }
    }
}
