// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// UseDataLabelProvider.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.RenderableSeries.DataLabelProviders;

namespace SciChart.Examples.Examples.StyleAChart.UseDataLabelProvider
{
    public partial class UseDataLabelProvider : UserControl
    {
        private const int LinePointCount = 150;
        private const double XRangeMax = LinePointCount - 1;

        private bool _updatingToolbar;

        public UseDataLabelProvider()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitColumnData();
            InitLineData();
            InitBubbleData();
            InitSplineData();

            sciChart.ZoomExtents();
        }

        private void InitColumnData()
        {
            const int columnCount = 15;
            var dataSeries = new UniformXyDataSeries<double>(0, XRangeMax / (columnCount - 1)) { SeriesName = "Columns" };
            var random = new Random(42);
            for (int i = 0; i < columnCount; i++)
            {
                dataSeries.Append(random.NextDouble() * 20 - 5);
            }

            columnSeries.DataSeries = dataSeries;
        }

        private void InitLineData()
        {
            var dataSeries = new UniformXyDataSeries<double> { SeriesName = "Line + Metadata" };
            for (int i = 0; i < LinePointCount; i++)
            {
                double t = i / (double)LinePointCount;
                double carrier = Math.Sin(2 * Math.PI * 8 * t);
                double modulator = 0.5 + 0.5 * Math.Sin(2 * Math.PI * 1.2 * t);
                double y = 4 * carrier * modulator + 22;
                dataSeries.Append(y, new LabelPointMetadata($"Label {i + 1}"));
            }

            lineSeries.DataSeries = dataSeries;
            lineSeries.DataLabelProvider = new PointDataLabelProvider
            {
                MetadataLabelSelector = md => (md as LabelPointMetadata)?.Label,
                LabelVerticalAnchorPoint = VerticalAnchorPoint.Auto,
                SkipMode = DataLabelSkipMode.SkipIfOverlap
            };
        }

        private void InitBubbleData()
        {
            const int bubbleCount = 12;
            var dataSeries = new XyzDataSeries<double, double, double> { SeriesName = "Bubbles" };
            var random = new Random(99);
            for (int i = 0; i < bubbleCount; i++)
            {
                double x = XRangeMax * i / (bubbleCount - 1);
                dataSeries.Append(x, random.NextDouble() * 6 + 30, random.NextDouble() * 15 + 5);
            }

            bubbleSeries.DataSeries = dataSeries;
        }

        private void InitSplineData()
        {
            const int pointCount = 20;
            const double amplitude = 5;
            const double offset = 42;

            var baseSeries = new UniformXyDataSeries<double>(0, XRangeMax / (pointCount - 1));
            for (int i = 0; i < pointCount; i++)
            {
                baseSeries.Append(amplitude * Math.Sin(2 * Math.PI * i / 8.0) + offset);
            }

            splineSeries.DataSeries = baseSeries.ToSpline(10);
            splineSeries.DataSeries.SeriesName = "Spline";
            splineSeries.DataLabelProvider = new PointDataLabelProvider
            {
                LabelVerticalAnchorPoint = VerticalAnchorPoint.Auto,
                SkipMode = DataLabelSkipMode.SkipIfOverlap
            };
        }

        private void OnSeriesSelectionChanged(object sender, EventArgs e)
        {
            var selected = sciChart.SelectedRenderableSeries.FirstOrDefault() as BaseRenderableSeries;
            var hasSelection = selected != null;

            btnPositioning.IsEnabled = hasSelection;
            btnCulling.IsEnabled = hasSelection;
            btnText.IsEnabled = hasSelection;

            SyncToolbarFromProvider(selected);
        }

        private void OnLabelOptionChanged(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded || _updatingToolbar) return;

            var selected = sciChart.SelectedRenderableSeries.FirstOrDefault() as BaseRenderableSeries;

