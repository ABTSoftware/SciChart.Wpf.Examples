// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
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
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.AnnotateAChart.DragHorizontalThreshold
{
    public class DragThresholdMvvmViewModel : BaseViewModel
    {
        private readonly RedIfOverThresholdPaletteProvider _paletteProvider;

        public DragThresholdMvvmViewModel()
        {
            ColumnDataSeries = new UniformXyDataSeries<double>(0d, 0.033);
            ColumnDataSeries.Append(DataManager.Instance.GetDampedSinewaveYData(1.0, 0.01, 300));

            _paletteProvider = new RedIfOverThresholdPaletteProvider();
            Threshold = 0.5;
        }

        public IViewportManager ViewportManager { get; } = new DefaultViewportManager();

        // We use a PaletteProvider type to override render colors for the column depending on threshold
        public IPaletteProvider ThresholdPaletteProvider => _paletteProvider;

        // We expose a DataSeries that is bound to in the view to RenderableSeries.DataSeriesProperty
        public IUniformXyDataSeries<double> ColumnDataSeries { get; private set; }

        /// <summary>
        /// We bind to Threshold in the view. This is the Y-value of the horizontal line. We pass through the value to the PaletteProvider, which 
        /// alters the color of columns on render
        /// </summary>
        public double Threshold
        {
            get => _paletteProvider.Threshold;
            set
            {
                _paletteProvider.Threshold = value;
                ViewportManager.InvalidateParentSurface(RangeMode.None);
                OnPropertyChanged(nameof(Threshold));
            }
        }
    }
}