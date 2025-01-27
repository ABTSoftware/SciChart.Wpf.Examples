// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FanChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    /// <summary>
    /// Interaction logic for FanChartExampleView.xaml
    /// </summary>
    public partial class FanChartExampleView : UserControl
    {
        public FanChartExampleView()
        {
            InitializeComponent();

            this.Loaded += FanChartExampleViewLoaded;
        }

        private void FanChartExampleViewLoaded(object sender, RoutedEventArgs e)
        {
            // Variance data is a 2D table containing actual values and several levels of projected high, low values
            IEnumerable<VarPoint> varianceData = GetVarianceData().ToArray();

            // To render the Fan, we use an XyDataSeries and three band series'
            var actualDataSeries = new XyDataSeries<DateTime, double>();
            var var3DataSeries = new XyyDataSeries<DateTime, double>();
            var var2DataSeries = new XyyDataSeries<DateTime, double>();
            var var1DataSeries = new XyyDataSeries<DateTime, double>();

            // Append data values from the Variance table
            actualDataSeries.Append(varianceData.Select(x => x.Date), varianceData.Select(x => x.Actual));
            var3DataSeries.Append(varianceData.Select(x => x.Date), varianceData.Select(x => x.VarMin), varianceData.Select(x => x.VarMax));
            var2DataSeries.Append(varianceData.Select(x => x.Date), varianceData.Select(x => x.Var1), varianceData.Select(x => x.Var4));
            var1DataSeries.Append(varianceData.Select(x => x.Date), varianceData.Select(x => x.Var2), varianceData.Select(x => x.Var3));

            // Assign data to renderableseries
			lineSeries.DataSeries = actualDataSeries;
			projectedVar3.DataSeries = var3DataSeries;
			projectedVar2.DataSeries = var2DataSeries;
            projectedVar1.DataSeries = var1DataSeries;

            sciChart.ZoomExtents();
        }


        // Create a table of Variance data. Each row in the table consists of
        // 
        //  DateTime, Actual (Y-Value), Projected Min, Variance 1, 2, 3, 4 and Projected Maximum
        // 
        //        DateTime    Actual 	Min     Var1	Var2	Var3	Var4	Max
        //        Jan-11	  y0	    -	    -	    -	    -	    -	    -
        //        Feb-11	  y1	    -	    -	    -	    -	    -	    -
        //        Mar-11	  y2	    -	    -	    -	    -	    -	    -
        //        Apr-11	  y3	    -	    -	    -	    -	    -	    -
        //        May-11	  y4	    -	    -	    -	    -	    -	    -
        //        Jun-11	  y5        min0  var1_0  var2_0  var3_0  var4_0  max_0
        //        Jul-11	  y6        min1  var1_1  var2_1  var3_1  var4_1  max_1
        //        Aug-11	  y7        min2  var1_2  var2_2  var3_2  var4_2  max_2
        //        Dec-11	  y8        min3  var1_3  var2_3  var3_3  var4_3  max_3
        //        Jan-12      y9        min4  var1_4  var2_4  var3_4  var4_4  max_4

        private IEnumerable<VarPoint> GetVarianceData()
        {
            var dates = Enumerable.Range(0, 10).Select(i => new DateTime(2011, 01, 01).AddMonths(i)).ToArray();
            var yValues = new RandomWalkGenerator(seed: 0).GetRandomWalkSeries(10).YData;

            for (int i = 0; i < 10; i++)
            {
                double varMax = double.NaN;
                double var4 = double.NaN;
                double var3 = double.NaN;
                double var2 = double.NaN;
                double var1 = double.NaN;
                double varMin = double.NaN;

                if (i > 4)
                {
                    varMax = yValues[i] + (i - 5) * 0.3;
                    var4 = yValues[i] + (i - 5) * 0.2;
                    var3 = yValues[i] + (i - 5) * 0.1;
                    var2 = yValues[i] - (i - 5) * 0.1;
                    var1 = yValues[i] - (i - 5) * 0.2;
                    varMin = yValues[i] - (i - 5) * 0.3;
                }

                yield return new VarPoint(dates[i], yValues[i], var4, var3, var2, var1, varMin, varMax);
            }
        }

        private struct VarPoint
        {
            public readonly DateTime Date;
            public readonly double Actual;
            public readonly double VarMax;
            public readonly double Var4;
            public readonly double Var3;
            public readonly double Var2;
            public readonly double Var1;
            public readonly double VarMin;

            public VarPoint(DateTime date, double actual, double var4, double var3, double var2, double var1, double varMin, double varMax) : this()
            {
                Date = date;
                Actual = actual;
                Var4 = var4;
                Var3 = var3;
                Var2 = var2;
                Var1 = var1;
                VarMin = varMin;
                VarMax = varMax;
            }
        }
    }
}
