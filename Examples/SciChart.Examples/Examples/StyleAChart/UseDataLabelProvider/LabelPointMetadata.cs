// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// LabelPointMetadata.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.ComponentModel;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.StyleAChart.UseDataLabelProvider
{
    public class LabelPointMetadata : IPointMetadata
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LabelPointMetadata(string label)
        {
            Label = label;
        }

        public bool IsSelected { get; set; }

        public string Label { get; }
    }
}
