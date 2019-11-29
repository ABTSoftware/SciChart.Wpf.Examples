// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ManipulateSeriesMvvmView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Controls;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.ManipulateSeriesMVVM
{
    /// <summary>
    /// Interaction logic for ManipulateSeriesMvvmView.xaml
    /// </summary>
    public partial class ManipulateSeriesMvvmView : UserControl
    {
        public ManipulateSeriesMvvmView()
        {
            InitializeComponent();
        }

        private void SeriesSelectionModifier_OnSelectionChanged(object sender, EventArgs e)
        {
            ((ManipulateSeriesMvvmViewModel) DataContext).SelectionChangedCommand.Execute(null);
        }
    }
}
