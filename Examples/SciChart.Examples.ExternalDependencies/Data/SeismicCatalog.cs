// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SeismicCatalog.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Collections.Generic;

namespace SciChart.Examples.ExternalDependencies.Data
{
    /// <summary>
    /// A generated set of <see cref="SeismicEvent"/>s together with the catalog-wide ranges
    /// derived from it: the focal-depth bounds (for the depth color scale) and the largest
    /// magnitude (the shared reference for bubble sizing).
    /// </summary>
    public class SeismicCatalog
    {
        internal SeismicCatalog(IList<SeismicEvent> events, double minDepthKm, double maxDepthKm, double maxMagnitude)
        {
            Events = events;
            MinDepthKm = minDepthKm;
            MaxDepthKm = maxDepthKm;
            MaxMagnitude = maxMagnitude;
        }

        public IList<SeismicEvent> Events { get; }

        public double MinDepthKm { get; }

        public double MaxDepthKm { get; }

        public double MaxMagnitude { get; }
    }
}
