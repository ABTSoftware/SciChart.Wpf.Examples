// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// HomeView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows.Controls;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo.Views
{
    public partial class HomeView : UserControl
    {
        private const double OffsetFactor = 250;

        public HomeView()
        {
            InitializeComponent();

            DataContext = ServiceLocator.Container.Resolve<IMainWindowViewModel>();
        }

        private void EverythingScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (e.Delta / 120) * OffsetFactor);
                e.Handled = true;
            }
        }
    }
}