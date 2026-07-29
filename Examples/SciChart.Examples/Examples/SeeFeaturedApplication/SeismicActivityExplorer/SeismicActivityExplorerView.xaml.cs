// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// SeismicActivityExplorerView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and pucblish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.SeismicActivityExplorer
{
    public partial class SeismicActivityExplorerView : UserControl
    {
        private const int SeismicEventCount = 25_000;

        private SeismicRegionDem _dem;
        private HeatmapColorPalette _currentPalette;
        private DepthTintConverter _tintConverter;

        private IList<SeismicEvent> _catalog;
        private IList<SeismicEventMetadata> _catalogMetadata;
        private double _catalogDepthMin;
        private double _catalogDepthMax;
        private double _catalogMaxMagnitude;

        public SeismicActivityExplorerView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _tintConverter = (DepthTintConverter)Resources["DepthTint"];

            if (_dem == null)
            {
                _dem = DataManager.Instance.GetSeismicTerrain();
                terrainSeries.DataSeries = new UniformHeatmapDataSeries<double, double, double>(
                    _dem.Elevation, SeismicRegionDem.LonStart, SeismicRegionDem.CellSizeDeg,
                    SeismicRegionDem.LatStart, SeismicRegionDem.CellSizeDeg);
            }

            if (_catalog == null)
            {
                GenerateSeismicEventsData();

                bubbleSeries.ColorMapValueProvider =
                    new DepthColorMapValueProvider(new DoubleRange(_catalogDepthMin, _catalogDepthMax));

                FillDataSeries();
                UpdateLegendInteraction();
            }

            // The ComboBoxes raise their initial SelectionChanged during InitializeComponent,
            // before the series exists, so apply the initial state now that everything is ready
            OnPaletteChanged(paletteCombo, null);
            OnRangeModeChanged(rangeModeCombo, null);
            OnFillBrushChanged(fillBrushCombo, null);

            SetInitialZoomExtents();
        }

        private void GenerateSeismicEventsData()
        {
            if (_dem == null) return;

            var catalog = DataManager.Instance.GenerateSeismicCatalog(_dem, SeismicEventCount);

            // Sort by longitude (the X axis) once, so both split series stay sorted-by-X and the
            // surface can binary-search hit-tests instead of scanning all points (see AcceptsUnsortedData).
            _catalog = catalog.Events.OrderBy(ev => ev.Longitude).ToList();
            _catalogDepthMin = catalog.MinDepthKm;
            _catalogDepthMax = catalog.MaxDepthKm;
            _catalogMaxMagnitude = catalog.MaxMagnitude;

            // The focal depth, time and name travel into each bubble as point metadata, where the
            // depth ColorMap value provider and the tooltip read them. Built once per catalog.
            _catalogMetadata = new List<SeismicEventMetadata>(_catalog.Count);
            foreach (var ev in _catalog)
            {
                _catalogMetadata.Add(new SeismicEventMetadata(ev.DepthKm, ev.Time, ev.Name));
            }
        }

        /// <summary>
        /// Splits the catalog into background events and highlighted mainshocks (at or above the
        /// magnitude threshold). The mainshocks form a separate series drawn on top of everything,
        /// and both series share one magnitude-to-size scale via SizeReferenceMagnitude.
        /// </summary>
        private void FillDataSeries()
        {
            if (_catalog == null || bubbleSeries == null || mainshockSeries == null) return;

            double mainshocksThreshold = thresholdSlider.Value;
            double maxDepthShown = depthFilterSlider.Value;

            // Both series receive points already in longitude (X) order, so neither needs AcceptsUnsortedData
            var seismicEventsDataSeries = new XyzDataSeries<double, double, double>
            {
                SeriesName = "Seismic Events"
            };
            var mainshocksDataSeries = new XyzDataSeries<double, double, double>
            {
                SeriesName = "Mainshocks"
            };

            using (seismicEventsDataSeries.SuspendUpdates())
            using (mainshocksDataSeries.SuspendUpdates())
            {
                for (int i = 0; i < _catalog.Count; i++)
                {
                    var ev = _catalog[i];

                    // Depth filter: hide events deeper than the cutoff
                    if (ev.DepthKm > maxDepthShown) continue;

                    var target = ev.Magnitude >= mainshocksThreshold ? mainshocksDataSeries : seismicEventsDataSeries;
                    target.Append(ev.Longitude, ev.Latitude, ev.Magnitude, _catalogMetadata[i]);
                }
            }

            bubbleSeries.SizeReferenceMagnitude = _catalogMaxMagnitude;
            mainshockSeries.SizeReferenceMagnitude = _catalogMaxMagnitude;

            bubbleSeries.DataSeries = seismicEventsDataSeries;
            mainshockSeries.DataSeries = mainshocksDataSeries;
        }

        private ColorMapRangeMode GetSelectedRangeMode()
        {
            return (rangeModeCombo.SelectedItem as ComboBoxItem)?.Tag as ColorMapRangeMode? ?? ColorMapRangeMode.Manual;
        }

        private void OnColorMapToggled(object sender, RoutedEventArgs e)
        {
            if (bubbleSeries == null) return;

            var activePalette = colorMapCheck.IsChecked == true ? _currentPalette : null;

            bubbleSeries.ColorMap = activePalette;

            // Tooltips tint with the active depth palette, falling back to neutral when off
            if (_tintConverter != null)
            {
                _tintConverter.Palette = activePalette;
            }

            UpdateColorLegend();
        }

        private void OnPaletteChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bubbleSeries == null) return;

            // Preserve the depth range the user has configured (sliders or legend drag)
            double previousMin = _currentPalette?.Minimum ?? 0;
            double previousMax = _currentPalette?.Maximum ?? 700;

            if (_currentPalette != null)
            {
                _currentPalette.PropertyChanged -= OnPalettePropertyChanged;
            }

            _currentPalette = (HeatmapColorPalette)((ComboBoxItem)paletteCombo.SelectedItem).Tag;

            _currentPalette.Minimum = previousMin;
            _currentPalette.Maximum = previousMax;

            // Redraw as soon as a legend drag (or anything else) changes the palette range
            _currentPalette.PropertyChanged += OnPalettePropertyChanged;

            UpdateLegendInteraction();
            OnColorMapToggled(colorMapCheck, null);
        }

        private void OnPalettePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HeatmapColorPalette.Minimum) || e.PropertyName == nameof(HeatmapColorPalette.Maximum))
            {
                sciChart?.InvalidateElement();
            }
        }

        private void OnRangeModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bubbleSeries == null) return;

            bubbleSeries.ColorMapRangeMode = GetSelectedRangeMode();

            UpdateLegendInteraction();
            sciChart?.InvalidateElement();
        }

        /// <summary>
        /// Configures the legend for the active range mode: axis drag is enabled whenever the
        /// range is not fully automatic, dragged values flow into the palette via two-way
        /// bindings, and the data-driven end (in AutoMin/AutoMax) is locked with a visible
        /// range limit so dragging cannot move it.
        /// </summary>
        private void UpdateLegendInteraction()
        {
            if (depthLegend == null || _currentPalette == null || _catalog == null) return;

            BindingOperations.ClearBinding(depthLegend, HeatmapColorMap.MinimumProperty);
            BindingOperations.ClearBinding(depthLegend, HeatmapColorMap.MaximumProperty);

            IRange dragLimit = null;
            var clipMode = RangeClipMode.MinMax;

            switch (GetSelectedRangeMode())
            {
                case ColorMapRangeMode.Auto:
                    // Both ends follow the data: dragging is meaningless, show the actual range
                    depthLegend.EnableAxisDrag = false;
                    depthLegend.Minimum = _catalogDepthMin;
                    depthLegend.Maximum = _catalogDepthMax;
                    break;

                case ColorMapRangeMode.AutoMax:
                    // Max follows the data (locked); the Min is the manual end, draggable on the legend
                    depthLegend.EnableAxisDrag = true;
                    BindLegendToPalette(HeatmapColorMap.MinimumProperty, nameof(HeatmapColorPalette.Minimum));
                    depthLegend.Maximum = _catalogDepthMax;
                    dragLimit = new DoubleRange(-10000, _catalogDepthMax);
                    clipMode = RangeClipMode.Max;
                    break;

                case ColorMapRangeMode.AutoMin:
                    // Min follows the data (locked); the Max is the manual end, draggable on the legend
                    depthLegend.EnableAxisDrag = true;
                    BindLegendToPalette(HeatmapColorMap.MaximumProperty, nameof(HeatmapColorPalette.Maximum));
                    depthLegend.Minimum = _catalogDepthMin;
                    dragLimit = new DoubleRange(_catalogDepthMin, 10000);
                    clipMode = RangeClipMode.Min;
                    break;

                default: // Manual: both ends draggable
                    depthLegend.EnableAxisDrag = true;
                    BindLegendToPalette(HeatmapColorMap.MinimumProperty, nameof(HeatmapColorPalette.Minimum));
                    BindLegendToPalette(HeatmapColorMap.MaximumProperty, nameof(HeatmapColorPalette.Maximum));
                    break;
            }

            ApplyLegendAxisStyle(dragLimit, clipMode);
        }

        private void BindLegendToPalette(DependencyProperty legendProperty, string palettePropertyPath)
        {
            depthLegend.SetBinding(legendProperty,
                new Binding(palettePropertyPath) { Source = _currentPalette, Mode = BindingMode.TwoWay });
        }

        /// <summary>
        /// Applies the XAML-defined color bar axis style, deriving from it (BasedOn) when the
        /// active range mode needs a visible range limit to lock the data-driven end during drag.
        /// </summary>
        private void ApplyLegendAxisStyle(IRange dragLimit, RangeClipMode clipMode)
        {
            var baseStyle = (Style)Resources["ColorBarAxisStyle"];

            if (dragLimit == null)
            {
                depthLegend.AxisStyle = baseStyle;
                return;
            }

            var style = new Style(typeof(NumericAxis)) { BasedOn = baseStyle };
            style.Setters.Add(new Setter(AxisBase.VisibleRangeLimitProperty, dragLimit));
            style.Setters.Add(new Setter(AxisBase.VisibleRangeLimitModeProperty, clipMode));

            depthLegend.AxisStyle = style;
        }

        private void UpdateColorLegend()
        {
            if (depthLegend == null || _currentPalette == null) return;

            // When depth coloring is off the legend stays in place but reads as inactive:
            // greyed-out gradient, faded, and not interactive
            bool active = colorMapCheck.IsChecked == true;

            if (active)
            {
                depthLegend.ColorPalette = _currentPalette;
            }
            else
            {
                var inactive = (HeatmapColorPalette)Resources["InactiveDepthColorMap"];
                inactive.Minimum = _currentPalette.Minimum;
                inactive.Maximum = _currentPalette.Maximum;
                depthLegend.ColorPalette = inactive;
            }

            depthLegend.Opacity = active ? 1.0 : 0.4;
            depthLegend.IsEnabled = active;
        }

        private void OnFilterChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_catalog == null) return;

            FillDataSeries();
        }

        private void OnFillBrushChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bubbleSeries == null) return;

            // Applies to the background events only; the mainshock series keeps its fixed look.
            // The default item carries no Tag, leaving FillBrush null (the series' own sprite).
            bubbleSeries.FillBrush = (fillBrushCombo.SelectedItem as ComboBoxItem)?.Tag as RadialGradientBrush;
        }

        /// <summary>
        /// Pins each axis's ZoomExtentsRange to the full region, so a double-click returns to exactly
        /// the startup view: <see cref="SciChart.Charting.ViewportManagers.AspectRatioViewportManager"/> then expands the tighter axis to
        /// the undistorted view - the same correction it applies to the region on first render.
        /// </summary>
        private void SetInitialZoomExtents()
        {
            longitudeAxis.ZoomExtentsRange = new DoubleRange(SeismicRegionDem.LonStart, SeismicRegionDem.LonEnd);
            latitudeAxis.ZoomExtentsRange = new DoubleRange(SeismicRegionDem.LatStart, SeismicRegionDem.LatEnd);
        }

        private void OnLayoutSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SyncLegendLayout();
        }

        /// <summary>
        /// Vertically aligns the depth legend with the surface's chart area (below the title,
        /// above the X axis), and keeps the invisible host axis exactly as wide as the legend,
        /// so the legend reads as an opposite Y axis sitting on the surface background.
        /// </summary>
        private void SyncLegendLayout()
        {
            var chartArea = sciChart.ModifierSurface as FrameworkElement;
            if (chartArea == null || chartArea.ActualHeight <= 0) return;

            double top = chartArea.TranslatePoint(new Point(0, 0), sciChart).Y;
            double bottom = sciChart.ActualHeight - top - chartArea.ActualHeight;
            if (top < 0 || bottom < 0) return;

            var margin = depthLegend.Margin;
            if (Math.Abs(margin.Top - top) > 0.5 || Math.Abs(margin.Bottom - bottom) > 0.5)
            {
                depthLegend.Margin = new Thickness(margin.Left, top, margin.Right, bottom);
            }

            double legendWidth = depthLegend.ActualWidth;
            if (legendWidth > 0 && Math.Abs(legendHostAxis.Width - legendWidth) > 0.5)
            {
                legendHostAxis.Width = legendWidth;
            }
        }
    }
}
