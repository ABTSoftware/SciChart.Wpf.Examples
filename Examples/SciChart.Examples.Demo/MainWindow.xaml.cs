using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ServiceLocator.Container.Resolve<Bootstrapper>().WhenInit += (s, e) =>
            {
                Action operation = () => { DataContext = ServiceLocator.Container.Resolve<IMainWindowViewModel>(); };
                Dispatcher.BeginInvoke(operation);
            };

            // Maximise a window that is too large for the screen 
            if (Width > SystemParameters.WorkArea.Width || Height > SystemParameters.WorkArea.Height)
            {
                WindowState = WindowState.Maximized;                
            }

            // Always topmost if /quickstart mode used by UIAutomationTests
            if (App.UIAutomationTestMode)
            {
                Topmost = true;
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", "https://www.scichart.com/scichart-wpf-v6-the-worlds-fastest-wpf-charts/");
        }
    }
}