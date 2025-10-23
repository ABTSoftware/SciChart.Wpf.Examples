// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace SciChart_SyncMultiChartMvvm
{
    public class SynchronizeMouseAcrossChartsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<IAxisViewModel> _topXAxesCollection;
        private ObservableCollection<IAxisViewModel> _bottomXAxesCollection;
        private IRange _sharedXVisibleRange;

        public SynchronizeMouseAcrossChartsViewModel()
        {
            // Create two X Axes: for the top and bottom chart
            // Add to the corresponding Axes Collections for binding to the charts
            var topXAxis = new NumericAxisViewModel
            {
                AxisTitle = "Top X Axis",
                AxisAlignment = AxisAlignment.Bottom,
                StyleKey = "XAxisStyle",
                Tag = new SolidColorBrush(Colors.OrangeRed)
            };
            TopXAxesCollection = new ObservableCollection<IAxisViewModel> { topXAxis };

            var bottomXAxis = new NumericAxisViewModel
            {
                AxisTitle = "Bottom X Axis",
                AxisAlignment = AxisAlignment.Bottom,
                StyleKey = "XAxisStyle",
                Tag = new SolidColorBrush(Colors.ForestGreen)
            };
            BottomXAxesCollection = new ObservableCollection<IAxisViewModel> { bottomXAxis };

            // Set the initial value for X-Axis ranges
            SharedXVisibleRange = new DoubleRange(0d, 1d);
        }

        public ObservableCollection<IAxisViewModel> TopXAxesCollection
        {
            get => _topXAxesCollection;
            set
            {
                if (Equals(value, _topXAxesCollection)) return;

                _topXAxesCollection = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IAxisViewModel> BottomXAxesCollection
        {
            get => _bottomXAxesCollection;
            set
            {
                if (Equals(value, _bottomXAxesCollection)) return;

                _bottomXAxesCollection = value;
                OnPropertyChanged();
            }
        }

        public IRange SharedXVisibleRange
        {
            get => _sharedXVisibleRange;
            set
            {
                if (Equals(_sharedXVisibleRange, value)) return;

                _sharedXVisibleRange = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
