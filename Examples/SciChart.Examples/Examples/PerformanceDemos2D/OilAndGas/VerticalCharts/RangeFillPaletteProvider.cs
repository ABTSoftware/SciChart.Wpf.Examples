// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// RangeFillPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.VerticalCharts
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

    public class FillMetadata : IPointMetadata
    {
        private bool _isSelected;

        public Brush FillBrush { get; set; }

        public FillMetadata(Brush fillBrush)
        {
            FillBrush = fillBrush;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RangeFillPaletteProvider : IFillPaletteProvider
    {
        public IList<PaletteRange> PaletteRanges { get; }

        public RangeFillPaletteProvider(IEnumerable<PaletteRange> paletteRanges)
        {
            PaletteRanges = paletteRanges.OrderBy(x => x.StartIndex).ToList();
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }

        public IPointMetadata GetMetadataByIndex(int index)
        {
            for (int i = 0; i < PaletteRanges.Count; i++)
            {
                var range = PaletteRanges[i];

                if (index >= range.StartIndex && index <= range.EndIndex)
                {
                    return new FillMetadata(range.FillBrush);
                }
            }
            return null;
        }

        public Brush OverrideFillBrush(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            if (metadata is FillMetadata fillMetadata)
            {
                return fillMetadata.FillBrush;
            }
            return null;
        }
    }
}