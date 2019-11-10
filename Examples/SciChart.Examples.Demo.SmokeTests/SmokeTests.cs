using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using NUnit.Framework;

namespace SciChart.Examples.Demo.SmokeTests
{
    [TestFixture]
    [Category("UIAutomationTests")]
    public class SmokeTests : AutomationTestBase
    {
        private Application _theApp;
        private UIA3Automation _automation;
        private Window _mainWindow;
        private Stopwatch _stopwatch;
        private const int BigWaitTimeout = 3000;
        private const int SmallWaitTimeout = 1000;
        const double DefaultTolerance = 0.5;
        private const bool ExportActualForTest = false;

        [SetUp]
        public void Setup()
        {
            _theApp = FlaUI.Core.Application.Launch(new ProcessStartInfo("SciChart.Examples.Demo.exe", "/quickStart"));
            _automation = new UIA3Automation();
            _mainWindow = _theApp.GetMainWindow(_automation);
            _stopwatch = Stopwatch.StartNew();

            // TODO: click AutomationProperties.AutomationId="ShellControl.Home"
            // ShellControl.ShowSettings
            // Breadcrumb.Home
        }

        [TearDown]
        public void Teardown()
        {
            _automation?.Dispose();
            _theApp?.Close();
            _stopwatch.Stop();
            Console.WriteLine("Time elapsed: " + _stopwatch.ElapsedMilliseconds);
        }

        [Test]
        [TestCase("Band Series Chart", "Charts2D/CreateSimpleChart/BandSeriesChart.png", DefaultTolerance)]
        [TestCase("Box Plot", "Charts2D/CreateSimpleChart/BoxPlot.png", DefaultTolerance)]
        [TestCase("Bubble Chart", "Charts2D/CreateSimpleChart/BubbleChart.png", DefaultTolerance)]
        [TestCase("Candlestick Chart", "Charts2D/CreateSimpleChart/CandlestickChart.png", DefaultTolerance)]
        [TestCase("Digital Band Series Chart", "Charts2D/CreateSimpleChart/DigitalBandSeriesChart.png", DefaultTolerance)]
        [TestCase("Digital Line Chart", "Charts2D/CreateSimpleChart/DigitalLineChart.png", DefaultTolerance)]
        [TestCase("Impulse (Stem) Chart", "Charts2D/CreateSimpleChart/ImpulseStemChart.png", DefaultTolerance)]
        [TestCase("Line Chart", "Charts2D/CreateSimpleChart/LineChart.png", DefaultTolerance)]
        [TestCase("Mountain Chart", "Charts2D/CreateSimpleChart/MountainChart.png", DefaultTolerance)]
        [TestCase("Polar Chart", "Charts2D/CreateSimpleChart/PolarChart.png", DefaultTolerance)]
        [TestCase("Scatter Chart", "Charts2D/CreateSimpleChart/ScatterChart.png", DefaultTolerance)]
        public void AssertExampleStarts(string exampleName, string resourceName, double tolerance)
        {
            // Get the example button and click it 
            var examplesWrapPanel = WaitForElement(() => _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ExamplesWrapPanel")));
            var exampleButton = WaitForElement(() => examplesWrapPanel?.FindFirstDescendant(cf => cf.ByAutomationId(exampleName)).AsButton());
            exampleButton?.WaitUntilClickable();
            exampleButton?.Invoke();

            // Click the 'Got it!' help button
            var tipsView = WaitForElement(() => _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("TipsView")));
            var gotItButton = WaitForElement(() => tipsView.FindFirstDescendant(cf => cf.ByAutomationId("TipsView.GotItButton"))?.AsButton());
            gotItButton?.WaitUntilClickable();
            gotItButton?.Invoke();

            WaitUntilClosed(tipsView);

            var exampleView = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ExampleView.TransitioningFrame"));
            if (exampleView == null)
            {
                Assert.Fail("Unable to get ExampleView");
            }

            // Capture a screenshot & compare
            using (var capture = Capture.Element(exampleView))
            {
                var actualBitmap = new WriteableBitmap(capture.BitmapImage);

#if DEBUG
                // When true, we export the image and open in Paint for test purposes. 
                // Save this image in resources, as embedded resource, then set flag exportActualForTest=false for the actual test
                if (ExportActualForTest)
                {
                    var pathString = Path.Combine(ExportActualPath, resourceName);
                    base.SaveToPng(pathString, actualBitmap);

                    // Export the actual 
                    ProcessStartInfo startInfo = new ProcessStartInfo(pathString);
                    startInfo.Verb = "edit";
                    Process.Start(startInfo);
                }
#endif

                WriteableBitmap expectedBitmap = null;
                try
                {
                    expectedBitmap = this.LoadResource(resourceName);
                }
                catch (Exception caught)
                {
                    throw new Exception("Unable to load image from resource " + resourceName, caught);
                }
                Assert.True(CompareBitmaps(resourceName, actualBitmap, expectedBitmap, tolerance));
            }
        }

        private void WaitUntilClosed(AutomationElement element)
        {
            var result = Retry.WhileFalse(() => element.IsOffscreen, TimeSpan.FromMilliseconds(BigWaitTimeout));
            if (!result.Success)
            {
                Assert.Fail($"Element failed to go offscreen within {BigWaitTimeout}ms" );
            }
        }

        private T WaitForElement<T>(Func<T> getter)
        {
            var retry = Retry.WhileNull<T>(
                () => getter(),
                TimeSpan.FromMilliseconds(BigWaitTimeout));

            if (!retry.Success)
            {
                Assert.Fail($"Failed to get an element within a {BigWaitTimeout}ms");
            }

            return retry.Result;
        }
    }
}