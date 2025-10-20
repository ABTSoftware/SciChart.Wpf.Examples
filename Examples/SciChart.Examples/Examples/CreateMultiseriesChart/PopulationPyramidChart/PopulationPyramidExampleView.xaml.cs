// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// PopulationPyramidExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.PopulationPyramidChart
{
    /// <summary>
    /// Interaction logic for PopulationPyramidExampleView.xaml
    /// </summary>
    public partial class PopulationPyramidExampleView
    {
        public PopulationPyramidExampleView()
        {
            InitializeComponent();

            var agesXValues = new[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100 };
            var maleAfricaYValues = new[] {35754890, 31813896, 28672207, 24967595, 20935790, 17178324, 14422055, 12271907, 10608417, 8608183,
                6579937, 5035598, 3832420, 2738448, 1769284, 1013988, 470834, 300795, 264940, 146520, 84000 };

            var femaleAfricaYValues = new[] { 34834623, 31000760, 27861135, 24206021, 20338468, 16815440, 14207659, 12167437, 10585531, 8658614,
                6721555, 5291815, 4176910, 3076943, 2039952, 1199203, 591092, 400010, 353922, 159610, 102500 };

            var dataSeriesMale = new XyDataSeries<double> { SeriesName = "Male Population" };
            var dataSeriesFemale = new XyDataSeries<double> { SeriesName = "Female Population" };

            for (int i = 0; i < maleAfricaYValues.Length; i++) dataSeriesMale.Append(agesXValues[i], maleAfricaYValues[i]);
            for (int i = 0; i < femaleAfricaYValues.Length; i++) dataSeriesFemale.Append(agesXValues[i], femaleAfricaYValues[i]);

            using (sciChart.SuspendUpdates())
            {
                MalePopulationSeries.DataSeries = dataSeriesMale;
                FemalePopulationSeries.DataSeries = dataSeriesFemale;
                PopulationAgeXAxis.LabelProvider = new AgeRangeLabelProvider();
            }

            sciChart.ZoomExtents();
        }
    }
}