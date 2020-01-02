using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
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
        private const bool DefaultExportActualForTest = false;

        // Top level example categories
        private const string Category_2DCharts = "2D Charts";
        private const string Category_3DCharts = "3D Charts";
        private const string Category_FeaturedApps = "Featured Apps";

        // 2D Chart Example Groups
        private const string Group_2D_CreateSimpleCharts = "Basic Chart Types";
        private const string Group_2D_Annotations = "Chart Annotations";
        private const string Group_2D_Gauge = "Create a Gauge Charts";
        private const string Group_2D_MultiSeries = "Create a Multiseries Chart";
        private const string Group_2D_Radar = "Create a Radar Chart";
        private const string Group_2D_Ternary = "Create a Ternary Chart";
        private const string Group_2D_Custom = "Create Custom Charts";
        private const string Group_2D_RealtimeCharts = "Create Realtime Charts";
        private const string Group_2D_StockCharts = "Create Stock Charts";
        private const string Group_2D_ExportAChart = "Export a Chart";
        private const string Group_2D_FiltersApi = "Filters API";
        private const string Group_2D_Heatmap = "HeatmapChartTypes";
        private const string Group_2D_Legends = "Legends";
        private const string Group_2D_LinkMultipleCharts = "Link Multiple Charts";
        private const string Group_2D_ManipulateSeries = "Manipulate Series";
        private const string Group_2D_ModifyAxisBehavior = "Modify Axis Behavior";
        private const string Group_2D_MVVMExamples = "MVVM Examples";
        private const string Group_2D_StylingTheming = "Styling and Theming";
        private const string Group_2D_TooltipsAndHitTest = "Tooltips and Hit Test";
        private const string Group_2D_ZoomPan = "Zoom and Pan a Chart";
        private const string Group_2D_ZoomHistory = "Zoom History Manager";

        // 3D Chart Example Groups
        private const string Group_3D_BasicChartTypes = "Basic Chart Types";
        private const string Group_3D_SurfaceMesh = "Create A Surface Mesh Chart";

        // Featured Apps example groups
        private const string Group_Featured_PerformanceDemos = "Performance Demos";
        private const string Group_Featured_ScientificCharts = "Scientific Charts";
        private const string Group_Featured_MedicalCharts = "Medical Charts";
        private const string Group_Featured_FinancialCharts = "Financial Charts";

        // Todo ... more groups...

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _theApp = FlaUI.Core.Application.Launch(new ProcessStartInfo("SciChart.Examples.Demo.exe", "/uiautomationTestMode"));
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

            // Move mouse into top left corner of screen
            Mouse.Position = new System.Drawing.Point(0, 0);
        }

        [TearDown]
        public void TearDown()
        {
            // report time elapsed per test
            _stopwatch.Stop();
            Console.WriteLine("Time elapsed: " + _stopwatch.ElapsedMilliseconds);
        }

        public class ExampleStartTestCase
        {
            public int TestNumber { get; }
            public string Category { get; }
            public string Group { get; }
            public string Example { get; }
            public string ResourceName { get; }
            public Action<Window> BeforeFunc { get; }
            public double Tolerance { get; }
            public bool ExportActual { get; }

            public ExampleStartTestCase(string category, string group, string example, string resourceName, double tolerance = DefaultTolerance, bool exportActual = DefaultExportActualForTest)    
                : this(category, group, example, resourceName, null, tolerance, exportActual)
            {
            }

            public ExampleStartTestCase(string category, string group, string example, string resourceName, Action<Window> beforeFunc, double tolerance = DefaultTolerance, bool exportActual = DefaultExportActualForTest)
            {
                Category = category;
                Group = @group;
                Example = example;
                ResourceName = resourceName;
                BeforeFunc = beforeFunc;
                Tolerance = tolerance;
                ExportActual = exportActual;
            }

            public override string ToString()
            {
                return $"{TestNumber}, {Category}/{Group}/{Example}";
            }
        }


        public ExampleStartTestCase[] FastAssertExampleStartsTestCases = new[]
        {
            // 2D Charts, Create Simple Charts
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Band Series Chart", "Charts2D/CreateSimpleChart/BandSeriesChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Box Plot", "Charts2D/CreateSimpleChart/BoxPlot.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Bubble Chart", "Charts2D/CreateSimpleChart/BubbleChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Candlestick Chart", "Charts2D/CreateSimpleChart/CandlestickChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Column Chart", "Charts2D/CreateSimpleChart/ColumnChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Digital Band Series Chart", "Charts2D/CreateSimpleChart/DigitalBandSeriesChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Digital Line Chart", "Charts2D/CreateSimpleChart/DigitalLineChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Digital Mountain Chart", "Charts2D/CreateSimpleChart/DigitalMountainChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Impulse (Stem) Chart", "Charts2D/CreateSimpleChart/ImpulseStemChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Line Chart", "Charts2D/CreateSimpleChart/LineChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Mountain Chart", "Charts2D/CreateSimpleChart/MountainChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Polar Chart", "Charts2D/CreateSimpleChart/PolarChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_CreateSimpleCharts, "Scatter Chart", "Charts2D/CreateSimpleChart/ScatterChart.png"),
            // 2D Charts, Annotations
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Annotations are Easy!", "Charts2D/ChartAnnotations/AnnotationsAreEasy.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Composite Annotations", "Charts2D/ChartAnnotations/CompositeAnnotations.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Create Annotations Dynamically", "Charts2D/ChartAnnotations/CreateAnnotationsDynamically.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Datapoint Markers", "Charts2D/ChartAnnotations/DatapointMarkers.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Drag Horizontal Threshold", "Charts2D/ChartAnnotations/DragHorizontalThreshold.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Interaction with Annotations", "Charts2D/ChartAnnotations/InteractionWithAnnotations.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Polar Chart Annotations", "Charts2D/ChartAnnotations/PolarChartAnnotations.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Trade Annotations", "Charts2D/ChartAnnotations/TradeAnnotations.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Annotations, "Trade Markers", "Charts2D/ChartAnnotations/TradeMarkers.png"),
            // 2D Charts, Gauge Charts
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Gauge, "Using Donut Chart", "Charts2D/CreateGaugeChart/UsingDonutChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Gauge, "Using Pie Chart", "Charts2D/CreateGaugeChart/UsingPieChart.png"),
            // 2D Charts, MultiSeries Chart
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Candlestick and Lines", "Charts2D/MultiseriesCharts/CandlestickAndLines.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Contours With Heatmap Chart", "Charts2D/MultiseriesCharts/ContoursWithHeatmap.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Dashboard Style Charts", "Charts2D/MultiseriesCharts/DashboardStyleCharts.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Dashboard Style Polar Charts", "Charts2D/MultiseriesCharts/DashboardPolarCharts.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Error Bars", "Charts2D/MultiseriesCharts/ErrorBars.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Fan Chart", "Charts2D/MultiseriesCharts/FanChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Gaps In Series", "Charts2D/MultiseriesCharts/GapsInSeries.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Line and Scatter Chart", "Charts2D/MultiseriesCharts/LineAndScatterChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Stacked Bar Chart", "Charts2D/MultiseriesCharts/StackedBarChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Stacked Column Chart", "Charts2D/MultiseriesCharts/StackedColumnChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Stacked Column Side By Side", "Charts2D/MultiseriesCharts/StackedColumnSideBySide.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MultiSeries, "Stacked Mountain Chart", "Charts2D/MultiseriesCharts/StackedMountainChart.png"),
            // 2D Charts, Create a Radar Chart
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Radar, "Radar Chart Customization Example", "Charts2D/RadarCharts/RadarChartCustomization.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Radar, "Using Radar Chart", "Charts2D/RadarCharts/RadarChart.png"),
            // 2D Charts, Create a Ternary Chart
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Ternary, "ErrorBar Series TernaryChart", "Charts2D/TernaryCharts/ErrorBarTernaryChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Ternary, "Polygon Series TernaryChart", "Charts2D/TernaryCharts/PolygonTernaryChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Ternary, "Scatter Series TernaryChart", "Charts2D/TernaryCharts/ScatterTernaryChart.png"),
            // 2D Charts, Create Custom Charts
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Custom, "Spline Scatter Line Chart", "Charts2D/CustomCharts/SplineScatterChart.png"),
            // 2D Charts, Create Realtime Charts
            // TODO: add later, stop timer for some of them, use fixed data for other or leave them

            // 2D Charts, Create Stock Charts
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StockCharts, "Multi-Pane Stock Charts", "Charts2D/StockCharts/MultiPaneStockChart.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StockCharts, "Realtime Ticking Stock Charts", "Charts2D/StockCharts/RealtimeTickingCharts.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StockCharts, "Using SciStockChart", "Charts2D/StockCharts/UsingSciStockChart.png"),
            // 2D Charts, Export a Chart
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ExportAChart, "Export and Screenshot Options in Chart", "Charts2D/ExportCharts/ExportChart.png"),
            // 2D Charts, Filters API
            new ExampleStartTestCase(Category_2DCharts, Group_2D_FiltersApi, "Filters API Example", "Charts2D/Filters/FiltersApiExample.png"),
            // 2D Charts, HeatmapChartType
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Heatmap, "Heatmap Chart", "Charts2D/Heatmaps/HeatmapRealTime.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Heatmap, "Heatmap Chart with Text", "Charts2D/Heatmaps/HeatmapWithText.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Heatmap, "HeatmapMetaData", "Charts2D/Heatmaps/HeatmapMetaData.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Heatmap, "NonUniformHeatmap", "Charts2D/Heatmaps/NonUniformHeatmap.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Heatmap, "UniformHeatmap and CustomPaletteProvider", "Charts2D/Heatmaps/HeatmapAndPaletteProvider.png"),
            // 2D Charts, Legends
            new ExampleStartTestCase(Category_2DCharts, Group_2D_Legends, "Chart Legends API", "Charts2D/Legends/LegendsAPI.png"),
            // 2D Charts, Link Multiple Charts
            new ExampleStartTestCase(Category_2DCharts, Group_2D_LinkMultipleCharts, "Sync Multi Chart Mouse", "Charts2D/SyncCharts/SyncCharts.png",
                (mw) => Thread.Sleep(1500)),
            // 2D Charts, Manipulate Series:
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ManipulateSeries, "Add or Remove DataSeries In Code", "Charts2D/ManipulateSeries/AddRemoveSeries.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ManipulateSeries, "Change RenderableSeries Type In Code", "Charts2D/ManipulateSeries/ChangeRenderableSeriesType.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ManipulateSeries, "Change Series Type In Code", "Charts2D/ManipulateSeries/ChangeSeriesType.png"),
            // 2D Charts, Modify Axis Behavior
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Category vs Value Axis", "Charts2D/ModifyAxisBehavior/CategoryVsValueAxis.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Central XAxis and YAxis", "Charts2D/ModifyAxisBehavior/CentralXYAxes.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Logarithmic Axis", "Charts2D/ModifyAxisBehavior/LogarithmicAxis.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Modify Axis Properties", "Charts2D/ModifyAxisBehavior/ModifyAxisProperties.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Multiple YAxis", "Charts2D/ModifyAxisBehavior/MultipleYAxis.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Multiple-XAxis", "Charts2D/ModifyAxisBehavior/MultipleXAxis.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Polar Chart with Multiple Axis", "Charts2D/ModifyAxisBehavior/PolarChartManyAxes.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Secondary Y-Axis", "Charts2D/ModifyAxisBehavior/SecondaryYAxis.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Switch Axis Type At Runtime", "Charts2D/ModifyAxisBehavior/SwitchAxisTypeRuntime.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Vertical Charts", "Charts2D/ModifyAxisBehavior/VerticalCharts.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ModifyAxisBehavior, "Vertically Stacked YAxis", "Charts2D/ModifyAxisBehavior/VerticallyStackedYAxis.png"),
            // 2D Charts, MVVM Examples
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MVVMExamples, "Axis Binding and Annotation Binding", "Charts2D/MVVMExamples/AxisAnnotationBinding.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MVVMExamples, "Bind Multiple Charts", "Charts2D/MVVMExamples/BindMultipleCharts.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MVVMExamples, "Bind SciChart to Data", "Charts2D/MVVMExamples/BindSciChartToData.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MVVMExamples, "Manipulate Series Mvvm", "Charts2D/MVVMExamples/ManipulateSeriesMvvm.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_MVVMExamples, "Series Binding", "Charts2D/MVVMExamples/SeriesBinding.png", (window) => Thread.Sleep(1500)),
            // 2D Charts, Styling and Theming
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StylingTheming, "Create a Custom Theme", "Charts2D/StylingTheming/CustomTheme.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StylingTheming, "Dashed Line Styling", "Charts2D/StylingTheming/DashedLineStyling.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StylingTheming, "Use High Quality Rendering", "Charts2D/StylingTheming/UseHQRendering.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StylingTheming, "Using PaletteProvider", "Charts2D/StylingTheming/UsingPaletteProvider.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StylingTheming, "Using PointMarkers", "Charts2D/StylingTheming/UsingPointMarkers.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StylingTheming, "Using ThemeManager", "Charts2D/StylingTheming/UsingThemeManager.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_StylingTheming, "Xaml Styling", "Charts2D/StylingTheming/Xaml Styling.png"),
            // 2D Charts, Tooltips and Hit Test
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Custom Point Marker", "Charts2D/TooltipsAndHitTests/CustomPointMarker.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Custom Tooltips With Modifiers", "Charts2D/TooltipsAndHitTests/TooltipsAndModifiers.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Hit-Test API", "Charts2D/TooltipsAndHitTests/HitTestAPI.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "PointMarkers Selection", "Charts2D/TooltipsAndHitTests/PointMarkersSelection.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Series Selection", "Charts2D/TooltipsAndHitTests/SeriesSelection.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Series With Metadata", "Charts2D/TooltipsAndHitTests/SeriesWithMetadata.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Using CursorModifier Tooltips", "Charts2D/TooltipsAndHitTests/UsingCursorModifier.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Using RolloverModifier Tooltips", "Charts2D/TooltipsAndHitTests/UsingRolloverModifier.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Using TooltipModifier Tooltips", "Charts2D/TooltipsAndHitTests/Using TooltipModifier.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_TooltipsAndHitTest, "Using Vertical Slice Tooltips", "Charts2D/TooltipsAndHitTests/UsingVerticalSlice.png"),
            // 2D Charts, Zoom and Pan a Chart
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomPan, "Custom Overview Control", "Charts2D/ZoomPan/CustomOverviewControl.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomPan, "Drag Area to Zoom", "Charts2D/ZoomPan/DragAreaToZoom.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomPan, "Drag Axis to Scale", "Charts2D/ZoomPan/DragAxisToScale.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomPan, "Mousewheel Zoom and Scroll", "Charts2D/ZoomPan/MousewheelZoomScroll.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomPan, "Pan on Mouse-Drag", "Charts2D/ZoomPan/PanOnMouseDrag.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomPan, "Pan Y or X Direction", "Charts2D/ZoomPan/PanYXDirection.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomPan, "Per-Axis Scrollbars", "Charts2D/ZoomPan/PerAxisScrollbars.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomPan, "Scroll Chart using Overview Control", "Charts2D/ZoomPan/OverviewControl.png"),
            // 2D Charts, Zoom History Manager
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomHistory, "Simple Undo Redo", "Charts2D/ZoomHistory/UndoRedo.png"),
            new ExampleStartTestCase(Category_2DCharts, Group_2D_ZoomHistory, "Zoom History MVVM", "Charts2D/ZoomHistory/ZoomHistoryMVVM.png"),
            // 3D Charts, Create Simple Charts
            new ExampleStartTestCase(Category_3DCharts, Group_3D_BasicChartTypes, "Closed Mesh 3D Chart", "Charts3D/BasicChartTypes/ClosedSurfaceMesh3D.png"),
            new ExampleStartTestCase(Category_3DCharts, Group_3D_BasicChartTypes, "Simple Bubble 3D Chart", "Charts3D/BasicChartTypes/SimpleBubble3DChart.png"),
            new ExampleStartTestCase(Category_3DCharts, Group_3D_BasicChartTypes, "Simple Cylindroid 3D Chart", "Charts3D/BasicChartTypes/SimpleCylindroid3DChart.png"),
            new ExampleStartTestCase(Category_3DCharts, Group_3D_BasicChartTypes, "Simple Ellipsoid 3D Chart", "Charts3D/BasicChartTypes/SimpleEllipsoid3DChart.png"),
            // 3D Charts, Surface Mesh Charts
            new ExampleStartTestCase(Category_3DCharts, Group_3D_SurfaceMesh, "Surface Mesh 3D Non-Uniform Data", "Charts3D/SurfaceMesh/NonUniformSurfaceMesh.png"),
            // Featured Apps, Performance Demos
            new ExampleStartTestCase(Category_FeaturedApps, Group_Featured_PerformanceDemos, "Fifo 1Billion Points Demo", "FeaturedApps/PerformanceDemos/Fifo1BillionPoints.png"),
            new ExampleStartTestCase(Category_FeaturedApps, Group_Featured_PerformanceDemos, "Fast Paletted Scatter Charts", "FeaturedApps/PerformanceDemos/FastPalettedScatterCharts.png"),
            new ExampleStartTestCase(Category_FeaturedApps, Group_Featured_PerformanceDemos, "Scatter Chart Performance Demo", "FeaturedApps/PerformanceDemos/ScatterChartPerformanceDemo.png",
                (mw) => Thread.Sleep(1500)), // bit of a delay to allow example to show 
        };

        [Test]
        [TestCaseSource("FastAssertExampleStartsTestCases")]
        public void WhenStartExample_ShouldHaveExpectedScreenshot_AndExportSource(ExampleStartTestCase testCase)
        {
            // Switch to example
            SwitchToExampleViaBreadCrumb(testCase.Category, testCase.Group, testCase.Example);

            // Any custom operations before test? 
            testCase.BeforeFunc?.Invoke(_mainWindow);

            // 1. Run the screenshot test 
            RunScreenshotTest(testCase);

            // 2. Run the export test
            RunExportExampleTest(testCase);
        }

        private void SwitchToExampleViaBreadCrumb(string category, string group, string example)
        {
            // Click breadcrumb home
            var breadcrumbHome = WaitForElement(() => _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("Breadcrumb.Home")))?.AsButton();
            if (breadcrumbHome == null)
            {
                Assert.Fail("Unable to get Breadcrumb Home button");
            }
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
            var exampleListBoxItem = WaitForElement(() => exampleListBox.FindFirstChild(example).AsListBoxItem());
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
        }

        private void RunExportExampleTest(ExampleStartTestCase testCase)
        {
            // Useful UIAutomation Ids

            // ExportExampleView
            // ExportExampleView.ExportPathTextBox
            // ExportExampleView.ExportButton
            // ExportExampleView.CloseButton
            // ExampleView.Export

            // Toggle the export button, this shows ExportExampleView
            var exportButton = _mainWindow.FindFirstDescendant("ExampleView.Export").AsToggleButton();
            exportButton?.Toggle();

            string exportPath = base.GetTemporaryDirectory();
            try
            {
                var exampleExportView = WaitForElement(() => _mainWindow.FindFirstDescendant("ExportExampleView"));
                var exampleExportTextBox = exampleExportView.FindFirstDescendant("ExportExampleView.ExportPathTextBox")
                    .AsTextBox();
                var exampleExportButton =
                    exampleExportView.FindFirstDescendant("ExportExampleView.ExportButton").AsButton();
                var exampleExportCloseButton =
                    exampleExportView.FindFirstDescendant("ExportExampleView.CloseButton").AsButton();

                // Set output path and export 
                exampleExportTextBox.Text = exportPath;
                exampleExportButton.Invoke();

                // Get the messagebox, close it 
                var msg = WaitForElement(() => _mainWindow.ModalWindows.FirstOrDefault().AsWindow());
                var yesButton = msg.FindFirstChild(cf => cf.ByName("OK")).AsButton();
                yesButton.Invoke();

                // Close example export view
                exampleExportCloseButton?.Invoke();

                // Now check the example
                var subDir = Directory.GetDirectories(exportPath).First();
                var projectFile = Directory.GetFiles(subDir, "*.sln").FirstOrDefault();

                // MSBuild
                //fs.WriteLine("@echo Building " + projectName);
                //fs.WriteLine(@"call ""C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe"" /ToolsVersion:12.0 /p:Configuration=""Debug"" ""{0}/{0}.csproj"" /p:WarningLevel=0", projectName);
                string msBuildPath = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\MSBuild\\Current\\Bin\\MSBuild.exe";
                var msBuildProcess = Process.Start(new ProcessStartInfo(msBuildPath,
                    $"/ToolsVersion:Current /p:Configuration=\"Debug\" \"{projectFile}\" /t:Restore;Build /p:WarningLevel=0"));
                msBuildProcess.WaitForExit(10000);
                Assert.That(msBuildProcess.ExitCode, Is.EqualTo(0), $"Failed to build example {testCase.Category}/{testCase.Group}/{testCase.Example}");
            }
            finally
            {
                Directory.Delete(exportPath, true);
            }
        }

        private void RunScreenshotTest(ExampleStartTestCase testCase)
        {
            var exampleView = WaitForElement(() => _mainWindow.FindFirstDescendant("ExampleView.TransitioningFrame"));
            if (exampleView == null)
            {
                Assert.Fail("Unable to get ExampleView");
            }
            var userControlNotFrame = exampleView.FindFirstByXPath($"Custom");

            // Capture a screenshot & compare
            using (var capture = Capture.Element(userControlNotFrame))
            {
                var actualBitmap = new WriteableBitmap(capture.BitmapImage);

                //#if DEBUG
                // When true, we export the image and open in Paint for test purposes. 
                // Save this image in resources, as embedded resource, then set flag exportActualForTest=false for the actual test
                if (testCase.ExportActual)
                {
                    var pathString = Path.Combine(ExportActualPath, testCase.ResourceName);
                    base.SaveToPng(pathString, actualBitmap);

                    // Export the actual 
                    ProcessStartInfo startInfo = new ProcessStartInfo(pathString);
                    startInfo.Verb = "edit";
                    Process.Start(startInfo);
                }
                //#endif

                WriteableBitmap expectedBitmap = null;
                try
                {
                    expectedBitmap = this.LoadResource(testCase.ResourceName);
                }
                catch (Exception caught)
                {
                    throw new Exception("Unable to load image from resource " + testCase.ResourceName, caught);
                }
                Assert.True(CompareBitmaps(testCase.ResourceName, actualBitmap, expectedBitmap, testCase.Tolerance));
            }
        }
    }
}
