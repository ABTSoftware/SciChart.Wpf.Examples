// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SettingsView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Examples.Demo.ViewModels;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            IsVisibleChanged += OnIsVisibleChanged;

            ServiceLocator.Container.Resolve<Bootstrapper>().WhenInit += (s, e) =>
            {
                Action operation = () =>
                {
                    var settingsViewModel = ServiceLocator.Container.Resolve<ISettingsViewModel>();
                    settingsViewModel.ParentViewModel = ServiceLocator.Container.Resolve<IMainWindowViewModel>();
                    DataContext = settingsViewModel;
                };

                Dispatcher.BeginInvoke(operation);
            };
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ISettingsViewModel viewModel)
            {
                viewModel.OnIsVisibleChanged((bool)e.NewValue);
            }
        }
    }
}
