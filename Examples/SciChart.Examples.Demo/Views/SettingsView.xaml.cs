using System;
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
    }
}
