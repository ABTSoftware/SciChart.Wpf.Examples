// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SeriesType.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.ManipulateSeriesMVVM
{
    public class SeriesType
    {
        public SeriesType(Type type, string iconPath)
        {
            Type = type;
            IconPath = iconPath;
        }

        public string IconPath { get; set; }
        public Type Type { get; set; }
    }
}