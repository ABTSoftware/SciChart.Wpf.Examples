// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SeismicEvent.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;

namespace SciChart.Examples.ExternalDependencies.Data
{
    /// <summary>
    /// A single synthetic earthquake record: its position (longitude, latitude), focal depth,
    /// moment magnitude and origin time. Notable mainshocks also carry a display name.
    /// </summary>
    public class SeismicEvent
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double DepthKm { get; set; }

        public double Magnitude { get; set; }

        public string Name { get; set; }

        public DateTime Time { get; set; }
    }
}
