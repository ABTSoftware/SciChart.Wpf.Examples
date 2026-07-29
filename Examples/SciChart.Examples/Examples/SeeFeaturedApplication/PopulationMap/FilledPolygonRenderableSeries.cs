// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// FilledPolygonRenderableSeries.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Drawing.Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.PopulationMap
{
    /// <summary>
    /// A custom RenderableSeries that draws a set of filled, outlined polygons, each with its own fill colour,
    /// plus a country-name watermark centred on each country. Country names are drawn behind the city bubbles
    /// (this series is first in the RenderableSeries collection) and only when the name fits inside the
    /// country's current on-screen bounds — so small countries reveal their names only when the map is zoomed in.
    /// The outline uses the base <see cref="BaseRenderableSeries.Stroke"/> and <see cref="BaseRenderableSeries.StrokeThickness"/>.
    /// </summary>
    public class FilledPolygonRenderableSeries : CustomRenderableSeries
    {
        // Watermark styling, tone-on-tone with the choropleth (semi-transparent warm dark).
        private static readonly Color NameColor = Color.FromArgb(0x55, 0x2B, 0x24, 0x18);
        private static readonly Color PopulationColor = Color.FromArgb(0x44, 0x2B, 0x24, 0x18);
        private static readonly FontFamily LabelFontFamily = new FontFamily("Segoe UI");
        private const float NameFontSize = 20f;
        private const float PopulationFontSize = 9f;

        // A label is drawn only when it fits within this fraction of the country's on-screen bounding box.
        private const double FitMargin = 0.9;

        private readonly List<(Point[] Ring, Color Fill)> _polygons = new List<(Point[], Color)>();
        private readonly List<CountryLabel> _labels = new List<CountryLabel>();

        public void AddPolygon(Point[] ring, Color fillColor) => _polygons.Add((ring, fillColor));

        /// <summary>
        /// Registers a country name watermark, drawn centred on <paramref name="centroid"/> only when it fits
        /// within the country's on-screen bounding box (projected bounds <paramref name="projectedMin"/>..<paramref name="projectedMax"/>).
        /// </summary>
        public void AddCountryLabel(CountryCentroid country, Point centroid, Point projectedMin, Point projectedMax)
            => _labels.Add(new CountryLabel(country, centroid, projectedMin, projectedMax));

        protected override bool GetIsValidForDrawing() => IsVisible && _polygons.Count > 0;

        protected override void Draw(IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            var xCalc = renderPassData.XCoordinateCalculator;
            var yCalc = renderPassData.YCoordinateCalculator;

            using (var pen = renderContext.CreatePen(Stroke, AntiAliasing, (float)StrokeThickness, Opacity))
            {
                foreach (var (ring, fillColor) in _polygons)
                {
                    var screen = new Point[ring.Length];
                    for (int i = 0; i < ring.Length; i++)
                        screen[i] = new Point(xCalc.GetCoordinate(ring[i].X), yCalc.GetCoordinate(ring[i].Y));

                    using (var brush = renderContext.CreateBrush(fillColor, 1.0))
                        renderContext.FillPolygon(brush, screen);

                    var closed = new Point[screen.Length + 1];
                    Array.Copy(screen, closed, screen.Length);
                    closed[screen.Length] = screen[0];
                    renderContext.DrawLines(pen, closed);
                }
            }

            DrawCountryLabels(renderContext, renderPassData);
        }

        // Draws each country name centred on its centroid, skipping any that would overflow the country's
        // current on-screen size. Text sizes are constant, so they are measured once and cached.
        private void DrawCountryLabels(IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            var xCalc = renderPassData.XCoordinateCalculator;
            var yCalc = renderPassData.YCoordinateCalculator;

            foreach (var label in _labels)
            {
                if (!label.Measured)
                {
                    label.NameSize = renderContext.MeasureText(label.Name, NameFontSize, LabelFontFamily, FontWeights.SemiBold, FontStyles.Normal);
                    label.PopulationSize = renderContext.MeasureText(label.Population, PopulationFontSize, LabelFontFamily, FontWeights.Normal, FontStyles.Normal);
                    label.Measured = true;
                }

                double boxWidth = Math.Abs(xCalc.GetCoordinate(label.Max.X) - xCalc.GetCoordinate(label.Min.X));
                double boxHeight = Math.Abs(yCalc.GetCoordinate(label.Max.Y) - yCalc.GetCoordinate(label.Min.Y));

                double labelWidth = Math.Max(label.NameSize.Width, label.PopulationSize.Width);
                double labelHeight = label.NameSize.Height + label.PopulationSize.Height;

                if (labelWidth > boxWidth * FitMargin || labelHeight > boxHeight * FitMargin) continue;

                double centreX = xCalc.GetCoordinate(label.Centroid.X);
                double centreY = yCalc.GetCoordinate(label.Centroid.Y);
                double top = centreY - labelHeight * 0.5;

                renderContext.DrawText(new Point(centreX - label.NameSize.Width * 0.5, top),
                    label.Name, NameFontSize, LabelFontFamily, FontWeights.SemiBold, FontStyles.Normal, NameColor);
                renderContext.DrawText(new Point(centreX - label.PopulationSize.Width * 0.5, top + label.NameSize.Height),
                    label.Population, PopulationFontSize, LabelFontFamily, FontWeights.Normal, FontStyles.Normal, PopulationColor);
            }
        }

        private sealed class CountryLabel
        {
            public readonly string Name;
            public readonly string Population;
            public readonly Point Centroid;
            public readonly Point Min;
            public readonly Point Max;

            public bool Measured;
            public Size NameSize;
            public Size PopulationSize;

            public CountryLabel(CountryCentroid country, Point centroid, Point projectedMin, Point projectedMax)
            {
                Name = country.Name;
                Population = country.PopulationLabel;
                Centroid = centroid;
                Min = projectedMin;
                Max = projectedMax;
            }
        }
    }
}
