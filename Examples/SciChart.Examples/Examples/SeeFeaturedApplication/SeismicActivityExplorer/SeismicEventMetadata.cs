// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// SeismicEventMetadata.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.ComponentModel;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.SeismicActivityExplorer
{
    /// <summary>
    /// Carries the 4th data dimension of a seismic event: the focal depth in km.
    /// X/Y/Z hold longitude, latitude and magnitude, so depth travels in metadata and
    /// feeds the ColorMap via <see cref="DepthColorMapValueProvider"/>.
    /// </summary>
    public class SeismicEventMetadata : IPointMetadata
    {
        public SeismicEventMetadata(double depthKm, DateTime time, string eventName = null)
        {
            DepthKm = depthKm;
            Time = time;
            EventName = eventName;
        }

        public double DepthKm { get; }

        public DateTime Time { get; }

        public string EventName { get; }

        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { }
            remove { }
        }
    }

    /// <summary>
    /// Feeds each event's focal depth (carried in <see cref="SeismicEventMetadata"/>) into the
    /// bubble ColorMap, so color encodes depth while the Z value (magnitude) drives bubble size.
    /// </summary>
    public class DepthColorMapValueProvider : IPointColorMapValueProvider
    {
        private readonly IRange _depthRange;

        public DepthColorMapValueProvider(IRange depthRange)
        {
            _depthRange = depthRange;
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries) { }

        public double GetValue(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var seismicEvent = metadata as SeismicEventMetadata;
            return seismicEvent != null ? seismicEvent.DepthKm : double.NaN;
        }

        public IRange GetValueRange(IRenderableSeries rSeries, IndexRange pointRange) => _depthRange;
    }
}
