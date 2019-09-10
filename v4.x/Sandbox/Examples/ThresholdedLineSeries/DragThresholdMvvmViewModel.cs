// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DragThresholdMvvmViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.ObjectModel;
using System.Linq;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Sandbox.Shared;

namespace SciChart.Sandbox.ThresholdedLineSeries
{
    public class DragThresholdMvvmViewModel : BindableObject
    {
        private readonly XyyDataSeries<double, double> _dataSeries;
        private double _thresholdValue;

        public DragThresholdMvvmViewModel()
        {
            _dataSeries = new XyyDataSeries<double, double>();
            var someData = DataManager.Instance.GetDampedSinewave(1.0, 0.01, 300);
            _thresholdValue = 0.0;
            _dataSeries.Append(someData.XData, someData.YData, Enumerable.Repeat(_thresholdValue, someData.Count));                        
        }

        // We expose a DataSeries that is bound to in the view to RenderableSeries.DataSeriesProperty
        public XyyDataSeries<double, double> DataSeries
        {
            get { return _dataSeries; }
        }

        /// <summary>
        /// We bind to Threshold in the view. This is the Y-value of the horizontal line. We pass through the value to the PaletteProvider, which 
        /// alters the color of columns on render
        /// </summary>
        public double Threshold
        {
            get { return _thresholdValue; }
            set 
            {
                _thresholdValue = value;

                // Update the Y1 value of dataseries to update threshold 
                for (int i = 0; i < _dataSeries.Count; i++)
                {
                    _dataSeries.Y1Values[i] = _thresholdValue;
                }
                _dataSeries.HasDataChanges = true;
                _dataSeries.InvalidateParentSurface(RangeMode.None);

                OnPropertyChanged("Threshold");
            }
        }
    }
}
