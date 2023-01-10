// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
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
        private readonly Color _overriddenColor;
        private readonly Brush _overriddenFill;

        private IList<double> _yValues;

        public RedIfOverThresholdPaletteProvider()
        {
            _overriddenColor = Color.FromArgb(0xFF, 0xDC, 0x79, 0x69);
            _overriddenFill = new SolidColorBrush(_overriddenColor);
        }

        public double Threshold { get; set; }

        public Color? OverrideStrokeColor(IRenderableSeries series, int index, IPointMetadata metadata)
        {
            if (_yValues[index] > Threshold)
            {
                var color = _overriddenColor;
                color.A = (byte) (_overriddenColor.A * series.Opacity);
                return color;
            }

            // Returning null means use the default color when rendering
            return null;
        }

        public Brush OverrideFillBrush(IRenderableSeries series, int index, IPointMetadata metadata)
        {
            if (_yValues[index] > Threshold)
            {
                var colorBrush = _overriddenFill;
                colorBrush.Opacity *= series.Opacity;
                return colorBrush;
            }

            // Returning null means use the default color when rendering
            return null;
        }

        public void OnBeginSeriesDraw(IRenderableSeries series)
        {
            _yValues = ((IUniformXyDataSeries<double>)series.DataSeries).YValues;
        }
    }
}
