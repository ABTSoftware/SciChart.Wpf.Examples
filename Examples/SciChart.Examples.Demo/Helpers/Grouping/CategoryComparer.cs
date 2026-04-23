// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CategoryComparer.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciChart.Examples.Demo.Helpers.Grouping
{
    public class CategoryComparer: IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null) return -1;
            if (y == null) return 1;

            var invX = x.ToUpperInvariant();
            var invY = y.ToUpperInvariant();
            var xPriority = GetPriority(invX);
            var yPriority = GetPriority(invY);
            var result = xPriority.CompareTo(yPriority);
            if (result == 0) result = String.Compare(invX, invY, StringComparison.InvariantCulture);

            return result;
        }

        private int GetPriority(string category)
        {
            // Custom examples order for Featured Apps, to show most prominent examples first
            if (category.Contains("FEATURED")) return 1;
            if (category.Contains("2D CHARTS")) return 2;
            if (category.Contains("PERFORMANCE")) return 3;
            if (category.Contains("SCIENTIFIC")) return 4;
            if (category.Contains("FINANCIAL")) return 5;
            return 6;
        }
    }
}
