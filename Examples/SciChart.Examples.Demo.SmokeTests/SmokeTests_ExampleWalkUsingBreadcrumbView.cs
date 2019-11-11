using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.UIA3;
using FlaUI.UIA3.Patterns;
using NUnit.Framework;

namespace SciChart.Examples.Demo.SmokeTests
{
    /// <summary>
    /// Smoke tests which start/stop the application once per ntire fixture. In this test fixture we create a new SciChart.Examples.Demo application
    /// once, then walk through all the examples by clicking programmatically the Examples Breadcrumb View.
    ///
    /// These tests are much faster, however one failure mid-way through the test run will cause all subsequent examples to fail.
    ///
    /// We pass the argument /quickStart to the SciChart.Examples.Demo.exe. This sets the flag App.QuickStart = true, which
    /// disables startup delays, series animations and usage service HTTP comms. This makes the application faster to get in and less
    /// waiting for transitions to complete 
    /// </summary>
    [TestFixture]
    public class SmokeTests_ExampleWalkUsingBreadcrumbView : AutomationTestBase
    {
        private Application _theApp;
        private UIA3Automation _automation;
        private Window _mainWindow;
        private Stopwatch _stopwatch;
        private const int BigWaitTimeout = 3000;
        private const int SmallWaitTimeout = 1000;
        const double DefaultTolerance = 0.5;
        private const bool ExportActualForTest = false;

        // Top level example categories
        private const string Category_2DCharts = "2D Charts";
        private const string Category_3DCharts = "3D Charts";
        private const string Category_FeaturedApps = "Featured Apps";

        // 2D Chart Example Groups
        private const string Group_2D_CreateSimpleCharts = "Basic Chart Types";
        private const string Group_2D_Annotations = "Chart Annotations";

        // 3D Chart Example Groups
        private const string Group_3D_BasicChartTypes = "Basic Chart Types";
        private const string Group_3D_SurfaceMesh = "Create A Surface Mesh Chart";

        // Todo ... more groups...

        public SmokeTests_ExampleWalkUsingBreadcrumbView()
        {
            _theApp = FlaUI.Core.Application.Launch(new ProcessStartInfo("SciChart.Examples.Demo.exe", "/quickStart"));
            _automation = new UIA3Automation();
            _mainWindow = _theApp.GetMainWindow(_automation);

            // Get any example button and click it 
            var examplesWrapPanel = WaitForElement(() => _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ExamplesWrapPanel")));
            var exampleButton = WaitForElement(() => examplesWrapPanel?.FindFirstDescendant(cf => cf.ByAutomationId("Band Series Chart")).AsButton());
            exampleButton?.WaitUntilClickable();
            exampleButton?.Invoke();

            // Click the 'Got it!' help button
            var tipsView = WaitForElement(() => _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("TipsView")));
            var gotItButton = WaitForElement(() => tipsView.FindFirstDescendant(cf => cf.ByAutomationId("TipsView.GotItButton"))?.AsButton());
            gotItButton?.WaitUntilClickable();
            gotItButton?.Invoke();

            // Now application state should be in the example view
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            // Kill the app 
            _automation?.Dispose();
            _theApp?.Close();
        }

        [SetUp]
        public void Setup()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        [TearDown]
        public void TearDown()
        {
            // report time elapsed per test
            _stopwatch.Stop();
            Console.WriteLine("Time elapsed: " + _stopwatch.ElapsedMilliseconds);
        }

