// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DashboardDataHelper.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
namespace SciChart.Examples.Examples.CreateMultiseriesChart.DashboardStyleCharts
{
    public static class DashboardDataHelper
    {
        private static readonly double[] XValues = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};

        private static readonly double[][] YValues =
        {
            new double[] {10, 13, 7, 16, 4, 6, 20, 14, 16, 10, 24, 11},
            new double[] {12, 17, 21, 15, 19, 18, 13, 21, 22, 20, 5, 10},
            new double[] {7, 30, 27, 24, 21, 15, 17, 26, 22, 28, 21, 22},
            new double[] {16, 10, 9, 8, 22, 14, 12, 27, 25, 23, 17, 17},
            new double[] {7, 24, 21, 11, 19, 17, 14, 27, 26, 22, 28, 16}
        };

        private static readonly string[] ColumnStyleKeys =
        {
            "StackedColumnFirstColorStyle",
            "StackedColumnSecondColorStyle",
            "StackedColumnThirdColorStyle",
            "StackedColumnFourthColorStyle",
            "StackedColumnFifthColorStyle"
        };

        private static readonly string[] MountainStyleKeys =
        {
            "StackedMountainFirstColorStyle",
            "StackedMountainSecondColorStyle",
            "StackedMountainThirdColorStyle",
            "StackedMountainFourthColorStyle",
            "StackedMountainFifthColorStyle"
        };

        public static double[] GetXValues()
        {
            return XValues;
        }

        public static double[][] GetYValues()
        {
            return YValues;
        }

        public static string[] GetColumnStyleKeys()
        {
            return ColumnStyleKeys;
        }

        public static string[] GetMountainStyleKeys()
        {
            return MountainStyleKeys;
        }
    }
}