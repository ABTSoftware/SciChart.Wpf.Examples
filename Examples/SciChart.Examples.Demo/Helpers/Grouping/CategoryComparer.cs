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
