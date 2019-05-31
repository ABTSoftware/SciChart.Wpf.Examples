// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
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
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.AnnotateAChart.DragHorizontalThreshold
{
    public class DragThresholdMvvmViewModel : BaseViewModel
    {
        private readonly RedIfOverThresholdPaletteProvider _paletteProvider;

        // Allows us to call SciChart.InvalidateElement() from the viewmodel
        private readonly IViewportManager _viewportManager = new DefaultViewportManager();
        private readonly XyDataSeries<double, double> _dataSeries;

        public DragThresholdMvvmViewModel()
        {
            _dataSeries = new XyDataSeries<double, double>();
            var someData = DataManager.Instance.GetDampedSinewave(1.0, 0.01, 300);
            _dataSeries.Append(someData.XData, someData.YData);
            
            _paletteProvider = new RedIfOverThresholdPaletteProvider();
            Threshold = 0.5;
        }

        public IViewportManager ViewportManager
        {
            get { return _viewportManager; }
        }

        // We use a PaletteProvider type to override render colors for the column depending on threshold
        public IPaletteProvider ThresholdPaletteProvider
        {
            get { return _paletteProvider; }
        }

        // We expose a DataSeries that is bound to in the view to RenderableSeries.DataSeriesProperty
        public XyDataSeries<double, double> ColumnDataSeries
        {
            get { return _dataSeries; }
        }

        /// <summary>
        /// We bind to Threshold in the view. This is the Y-value of the horizontal line. We pass through the value to the PaletteProvider, which 
        /// alters the color of columns on render
        /// </summary>
        public double Threshold
        {
            get { return _paletteProvider.Threshold; }
            set 
            { 
                _paletteProvider.Threshold = value;
                _viewportManager.InvalidateParentSurface(RangeMode.None);
                OnPropertyChanged("Threshold");
            }
        }
    }
}
