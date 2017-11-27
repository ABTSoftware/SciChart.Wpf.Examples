// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateRealTimeTickingStockChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System.Windows.Controls;

namespace SciChart.Sandbox.Examples.MarketProfileTradingExample
{
    [TestCase("Market Profile Trading Example")]
    public partial class MarketProfileTradingChart : UserControl
    {
        public MarketProfileTradingChart()
        {
            InitializeComponent();
            DataContext = new HistogramBarDemoProjectViewModel();
        }
    }

}