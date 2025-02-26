// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// CategoryVsValueAxisViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes.IndexDataProviders;
using SciChart.Examples.ExternalDependencies.Common;
using System.Linq;
using SciChart.Charting.ViewportManagers;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    public class CategoryVsValueAxisViewModel : BaseViewModel
    {
        private IViewportManager _viewportManager;

        private bool _useIndexNumericAxis;
        private IIndexDataProvider _indexDataProvider;

        private IDataSeries<double, double> _dataSeries;
        private readonly double[] _xData = { -3, 1.8, 2.35, 3.4, 4, 12, 12.3, 13.2, 13.5, 14, 20, 20.1, 20.6, 21.5, 22, 23, 24.2, 24.8, 25.15, 25.65, 26 };
        private readonly double[] _yData = { 1, 4, 3.0, 5.2, 2, 2, 1.3, 7, 5.5, 6.3, 6.3, 5.8, 4.1, 5.5, 3, 3, 4.8, 4.1, 6, 5.1, 5.8 };

        public CategoryVsValueAxisViewModel()
        {
            // ViewportManager allows to reset zoom level
            ViewportManager = new DefaultViewportManager();

            // Create a DataSeries and append data
            DataSeries = new XyDataSeries<double>
            {
                // IndexNumericAxis doesn't support unsorted data
                // Explicitly specify that DataSeries only allows data with X-values sorted ascending
                AcceptsUnsortedData = false
            };
            DataSeries.Append(_xData, _yData);

            // Specify that the Category Axis should generate ticks and
            // calculate coordinates based on this DataSeries
            IndexDataProvider = new DataSeriesIndexDataProvider(DataSeries);
        }

        public IDataSeries<double, double> DataSeries
        {
            get => _dataSeries;
            set
            {
                if (ReferenceEquals(value, _dataSeries)) return;
                _dataSeries = value;
                OnPropertyChanged(nameof(DataSeries));
            }
        }

        public bool UseIndexNumericAxis
        {
            get => _useIndexNumericAxis;
            set
            {
                if (_useIndexNumericAxis == value) return;
                _useIndexNumericAxis = value;
                OnPropertyChanged(nameof(UseIndexNumericAxis));

                // Reset zoom level when Axis Type is changed
                ViewportManager?.ZoomExtents();
            }
        }

        public IIndexDataProvider IndexDataProvider
        {
            get => _indexDataProvider;
            set
            {
                if (ReferenceEquals(value, _indexDataProvider)) return;
                _indexDataProvider = value;
                OnPropertyChanged(nameof(IndexDataProvider));
            }
        }

        public IViewportManager ViewportManager
        {
            get => _viewportManager;
            set
            {
                if (ReferenceEquals(value, _viewportManager)) return;
                _viewportManager = value;
                OnPropertyChanged(nameof(ViewportManager));
            }
        }
    }
}