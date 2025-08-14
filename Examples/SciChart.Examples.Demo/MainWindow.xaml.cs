using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
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
                if (TryGetPrimaryScreenSize(out double screenWidth, out double screenHeight))
                {
                    double windowWidth, windowHeight;

                    // Landscape
                    if (screenWidth > screenHeight)
                    {
                        const double DefaultAspectRatioX = 16;
                        const double DefaultAspectRatioY = 9;

                        var aspectRatioUnit = screenHeight * DefaultSizeFactor / DefaultAspectRatioY;

                        windowWidth = aspectRatioUnit * DefaultAspectRatioX;
                        windowHeight = screenHeight * DefaultSizeFactor;
                    }
                    // Portrait
                    else
                    {
                        const double DefaultAspectRatioX = 4;
                        const double DefaultAspectRatioY = 3;

                        var aspectRatioUnit = screenWidth * DefaultSizeFactor / DefaultAspectRatioX;

                        windowWidth = screenWidth * DefaultSizeFactor;
                        windowHeight = aspectRatioUnit * DefaultAspectRatioY;
                    }

                    Width = windowWidth;
                    Height = windowHeight;

                    WindowStartupLocation = WindowStartupLocation.Manual;

                    if (windowWidth > screenWidth || windowHeight > screenHeight)
                    {
                        // We cannot calculate the position because the window is larger than the display.
                        // We set the default position to 10% of the display size and maximize the window.
                        Left = screenWidth * 0.1;
                        Top = screenHeight * 0.1;

                        WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        Left = (screenWidth - windowWidth) / 2;
                        Top = (screenHeight - windowHeight) / 2;
                    }
                }
                else
                {
                    // We cannot calculate the size and position because we are unable to get the display bounds.
                    // We set the default size to show 3 columns of example tiles with an aspect ratio of 16x9.
                    Width = 1300;
                    Height = 730;

                    WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
        }

        private bool TryGetPrimaryScreenSize(out double width, out double height)
        {
            try
            {
                // The screen size (device-independent units) of the primary display.
                width = SystemParameters.PrimaryScreenWidth;
                height = SystemParameters.PrimaryScreenHeight;

                return true;
            }
            catch (Exception ex)
            {
                App.Log.Error("SystemParameters: An error occurred while getting the screen size", ex);
            }

            try
            {
                // The screen size (physical pixels) of the primary display.
                var screenWidth = Screen.PrimaryScreen.Bounds.Width;
                var screenHeight = Screen.PrimaryScreen.Bounds.Height;

                // The DPI scaling factor of the primary display.
                var screenScale = VisualTreeHelper.GetDpi(this);

                width = screenWidth / screenScale.DpiScaleX;
                height = screenHeight / screenScale.DpiScaleY;

                return true;
            }
            catch (Exception ex)
            {
                App.Log.Error("Screen: An error occurred while getting the screen size", ex);
            }

            width = double.NaN;
            height = double.NaN;

            return false;
        }
    }
}