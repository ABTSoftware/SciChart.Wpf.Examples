// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CountryPointMetadata.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using System;
using System.ComponentModel;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.PopulationMap
{
    public class CountryPointMetadata : IPointMetadata
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsSelected { get; set; }
        public string Name { get; }
        public string PopulationLabel { get; }
        public string Country { get; }

        /// <summary>
        /// True when this city is its country's capital. Drives the selective data label
        /// (only capitals are labelled) via <see cref="CityBubbleSeries"/>' MetadataLabelSelector.
        /// </summary>
        public bool IsCapital { get; }

        /// <summary>
        /// The city's share of its country's population (urban primacy), or NaN when the country is unknown.
        /// Drives the bubble ColorMap via <see cref="CityPrimacyColorMapValueProvider"/>.
        /// </summary>
        public double NationalShare { get; }

        public CountryPointMetadata(CountryCentroid c)
        {
            Name = c.Name;
            PopulationLabel = c.PopulationLabel;
            NationalShare = double.NaN;
        }

        public CountryPointMetadata(string name, long population, string country = null, double nationalShare = double.NaN, bool isCapital = false)
        {
            Name = name;
            Country = country;
            NationalShare = nationalShare;
            IsCapital = isCapital;
            PopulationLabel = population >= 1_000_000 ? $"{population / 1_000_000.0:F1} M"
                            : population >= 1_000     ? $"{population / 1_000.0:F0} k"
                            : population.ToString();
        }
    }

    /// <summary>
    /// Feeds each city's urban primacy (its share of national population, carried in
    /// <see cref="CountryPointMetadata.NationalShare"/>) into the bubble ColorMap, so color encodes
    /// primacy while the Z value (population) drives bubble size. The value is log-scaled.
    /// </summary>
    public class CityPrimacyColorMapValueProvider : IPointColorMapValueProvider
    {
        // Mirrors the CityPrimacyPalette Minimum/Maximum set in XAML. The series uses ColorMapRangeMode.Manual,
        // so the palette's own range defines the scale; this is consulted only by the Auto range modes.
        private static readonly IRange LogPrimacyRange = new DoubleRange(-4.5, 0);

        public void OnBeginSeriesDraw(IRenderableSeries rSeries) { }

        public double GetValue(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            return metadata is CountryPointMetadata city && city.NationalShare > 0.0
                ? Math.Log10(city.NationalShare)
                : double.NaN;
        }

        public IRange GetValueRange(IRenderableSeries rSeries, IndexRange pointRange) => LogPrimacyRange;
    }
}