        [Test]
        // 2D Charts, Create Simple Charts
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Band Series Chart", "Charts2D/CreateSimpleChart/BandSeriesChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Box Plot", "Charts2D/CreateSimpleChart/BoxPlot.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Bubble Chart", "Charts2D/CreateSimpleChart/BubbleChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Candlestick Chart", "Charts2D/CreateSimpleChart/CandlestickChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Digital Band Series Chart", "Charts2D/CreateSimpleChart/DigitalBandSeriesChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Digital Line Chart", "Charts2D/CreateSimpleChart/DigitalLineChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Impulse (Stem) Chart", "Charts2D/CreateSimpleChart/ImpulseStemChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Line Chart", "Charts2D/CreateSimpleChart/LineChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Mountain Chart", "Charts2D/CreateSimpleChart/MountainChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Polar Chart", "Charts2D/CreateSimpleChart/PolarChart.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Scatter Chart", "Charts2D/CreateSimpleChart/ScatterChart.png", DefaultTolerance)]
        // 2D Charts, Annotations
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Annotations are Easy!", "Charts2D/ChartAnnotations/AnnotationsAreEasy.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Composite Annotations", "Charts2D/ChartAnnotations/CompositeAnnotations.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Create Annotations Dynamically", "Charts2D/ChartAnnotations/CreateAnnotationsDynamically.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Datapoint Markers", "Charts2D/ChartAnnotations/DatapointMarkers.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Drag Horizontal Threshold", "Charts2D/ChartAnnotations/DragHorizontalThreshold.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Interaction with Annotations", "Charts2D/ChartAnnotations/InteractionWithAnnotations.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Polar Chart Annotations", "Charts2D/ChartAnnotations/PolarChartAnnotations.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Trade Annotations", "Charts2D/ChartAnnotations/TradeAnnotations.png", DefaultTolerance)]
        [TestCase(Category_2DCharts, Group_2D_Annotations, "Trade Markers", "Charts2D/ChartAnnotations/TradeMarkers.png", DefaultTolerance)]
        // 3D Charts, Create Simple Charts 
        [TestCase(Category_3DCharts, Group_3D_BasicChartTypes, "Closed Mesh 3D Chart", "Charts3D/BasicChartTypes/ClosedSurfaceMesh3D.png", DefaultTolerance)]
        [TestCase(Category_3DCharts, Group_3D_BasicChartTypes, "Simple Bubble 3D Chart", "Charts3D/BasicChartTypes/SimpleBubble3DChart.png", DefaultTolerance)]
        [TestCase(Category_3DCharts, Group_3D_BasicChartTypes, "Simple Cylindroid 3D Chart", "Charts3D/BasicChartTypes/SimpleCylindroid3DChart.png", DefaultTolerance)]
        [TestCase(Category_3DCharts, Group_3D_BasicChartTypes, "Simple Ellipsoid 3D Chart", "Charts3D/BasicChartTypes/SimpleEllipsoid3DChart.png", DefaultTolerance)]
        // 3D Charts, Surface Mesh Charts
        [TestCase(Category_3DCharts, Group_3D_SurfaceMesh, "Surface Mesh 3D Non-Uniform Data", "Charts3D/SurfaceMesh/NonUniformSurfaceMesh.png", DefaultTolerance)]
        public void FastAssertExampleStarts(string category, string group, string exampleName, string resourceName,
            double tolerance)
        {
            // Click breadcrumb home
            var breadcrumbHome = WaitForElement(() => _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("Breadcrumb.Home")))?.AsButton();
            breadcrumbHome?.Invoke();

            // wait for navigation popup
            var exampleNavView = WaitForElement(() => _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ExampleNavigationView")));
            if (exampleNavView == null)
            {
                Assert.Fail("Unable to get exampleNavView");
            }

            // Select the category, group and example 
            var categoryListBox = exampleNavView.FindFirstDescendant(cf => cf.ByAutomationId("CategoryListBox")).AsListBox();
            var groupListBox = exampleNavView.FindFirstDescendant(cf => cf.ByAutomationId("GroupsListBox")).AsListBox();
            var exampleListBox = exampleNavView.FindFirstDescendant(cf => cf.ByAutomationId("ExamplesListBox")).AsListBox();

            // 1. select category 
            var categoryItem = categoryListBox.FindFirstChild(category).AsListBoxItem();
            categoryItem.Select();

            // 2. select group 
            var groupListBoxItem = WaitForElement(() => groupListBox.FindFirstChild(group).AsListBoxItem());
            groupListBoxItem.Select();

            // 3. select item 
            var exampleListBoxItem = WaitForElement(() => exampleListBox.FindFirstChild(exampleName).AsListBoxItem());
            if (exampleListBoxItem.IsSelected)
            {
                // already selected? Close the example navigation view 
                exampleNavView.FindFirstDescendant("ExampleNavigationView.CloseButton").AsButton().Invoke();
            }
            else
            {
                // else select it 
                exampleListBoxItem.Select();
            }

            // Wait for nav view to close
            WaitUntilClosed(exampleNavView);

            var exampleView = _mainWindow.FindFirstDescendant("ExampleView.TransitioningFrame");
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
    }
}
