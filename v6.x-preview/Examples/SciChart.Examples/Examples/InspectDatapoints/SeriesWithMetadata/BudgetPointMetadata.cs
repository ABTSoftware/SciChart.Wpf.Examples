// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BudgetPointMetadata.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Text;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.InspectDatapoints.SeriesWithMetadata
{
    public class BudgetPointMetadata : IPointMetadata
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BudgetPointMetadata(double gainLossValue)
        {
            GainLossValue = gainLossValue;
        }

        public bool IsSelected { get; set; }

        public bool IsCheckPoint { get; set; }

        public double GainLossValue { get; set; }

        public string CEO { get; set; }
    }
}
