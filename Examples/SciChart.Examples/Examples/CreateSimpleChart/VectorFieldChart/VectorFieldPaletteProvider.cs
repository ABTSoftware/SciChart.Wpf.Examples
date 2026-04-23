// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// VectorFieldPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.ComponentModel;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Minimal <see cref="IPointMetadata"/> implementation for vector field data points.
    /// Carries the <see cref="IsSelected"/> flag written by
    /// <see cref="SciChart.Charting.ChartModifiers.DataPointSelectionModifier"/> and read by
    /// <see cref="VectorSelectionPaletteProvider"/>.
    /// </summary>
    public sealed class VectorPointMetadata : IPointMetadata
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// Returns a bright selection color (<see cref="Colors.OrangeRed"/>) for any vector whose
    /// metadata has <see cref="IPointMetadata.IsSelected"/> set, and <c>null</c> otherwise
    /// (preserving the series default or colormap color).
    /// </summary>
    public sealed class VectorSelectionPaletteProvider : IStrokePaletteProvider
    {
        private static readonly Color SelectionColor = Colors.White;

        public void OnBeginSeriesDraw(IRenderableSeries rSeries) { }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            return metadata?.IsSelected == true ? SelectionColor : (Color?)null;
        }
    }
}