            // ApplyLabelOptions wraps the changes in SuspendUpdates, which issues a single redraw on resume
            ApplyLabelOptions(selected);
        }

        private void SyncToolbarFromProvider(BaseRenderableSeries series)
        {
            _updatingToolbar = true;
            try
            {
                var labelSeries = series as IDataLabelProviderSeries;
                chkShowLabels.IsChecked = labelSeries?.ShowDataLabels == true;

                var provider = labelSeries?.DataLabelProvider as PointDataLabelProvider;
                if (provider == null) return;

                txtNumericFormat.Text = provider.LabelTextFormatting;
                sliderSkipNumber.Value = provider.SkipNumber;
                sliderMarginLeft.Value = provider.LabelPadding.Left;
                sliderMarginTop.Value = provider.LabelPadding.Top;
                sliderMarginRight.Value = provider.LabelPadding.Right;
                sliderMarginBottom.Value = provider.LabelPadding.Bottom;
                sliderThreshold.Value = provider.PointCountThreshold;
                sliderFontSize.Value = series.FontSize;

                var hasBarAnchor = provider is BarDataLabelProvider;
                txtBarAnchor.Visibility = cboBarAnchorPoint.Visibility =
                    hasBarAnchor ? Visibility.Visible : Visibility.Collapsed;

                SelectComboItem(cboHAlign, provider.LabelHorizontalAnchorPoint.ToString());
                SelectComboItem(cboVAlign, provider.LabelVerticalAnchorPoint.ToString());
                SelectComboItem(cboSkipMode, provider.SkipMode.ToString());
                SelectColorComboItem(series.Foreground as SolidColorBrush);
                if (provider is BarDataLabelProvider columnProvider)
                {
                    SelectComboItem(cboBarAnchorPoint, columnProvider.BarAnchorPoint.ToString());
                }
            }
            finally
            {
                _updatingToolbar = false;
            }
        }

        private static void SelectComboItem(ComboBox combo, string value)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (string.Equals(combo.Items[i]?.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void SelectColorComboItem(SolidColorBrush brush)
        {
            if (brush == null) return;

            var color = brush.Color;
            for (int i = 0; i < cboColor.Items.Count; i++)
            {
                var itemColor = (Color)ColorConverter.ConvertFromString((string)cboColor.Items[i]);
                if (itemColor == color)
                {
                    cboColor.SelectedIndex = i;
                    return;
                }
            }
        }

        private void ApplyLabelOptions(BaseRenderableSeries series)
        {
            // Suspend updates so the batch of property changes triggers a single redraw on resume
            using (sciChart.SuspendUpdates())
            {
                var labelSeries = series as IDataLabelProviderSeries;
                if (labelSeries != null)
                {
                    labelSeries.ShowDataLabels = chkShowLabels.IsChecked == true;
                }

                var provider = labelSeries?.DataLabelProvider as PointDataLabelProvider;
                if (provider == null) return;

                provider.LabelTextFormatting = txtNumericFormat.Text;
                provider.LabelHorizontalAnchorPoint = (HorizontalAnchorPoint)cboHAlign.SelectedItem;
                provider.LabelVerticalAnchorPoint = (VerticalAnchorPoint)cboVAlign.SelectedItem;
                provider.SkipMode = (DataLabelSkipMode)cboSkipMode.SelectedItem;
                provider.SkipNumber = (int)sliderSkipNumber.Value;
                provider.LabelPadding = new Thickness(sliderMarginLeft.Value, sliderMarginTop.Value, sliderMarginRight.Value, sliderMarginBottom.Value);
                provider.PointCountThreshold = (int)sliderThreshold.Value;

                series.FontSize = sliderFontSize.Value;
                series.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)cboColor.SelectedItem));

                if (provider is BarDataLabelProvider columnProvider)
                {
                    columnProvider.BarAnchorPoint = (BarAnchorPoint)cboBarAnchorPoint.SelectedItem;
                }
            }
        }
    }
}
