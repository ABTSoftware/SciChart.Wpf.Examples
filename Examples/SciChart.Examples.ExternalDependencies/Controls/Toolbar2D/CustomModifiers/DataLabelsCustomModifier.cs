// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// DataLabelsCustomModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.ChartModifiers;
using SciChart.Core.Framework;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.RenderableSeries.DataLabelProviders;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers
{
    /// <summary>
    /// A developer-mode modifier that exposes the <see cref="IDataLabelProvider"/> API of the currently
    /// selected series through the example toolbar. Acts as a view model for the Positioning, Culling and
    /// Text flyout menus: it syncs its properties from the selected series and writes edits back to it.
    /// Requires the <see cref="SeriesSelectionModifier"/> to be enabled so a series can be selected.
    /// </summary>
    public class DataLabelsCustomModifier : ChartModifierBase
    {
        private BaseRenderableSeries _selectedSeries;
        private bool _suppressApply;

        private bool _hasSelection;
        private bool _isBarSeries;
        private bool _showDataLabels;
        private HorizontalAnchorPoint _labelHorizontalAnchorPoint = HorizontalAnchorPoint.Center;
        private VerticalAnchorPoint _labelVerticalAnchorPoint = VerticalAnchorPoint.Top;
        private BarAnchorPoint _barAnchorPoint = BarAnchorPoint.Auto;
        private DataLabelSkipMode _skipMode = DataLabelSkipMode.ShowAll;
        private double _skipNumber;
        private double _pointCountThreshold = 1000;
        private double _paddingLeft;
        private double _paddingTop = 3;
        private double _paddingRight;
        private double _paddingBottom = 3;
        private string _labelTextFormatting = "0.##";
        private double _fontSize = 14;
        private string _labelColor = "White";

        private static readonly string[] NamedColors = { "White", "Yellow", "LimeGreen", "Cyan", "Black" };

        public bool HasSelection
        {
            get => _hasSelection;
            private set { _hasSelection = value; OnPropertyChanged(nameof(HasSelection)); }
        }

        public bool IsBarSeries
        {
            get => _isBarSeries;
            private set { _isBarSeries = value; OnPropertyChanged(nameof(IsBarSeries)); }
        }

        public bool ShowDataLabels
        {
            get => _showDataLabels;
            set { _showDataLabels = value; OnPropertyChanged(nameof(ShowDataLabels)); Apply(); }
        }

        public HorizontalAnchorPoint LabelHorizontalAnchorPoint
        {
            get => _labelHorizontalAnchorPoint;
            set { _labelHorizontalAnchorPoint = value; OnPropertyChanged(nameof(LabelHorizontalAnchorPoint)); Apply(); }
        }

        public VerticalAnchorPoint LabelVerticalAnchorPoint
        {
            get => _labelVerticalAnchorPoint;
            set { _labelVerticalAnchorPoint = value; OnPropertyChanged(nameof(LabelVerticalAnchorPoint)); Apply(); }
        }

        public BarAnchorPoint BarAnchorPoint
        {
            get => _barAnchorPoint;
            set { _barAnchorPoint = value; OnPropertyChanged(nameof(BarAnchorPoint)); Apply(); }
        }

        public DataLabelSkipMode SkipMode
        {
            get => _skipMode;
            set { _skipMode = value; OnPropertyChanged(nameof(SkipMode)); Apply(); }
        }

        public double SkipNumber
        {
            get => _skipNumber;
            set { _skipNumber = value; OnPropertyChanged(nameof(SkipNumber)); Apply(); }
        }

        public double PointCountThreshold
        {
            get => _pointCountThreshold;
            set { _pointCountThreshold = value; OnPropertyChanged(nameof(PointCountThreshold)); Apply(); }
        }

        public double PaddingLeft
        {
            get => _paddingLeft;
            set { _paddingLeft = value; OnPropertyChanged(nameof(PaddingLeft)); Apply(); }
        }

        public double PaddingTop
        {
            get => _paddingTop;
            set { _paddingTop = value; OnPropertyChanged(nameof(PaddingTop)); Apply(); }
        }

        public double PaddingRight
        {
            get => _paddingRight;
            set { _paddingRight = value; OnPropertyChanged(nameof(PaddingRight)); Apply(); }
        }

        public double PaddingBottom
        {
            get => _paddingBottom;
            set { _paddingBottom = value; OnPropertyChanged(nameof(PaddingBottom)); Apply(); }
        }

        public string LabelTextFormatting
        {
            get => _labelTextFormatting;
            set { _labelTextFormatting = value; OnPropertyChanged(nameof(LabelTextFormatting)); Apply(); }
        }

        public double LabelFontSize
        {
            get => _fontSize;
            set { _fontSize = value; OnPropertyChanged(nameof(LabelFontSize)); Apply(); }
        }

        public string LabelColor
        {
            get => _labelColor;
            set { _labelColor = value; OnPropertyChanged(nameof(LabelColor)); Apply(); }
        }

        public override void OnAttached()
        {
            base.OnAttached();

            if (ParentSurface != null)
            {
                ParentSurface.SelectedRenderableSeries.CollectionChanged += OnSelectedSeriesChanged;
            }
        }

        public override void OnDetached()
        {
            if (ParentSurface != null)
            {
                ParentSurface.SelectedRenderableSeries.CollectionChanged -= OnSelectedSeriesChanged;
            }

            base.OnDetached();
        }

        private void OnSelectedSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _selectedSeries = ParentSurface?.SelectedRenderableSeries
                .OfType<BaseRenderableSeries>()
                .FirstOrDefault();

            SyncFromSeries();
        }

        private void SyncFromSeries()
        {
            _suppressApply = true;
            try
            {
                var labelSeries = _selectedSeries as IDataLabelProviderSeries;
                HasSelection = labelSeries != null;
                if (labelSeries == null) return;

                ShowDataLabels = labelSeries.ShowDataLabels;

                var provider = labelSeries.DataLabelProvider as PointDataLabelProvider;
                if (provider == null) return;

                LabelTextFormatting = provider.LabelTextFormatting;
                LabelHorizontalAnchorPoint = provider.LabelHorizontalAnchorPoint;
                LabelVerticalAnchorPoint = provider.LabelVerticalAnchorPoint;
                SkipMode = provider.SkipMode;
                SkipNumber = provider.SkipNumber;
                PointCountThreshold = provider.PointCountThreshold;
                PaddingLeft = provider.LabelPadding.Left;
                PaddingTop = provider.LabelPadding.Top;
                PaddingRight = provider.LabelPadding.Right;
                PaddingBottom = provider.LabelPadding.Bottom;
                LabelFontSize = _selectedSeries.FontSize;

                if (_selectedSeries.Foreground is SolidColorBrush brush)
                {
                    LabelColor = MatchNamedColor(brush.Color);
                }

                IsBarSeries = provider is BarDataLabelProvider;
                if (provider is BarDataLabelProvider barProvider)
                {
                    BarAnchorPoint = barProvider.BarAnchorPoint;
                }
            }
            finally
            {
                _suppressApply = false;
            }
        }

        private void Apply()
        {
            if (_suppressApply) return;

            var labelSeries = _selectedSeries as IDataLabelProviderSeries;
            if (labelSeries == null) return;

            // Suspend updates so the batch of property changes triggers a single redraw on resume
            using ((ParentSurface as ISuspendable)?.SuspendUpdates())
            {
                labelSeries.ShowDataLabels = ShowDataLabels;

                var provider = labelSeries.DataLabelProvider as PointDataLabelProvider;
                if (provider != null)
                {
                    provider.LabelTextFormatting = LabelTextFormatting;
                    provider.LabelHorizontalAnchorPoint = LabelHorizontalAnchorPoint;
                    provider.LabelVerticalAnchorPoint = LabelVerticalAnchorPoint;
                    provider.SkipMode = SkipMode;
                    provider.SkipNumber = (int)SkipNumber;
                    provider.PointCountThreshold = (int)PointCountThreshold;
                    provider.LabelPadding = new Thickness(PaddingLeft, PaddingTop, PaddingRight, PaddingBottom);

                    _selectedSeries.FontSize = LabelFontSize;
                    if (!string.IsNullOrEmpty(LabelColor))
                    {
                        _selectedSeries.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(LabelColor));
                    }

                    if (provider is BarDataLabelProvider barProvider)
                    {
                        barProvider.BarAnchorPoint = BarAnchorPoint;
                    }
                }
            }
        }

        private static string MatchNamedColor(Color color)
        {
            foreach (var name in NamedColors)
            {
                if ((Color)ColorConverter.ConvertFromString(name) == color)
                {
                    return name;
                }
            }

            return null;
        }
    }
}
