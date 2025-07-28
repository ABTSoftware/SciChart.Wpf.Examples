using System.Windows;
using MahApps.Metro.Controls;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo
{
    public partial class MainWindow : MetroWindow
    {
        private const double DefaultSizeFactor = 0.8;

        public MainWindow()
        {
            InitializeComponent();

            ServiceLocator.Container.Resolve<Bootstrapper>().WhenInit += (s, e) =>
            {
                Dispatcher.BeginInvoke(() => DataContext = ServiceLocator.Container.Resolve<IMainWindowViewModel>());
            };

            // Always topmost if /quickstart mode used by UIAutomationTests.
            if (App.UIAutomationTestMode)
            {
                Width = 1200;
                Height = 900;

                Topmost = true;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                // The screen size, in pixels adjusted for DPI, of the primary display monitor.
                var monitorWidth = SystemParameters.PrimaryScreenWidth;
                var monitorHeight = SystemParameters.PrimaryScreenHeight;

                var windowWidth = 0d;
                var windowHeight = 0d;

                // Landscape
                if (monitorWidth > monitorHeight)
                {
                    const double DefaultAspectRatioX = 16;
                    const double DefaultAspectRatioY = 9;

                    var aspectRatioUnit = monitorHeight * DefaultSizeFactor / DefaultAspectRatioY;

                    windowWidth = aspectRatioUnit * DefaultAspectRatioX;
                    windowHeight = monitorHeight * DefaultSizeFactor;
                }
                // Portrait
                else
                {
                    const double DefaultAspectRatioX = 4;
                    const double DefaultAspectRatioY = 3;

                    var aspectRatioUnit = monitorWidth * DefaultSizeFactor / DefaultAspectRatioX;

                    windowWidth = monitorWidth * DefaultSizeFactor;
                    windowHeight = aspectRatioUnit * DefaultAspectRatioY;
                }

                Width = windowWidth;
                Height = windowHeight;

                WindowStartupLocation = WindowStartupLocation.Manual;

                if (windowWidth > monitorWidth || windowHeight > monitorHeight)
                {
                    // We cannot calculate the position because the window is larger than the display monitor size.
                    // We set the default position to 10% of the display width and height respectively and maximize the window.
                    Left = monitorWidth * 0.1;
                    Top = monitorHeight * 0.1;

                    WindowState = WindowState.Maximized;
                }
                else
                {
                    Left = (monitorWidth - windowWidth) / 2;
                    Top = (monitorHeight - windowHeight) / 2;
                }
            }
        }
    }
}