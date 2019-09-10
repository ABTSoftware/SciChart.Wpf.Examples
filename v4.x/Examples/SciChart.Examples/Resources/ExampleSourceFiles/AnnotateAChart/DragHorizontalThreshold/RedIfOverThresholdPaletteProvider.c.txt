// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RedIfOverThresholdPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.AnnotateAChart.DragHorizontalThreshold
{
    /// <summary>
    /// Defines a paletter provider to return a red color if the Y-Value is over a threshold value
    /// </summary>
    public class RedIfOverThresholdPaletteProvider : IStrokePaletteProvider, IFillPaletteProvider
    {
        private Color _overriddenColor;
        private Brush _overriddenFill;

        private IList<double> _yValues;

        public RedIfOverThresholdPaletteProvider()
        {
            _overriddenColor = Color.FromArgb(0xFF, 0xFF, 0x33, 0x33);
            _overriddenFill = new SolidColorBrush(_overriddenColor);
        }

        public double Threshold { get; set; }

        public Color? OverrideStrokeColor(IRenderableSeries series, int index, IPointMetadata metadata)
        {
            if (_yValues[index] > Threshold)
                return _overriddenColor;

            // Returning null means use the default color when rendering
            return null;
        }

        public Brush OverrideFillBrush(IRenderableSeries series, int index, IPointMetadata metadata)
        {
            if (_yValues[index] > Threshold)
                return _overriddenFill;

            // Returning null means use the default color when rendering
            return null;
        }

        public void OnBeginSeriesDraw(IRenderableSeries series)
        {
            var dataSeries = series.DataSeries as XyDataSeries<double, double>;

            _yValues = dataSeries.YValues;
        }
    }
}
