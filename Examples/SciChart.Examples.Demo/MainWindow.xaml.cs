using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Unity;
using SciChart.UI.Bootstrap;

namespace SciChart.Examples.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ServiceLocator.Container.Resolve<Bootstrapper>().WhenInit += (s, e) =>
            {
                Action op = () => { DataContext = ServiceLocator.Container.Resolve<IMainWindowViewModel>(); };
                Dispatcher.BeginInvoke(op);
            };

            // Maximise a window that is too large for the screen 
            if (this.Width > SystemParameters.WorkArea.Width || this.Height > SystemParameters.WorkArea.Height)
            {
                this.WindowState = WindowState.Maximized;                
            }

            // Always topmost if /quickstart mode used by UIAutomationTests
            if (App.QuickStart)
            {
                this.Topmost = true;
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", "https://www.scichart.com/scichart-wpf-v6-the-worlds-fastest-wpf-charts/");
        }
    }
}
