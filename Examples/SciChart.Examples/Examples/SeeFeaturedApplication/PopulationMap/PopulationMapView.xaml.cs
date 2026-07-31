// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PopulationMapView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.RenderableSeries.DataLabelProviders;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.PopulationMap
{
    public partial class PopulationMapView : UserControl
    {
        // Country choropleth palette (defined in XAML); GetColor returns the fill colour for a population.
        private readonly HeatmapColorPalette CountryPopulationPalette;

        // Source data and the ISO-2 -> population / name lookups, cached for the population filter to reuse.
        private List<City> _cities;
        private IDictionary<string, long> _countryPopulation;
        private IDictionary<string, string> _countryNames;

        // Largest population among the displayed cities; the reference size for the bubble size legend.
        private double _maxCityPopulation = 0;

        private bool _loaded;

        public PopulationMapView()
        {
            InitializeComponent();

            // Resolve the choropleth palette from the XAML resources and prime its colour lookup.
            CountryPopulationPalette = (HeatmapColorPalette)Resources["CountryPopulationPalette"];
            CountryPopulationPalette.InitializeColorMap(typeof(PopulationMapView), 0.8);
        }

        /// <summary>
        /// Builds the whole map once the control is loaded: the country choropleth, the city bubble data,
        /// the per-country labels, the two legends, and the initial viewport.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Loaded can fire again when the example is re-shown; build the map only once.
            if (_loaded) return;
            _loaded = true;

            var countryData = DataManager.Instance.GetEuropeMapData();

            var countryPopulation = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            var countryNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Project each country's polygon rings from lon/lat to LAEA, add them to the choropleth series
            // shaded by population, and grow the projected bounding box of the map.
            double minX = double.MaxValue, minY = double.MaxValue, maxX = double.MinValue, maxY = double.MinValue;
            foreach (var country in countryData)
            {
                var fillColor = GetFillColorForPopulation(country.Population);
                double bestRingArea = -1d;
                double lMinX = 0, lMinY = 0, lMaxX = 0, lMaxY = 0;
                foreach (var ring in country.PolygonRings)
                {
                    var projected = new Point[ring.Length];
                    double rMinX = double.MaxValue, rMinY = double.MaxValue, rMaxX = double.MinValue, rMaxY = double.MinValue;
                    for (int i = 0; i < ring.Length; i++)
                    {
                        var p = DataManager.Wgs84ToLaea(ring[i].X, ring[i].Y);
                        projected[i] = p;

                        if (p.X < minX) minX = p.X;
                        if (p.X > maxX) maxX = p.X;
                        if (p.Y < minY) minY = p.Y;
                        if (p.Y > maxY) maxY = p.Y;

                        if (p.X < rMinX) rMinX = p.X;
                        if (p.X > rMaxX) rMaxX = p.X;
                        if (p.Y < rMinY) rMinY = p.Y;
                        if (p.Y > rMaxY) rMaxY = p.Y;
                    }
                    countrySeries.AddPolygon(projected, fillColor);

                    // The label fit test uses the largest polygon's bounds (the mainland the centroid sits on),
                    // not all rings combined — otherwise offshore islands/exclaves inflate the box so the name
                    // shows (and overflows) even when the mainland is too small on screen.
                    double ringArea = (rMaxX - rMinX) * (rMaxY - rMinY);
                    if (ringArea > bestRingArea)
                    {
                        bestRingArea = ringArea;
                        lMinX = rMinX; lMaxX = rMaxX; lMinY = rMinY; lMaxY = rMaxY;
                    }
                }

                if (country.Population > 0)
                {
                    // Register the country name as a watermark the series draws (behind the cities) only when it
                    // fits its mainland's on-screen bounds. The ISO-2 maps index population and name for the cities.
                    var centroid = DataManager.Wgs84ToLaea(country.CentroidLon, country.CentroidLat);
                    countrySeries.AddCountryLabel(
                        new CountryCentroid(country.Name, country.CentroidLon, country.CentroidLat, country.Population),
                        centroid, new Point(lMinX, lMinY), new Point(lMaxX, lMaxY));

                    if (!string.IsNullOrEmpty(country.Id))
                    {
                        countryPopulation[country.Id] = country.Population;
                        countryNames[country.Id] = country.Name;
                    }
                }
            }

            // Cache the source data so the filter slider can rebuild the bubbles without re-reading it.
            _cities = DataManager.Instance.GetEuropeCities();
            _countryPopulation = countryPopulation;
            _countryNames = countryNames;

            // Label only capital cities with their name; returning an empty string suppresses the label for
            // every other city (returning null would fall back to the numeric Y value). The series holds
            // every city (thousands) but only the ~40 capitals are labelled, so lift the point-count guard
            // (default 1000, which would otherwise suppress all labels on so large a series); overlap culling
            // keeps the result readable. Set once here since the provider persists across the data-series
            // rebuilds the population filter triggers.
            if (citySeries.DataLabelProvider is PointDataLabelProvider capitalLabels)
            {
                capitalLabels.MetadataLabelSelector =
                    md => md is CountryPointMetadata city && city.IsCapital ? city.Name : string.Empty;
                capitalLabels.PointCountThreshold = int.MaxValue;
            }

            // The bubble series is declared and configured in XAML; only its data is built here.
            FillCityDataSeries(minPopSlider.Value);

            // Fill the country colour key and the city size key.
            PopulationLegend.ItemsSource = BuildLegendItems();
            CitySizeLegend.ItemsSource = BuildCitySizeLegendItems();

            // Set the visible range (and the double-click zoom-extents range) to the padded projected bounds.
            var padX = (maxX - minX) * 0.02;
            var padY = (maxY - minY) * 0.02;
            var xRange = new DoubleRange(minX - padX, maxX + padX);
            var yRange = new DoubleRange(minY - padY, maxY + padY);
            ChartSurface.XAxis.VisibleRange = xRange;
            ChartSurface.YAxis.VisibleRange = yRange;
            ChartSurface.XAxis.ZoomExtentsRange = xRange;
            ChartSurface.YAxis.ZoomExtentsRange = yRange;
            ChartSurface.ZoomExtents();
        }

        /// <summary>
        /// Builds the country colour-key rows: a swatch coloured from the choropleth palette at evenly
        /// spaced population values, with a matching label.
        /// </summary>
        private List<PopulationLegendItem> BuildLegendItems()
        {
            var labels = new[] { "1M", "20M", "40M", "60M", "80M", "100+M" };
            var step = (CountryPopulationPalette.Maximum - CountryPopulationPalette.Minimum) / (labels.Length - 1);
            var items = new List<PopulationLegendItem>(labels.Length);
            for (int i = 0; i < labels.Length; i++)
            {
                var population = step * i;
                items.Add(new PopulationLegendItem { Color = GetFillColorForPopulation(population), Label = labels[i] });
            }
            return items;
        }

        /// <summary>Returns the choropleth fill colour for the given population.</summary>
        private Color GetFillColorForPopulation(double population)
        {
            return CountryPopulationPalette.GetColor(population).ToColor();
        }

        /// <summary>
        /// Builds the city size-key rows: a circle whose diameter equals the on-map bubble size for each
        /// population bracket. Brackets larger than the biggest displayed city are skipped.
        /// </summary>
        private List<CityLegendItem> BuildCitySizeLegendItems()
        {
            var brackets = new long[] { 2_000_000, 4_000_000, 6_000_000, 8_000_000, 10_000_000, 12_000_000 };
            var labels = new[] { "2M", "4M", "6M", "8M", "10M", "12M" };
            var items = new List<CityLegendItem>(brackets.Length);
            for (int i = 0; i < brackets.Length; i++)
            {
                if (brackets[i] > _maxCityPopulation) continue;
                items.Add(new CityLegendItem
                {
                    Diameter = CityBubbleSeries.ComputeDiameter(brackets[i], _maxCityPopulation,
                        citySeries.MinBubbleSizeInPixels, citySeries.MaxBubbleSizeInPixels),
                    Label = labels[i],
                });
            }
            return items;
        }

        /// <summary>
        /// Rebuilds the bubble series' data from the cached cities, keeping only those at or above
        /// <paramref name="minPopulation"/> and belonging to a mapped country. Each point carries the
        /// projected position (X/Y), the population (Z), and metadata (name, country, urban primacy).
        /// Also records the largest displayed population for the size legend.
        /// </summary>
        private void FillCityDataSeries(double minPopulation)
        {
            long maxPopulation = 0;
            var dataSeries = new XyzDataSeries<double, double, double> { AcceptsUnsortedData = true };
            foreach (var c in _cities)
            {
                if (c.Population <= 0 || c.Population < minPopulation) continue;

                // Keep only cities whose country is on the map; look up that country's population.
                if (string.IsNullOrEmpty(c.Country) ||
                    !_countryPopulation.TryGetValue(c.Country, out var countryPop) || countryPop <= 0)
                {
                    continue;
                }

                if (c.Population > maxPopulation) maxPopulation = c.Population;

                // Urban primacy: the city's share of its country's population.
                var share = (double)c.Population / countryPop;
                _countryNames.TryGetValue(c.Country, out var countryName);

                // Project the city's lon/lat to LAEA coordinates.
                var p = DataManager.Wgs84ToLaea(c.Lon, c.Lat);

                // Append the point: X/Y = projected position, Z = population, metadata = name/country/primacy/capital.
                dataSeries.Append(p.X, p.Y, c.Population,
                    new CountryPointMetadata(c.Name, c.Population, countryName ?? c.Country, share, c.IsCapital));
            }
            citySeries.DataSeries = dataSeries;
            _maxCityPopulation = maxPopulation;

            // Update the city count label
            cityCountTxt.Text = $"({dataSeries.Count})";
        }

        /// <summary>Filter-slider handler: rebuilds the bubbles for the new minimum-population threshold.</summary>
        private void OnMinPopulationChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_cities == null) return;
            FillCityDataSeries(e.NewValue);
        }
    }
}
