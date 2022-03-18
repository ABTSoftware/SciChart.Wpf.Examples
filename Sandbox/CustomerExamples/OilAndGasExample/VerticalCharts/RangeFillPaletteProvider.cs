using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace OilAndGasExample.VerticalCharts
{
    public class PaletteRange
    {
        public int StartIndex { get; }

        public int EndIndex { get; }

        public Brush FillBrush { get; }

        public PaletteRange(int startIndex, int endIndex, Brush fillBrush)
        {
            if (startIndex > endIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            StartIndex = startIndex;
            EndIndex = endIndex;
            FillBrush = fillBrush;
        }
    }

    public class RangeFillPaletteProvider : IFillPaletteProvider
    {
        private Brush _defaultBrush;

        private int _rangeIndex;

        public IList<PaletteRange> PaletteRanges { get; }

        public RangeFillPaletteProvider(IEnumerable<PaletteRange> paletteRanges)
        {
            PaletteRanges = paletteRanges.OrderBy(x => x.StartIndex).ToList();
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            _rangeIndex = 0;

            if (rSeries is StackedMountainRenderableSeries stackedMountainSeries)
            {
                _defaultBrush = stackedMountainSeries.Fill;
            }
        }

        public Brush OverrideFillBrush(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var range = PaletteRanges[_rangeIndex];

            if (index > range.StartIndex && index > range.EndIndex)
            {
                _rangeIndex = Math.Min(_rangeIndex + 1, PaletteRanges.Count - 1);

                range = PaletteRanges[_rangeIndex];
            }

            if (index >= range.StartIndex && index <= range.EndIndex)
            {
                return range.FillBrush;
            }
            return _defaultBrush;
        }
    }
}