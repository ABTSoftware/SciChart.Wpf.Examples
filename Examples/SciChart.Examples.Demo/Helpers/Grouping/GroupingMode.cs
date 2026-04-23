// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// GroupingMode.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.ComponentModel;

namespace SciChart.Examples.Demo.Helpers.Grouping
{
    public enum GroupingMode
    {
        [Description("Feature")]
        Feature,      
        
        [Description("Name")]
        Name,

        [Description("Category")]
        Category,

        [Description("Date Released")]
        DateReleased,

        [Description("Most Used")]
        MostUsed,
    }
}