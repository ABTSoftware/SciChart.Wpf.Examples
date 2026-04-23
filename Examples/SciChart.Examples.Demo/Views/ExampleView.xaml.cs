// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using SciChart.Examples.Demo.ViewModels;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo.Views
{
    public partial class ExampleView
    {
        public ExampleView()
        {
            InitializeComponent();

            DataContext = ServiceLocator.Container.Resolve<IExampleViewModel>();

            BreadcrumbWrapper.SizeChanged += (s, e) =>
            {
                if (e.WidthChanged)
                {
                    if (BreadcrumbWrapper.ActualWidth <= 120)
                    {
                        BreadcrumbChain.Visibility = Visibility.Collapsed;
                        BreadcrumbButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        BreadcrumbChain.Visibility = Visibility.Visible;
                        BreadcrumbButton.Visibility = Visibility.Collapsed;
                    }
                }
            };
        }

        private void DescriptionBox_OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}
