// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SeriesTooltipsViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.ChartData;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.InspectDatapoints
{
    public class SeriesTooltipsViewModel : BaseViewModel
    {
        private IXyDataSeries<double, double> _dataSeries1;
        private IXyDataSeries<double, double> _dataSeries2;
        
        private double _highThreshold;
        private double _lowThreshold;
        
        public SeriesTooltipsViewModel()
        {
            DataSeries1 = new XyDataSeries<double, double> {SeriesName = "Lissajous Curve", AcceptsUnsortedData = true};

            var data1 = DataManager.Instance.GetLissajousCurve(0.8, 0.2, 0.43, 500);

            // just scale the data in X a bit so it matches sinewave data. This is purely for the demo purposes
            DataSeries1.Append(data1.XData.Select(x => (x + 1.0) * 5.0), data1.YData);

            DataSeries2 = new XyDataSeries<double, double> { SeriesName = "Sinewave" };

            var data2 = DataManager.Instance.GetSinewave(1.5, 1.0, 500);

            DataSeries2.Append(data2.XData, data2.YData); 

            LowThreshold = -0.5d;
            HighThreshold = 0.5d;
        }

        public IXyDataSeries<double, double> DataSeries2
        {
            get { return _dataSeries2; }
            set
            {
                if (_dataSeries2 == value) return;
                _dataSeries2 = value;
                OnPropertyChanged("DataSeries2");
            }
        }

        public IXyDataSeries<double, double> DataSeries1
        {
            get { return _dataSeries1; }
            set
            {
                if (_dataSeries1 == value) return;
                _dataSeries1 = value;
                OnPropertyChanged("DataSeries1");
            }
        }

        public double HighThreshold
        {
            get { return _highThreshold; }
            set
            {
                if (_highThreshold == value) return;
                _highThreshold = value;
                OnPropertyChanged("HighThreshold");
            }
        }

        public double LowThreshold
        {
            get { return _lowThreshold; }
            set
            {
                if (_lowThreshold == value) return;
                _lowThreshold = value;
                OnPropertyChanged("LowThreshold");
            }
        }

        public Func<SeriesInfo, object> TooltipDataContext
        {
            get
            {
                return seriesInfo =>
                {
                    // Create and return a ToolTipViewModel for the tooltip
				    var vm = new TooltipViewModel
				    {
                        XValue = Convert.ToDouble(seriesInfo.XValue),
                        YValue = Convert.ToDouble(seriesInfo.YValue),
                        HighThreshold = this.HighThreshold,
                        LowThreshold = this.LowThreshold,                        
                        Title = seriesInfo.RenderableSeries.DataSeries.SeriesName,
                        Stroke = seriesInfo.Stroke,  
     
                        // Fill is not used, but we include it to prevent a Binding error from the
                        // overridden TooltipViewModel to the default TooltipModifier.TooltipContainerStyle which 
                        // expects a property of type Brush called Fill
                        Fill = Brushes.Transparent,
				    };

                    vm.OverThreshold = vm.YValue > HighThreshold;
                    vm.UnderThreshold = vm.YValue < LowThreshold;
                    return vm;
                };
            }
        }        
    }

    // Here we create a ViewModel for our ToolTip label. 
    // We assume you are using your own ViewModelBase
    public class TooltipViewModel : BaseViewModel
    {
        public double HighThreshold { get; set; }
        public double LowThreshold { get; set; }
        public double XValue { get; set;}
        public double YValue { get; set; }
        public string Title { get; set; }
        public string ThresholdText { get; set; }
        public bool OverThreshold { get; set; }
        public bool UnderThreshold { get; set; }
        public bool OverOrUnderThreshold  { get { return OverThreshold | UnderThreshold; }}
        public Color BackgroundColor { get; set; }
        public Color Stroke { get; set; }
        public Brush Fill { get; set; }
    }
}