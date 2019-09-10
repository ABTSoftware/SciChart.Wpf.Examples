// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// GainLossPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.InspectDatapoints.SeriesWithMetadata
{
    public class GainLossPaletteProvider  : IStrokePaletteProvider
    {
        private double _previousValue = 0d; 

        public Color GainColor { get; set; }
        public Color LossColor { get; set; }

        /// <summary>
        /// Called at the start of an renderable series rendering, before the current draw operation.
        /// </summary>
        /// <param name="series"></param>
        public void OnBeginSeriesDraw(IRenderableSeries series){}

        /// <summary>
        /// Overrides the color of the outline on the attached <see cref="IRenderableSeries" />.
        /// Return null to keep the default series color.
        /// Return a value to override the series color.
        /// </summary>
        /// <param name="rSeries">The source <see cref="IRenderableSeries" />.</param>
        /// <param name="index">The index of the data-point. To get X,Y values use rSeries.DataSeries.XValues[index] etc...</param>
        /// <param name="metadata">The PointMetadata associated with this X,Y data-point.</param>
        /// <returns></returns>
        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            // Note: Since IPointMetadata is now passed to palette provider, we can use this too to affect coloring. 
            // In this case we use only YValue but #justsaying

            var currentValue = (double)rSeries.DataSeries.YValues[index];

            var isLoss = currentValue < _previousValue;

            _previousValue = currentValue;

            return isLoss ? LossColor : GainColor;
        }
    }
}
