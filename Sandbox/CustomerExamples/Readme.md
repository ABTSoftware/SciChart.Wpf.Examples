# SciChart: Useful Examples

The solution SciChart.Sandbox.sln contains a number of useful examples and user provided solutions. We update this list from time to time. 

### Build instructions

1. Ensure NuGet feed is set in Visual Studio options 

2. Build and run the SciChart.Sandbox.sln

3. Double click on the example you want to run. It will start in a new window. 

### Running the app 

With the app running, search or double click the exmaple you want to run. This will start in a new window. 

![SciChart useful examples](https://github.com/ABTSoftware/SciChart.WPF.Examples/blob/master/v5.x/Sandbox/Images/useful-examples.JPG?raw=true)

# Example Directory 

Every example folder is catalogued below, in folder order. The folder name (in `code font`) follows each title.

### 3D Chart Change Properties Dynamically
`3DChartChangePropertiesDynamically`

Demonstrates how to bind and change SciChart3DSurface properties at runtime via MVVM, letting the user alter the chart title, font size, foreground colour, gridline colour, axis title and axis VisibleRange through WPF controls, including a custom AxisGridLineStyleBehavior attached property for gridline stroke on a scatter 3D chart.

### 3D Scatter Series Projected Onto Chart Walls
`3DChartScatterSeriesOnWalls`

Demonstrates how to render a 3D scatter series with 100,000 animated (Brownian-motion) points and project two additional scatter series onto the chart walls, clamping their X/Z positions to the axis VisibleRange and switching walls based on camera position for fast direct-array data updates.

### 3D Surface Mesh With Selectable Points
`3DChartSelectPointsOnSurfaceMesh`

Demonstrates how to overlay a SurfaceMeshRenderableSeries3D (built from a NonUniformGridDataSeries3D) with a mirrored scatter series of sphere point markers so individual mesh vertices can be selected via the VertexSelectionModifier3D (CTRL+drag) or programmatically, with axis-flip checkboxes.

### 3D Scatter Chart Draggable Points
`3DScatterChart_DragPointModifier`

Demonstrates how to write a custom ChartModifierBase3D (DragPointYAxisModifier3D) that hit-tests a scatter point and drags it along the Y axis using ray/plane projection math, raising drag start/delta/end events to update the point's Y value over a surface mesh.

### Add 3D Objects To A 3D Chart
`AddObjectsToA3DChart`

Demonstrates how to add custom 3D object models (Wavefront .obj with a texture) as SceneObjects on a SciChart3DSurface, both declaratively in XAML via ObjectModelSource and at runtime by loading an .obj model from an in-memory stream, positioned over a checkerboard surface mesh.

### Animated DataSeries Via Custom Filter
`AnimatedDataSeries`

Demonstrates how to create a custom FilterBase subclass that progressively appends points from an original XyDataSeries to a filtered series on a timed schedule with easing, producing an animated "draw-on" line chart effect.

### Draggable 3D Box Annotation
`AnnotationDragModifier3D`

Demonstrates how to build a custom 3D box annotation as a BaseSceneEntity that renders a lit, wireframed cube whose faces can be hit-tested and dragged with the mouse (with CTRL/Shift for symmetric resizing), mapping its RangeX/Y/Z to axis coordinates and binding a range value to a TextBlock.

### Two-Way Binding Annotations To A TextBox
`AnnotationsBindingToTextBox`

Demonstrates how to bind chart annotations via MVVM (AnnotationsBinding) using a VerticalLineAnnotationViewModel, and two-way bind its X1 value to an external WPF TextBox so editing the textbox moves the annotation and dragging the annotation updates the textbox.

### Aspect Ratio Grid Lines
`AspectRatioGridLines`

Demonstrates how to maintain square, constant-size grid cells by using fixed Major/MinorDelta axis styling and recalculating the X axis VisibleRange from the Y range whenever the surface is resized or the Y range changes, with custom direction-locked zoom/pan modifiers.

### Constant Aspect Ratio Sandbox
`AspectRatioSandbox`

Demonstrates how to keep a 1:1 chart aspect ratio (drawing an undistorted circle) by handling Window.SizeChanged and normalizing the X axis VisibleRange.Max against the Y range using the surface's GridLinesPanel dimensions.

### Apply Axis Styles In MVVM
`AxisMvvmApplyStyle`

Demonstrates how to apply named XAML Styles to axes defined via MVVM by setting the StyleKey on NumericAxisViewModel instances bound through AxesBinding, styling border, tick text brush and label-fitting on the X and Y axes.

### Print And Export Multi-Pane Charts
`ChartPrinting`

Demonstrates how to display multiple MVVM-bound SciChartSurfaces stacked in a UniformGrid and then print them all via PrintDialog.PrintVisual or export the whole group to a PNG using RenderToBitmap.

### Chart Width Synchronization
`ChartsWidthSync`

Demonstrates how to synchronize the Y-axis area widths of two vertically stacked SciChartSurfaces using SciChartGroup.VerticalChartGroup, with the alternative of turning the Y-axis inwards (IsCenterAxis) shown on the second chart, while sharing X/Y visible ranges and mouse modifier events between them.

### Column Series With No Gaps
`ColumnSeriesNoGaps`

Demonstrates how to render a FastColumnRenderableSeries with no gaps between columns using DataPointWidth=1 and UseUniformWidth, and how to color each column individually via a custom IFillPaletteProvider/IStrokePaletteProvider.

### Composite Annotations In MVVM
`CompositeAnnotationsMvvm`

Demonstrates how to define a reusable composite annotation (a box with two vertical boundary lines) as a CompositeAnnotationForMvvm and bind a collection of annotation view models to the surface via AnnotationsBinding, configured as an editable, X-direction draggable/resizable range.

### Custom Axis Bands Provider
`CustomAxisBandsProvider`

Demonstrates how to draw custom colored axis bands on a DateTime X-axis and numeric Y-axis by subclassing the axis bands providers, plus a custom ChartModifier that hit-tests the mouse position to identify which band and axis value is under the cursor.

### Custom Composite Annotation
`CustomCompositeAnnotationExample`

Demonstrates how to build a custom CompositeAnnotation (a square composed of four LineAnnotations) with full drag, move and resize support, and create it interactively on a candlestick chart using an AnnotationCreationModifier toggled by a button.

### Custom Chart Modifiers Sandbox
`CustomModifiersSandbox`

Demonstrates how to implement a wide range of custom ChartModifiers (series drag, data-point editing, point selection, free draw, legend, rollover, zoom/pan, zoom in/out, and Y-axis mouse-wheel zoom) that can be enabled or disabled at runtime on a bid/offer line chart.

### Custom Point Markers
`CustomPointMarker`

Demonstrates how to create custom point markers three ways (a built-in TrianglePointMarker, a SpritePointMarker driven by a control template, and a custom BasePointMarker that draws a diamond), together with a customized legend offering per-series toggles for line/marker visibility, stroke thickness and color.

### Custom Renderable Series In MVVM
`CustomSeriesMvvm`

Demonstrates how to create a custom renderable series by overriding InternalDraw on FastLineRenderableSeriesForMvvm and pairing it with a custom series view model that overrides ViewType, so the custom series type works with SeriesBinding in an MVVM chart.

### Custom Shaped Zoom Modifier
`CustomShapeZoomModifier`

Demonstrates how to write a custom ChartModifier that draws a rubber-band rectangle for XY zoom or a line-with-end-cap shape for single-axis (X-only or Y-only) zoom while dragging, then animates the selected axes to the chosen range, with the zoom axis mode selectable from a combo box.

### DPI-Aware SciChartSurface
`DPI_Aware_SciChartSurface`

Demonstrates how to subclass SciChartSurface to respond to OnDpiChanged by applying an inverse ScaleTransform and NearestNeighbor bitmap scaling, keeping gridlines crisp and the chart correctly sized when Windows display scaling exceeds 100%, shown on an animated scatter chart.

### Dashed Lines Chart
`DashedLinesChart`

Demonstrates how to render dashed strokes by setting StrokeDashArray on a FastLineRenderableSeries and a FastBandRenderableSeries, with major bands drawn on both axes.

### Digital Analyzer Performance Demo
`DigitalAnalyzerPerformanceDemo`

Demonstrates how to build a high-performance multi-channel digital logic analyzer using many stacked SciChartSurfaces bound in an ItemsControl that share a common X-axis range, generating configurable channel counts and up to a billion points per channel with synchronized zoom/pan and channel-height scroll behaviors.

### Numeric 3D Axis Displaying DateTime Labels Via OADate
`DoubleAxisAsDateTimeAxis`

Demonstrates how to make a NumericAxis3D behave like a DateTime axis on a 3D Waterfall chart by storing OADate double values as slice positions and using a custom LabelProvider (OADateLabelProvider) that converts the double back to DateTime for axis and cursor label formatting.

### Discontinuous DateTime Axis With Dual-Scale Bands And Weekday Calendar
`DoubleScaleDiscontinuousDateTimeAxis`

Demonstrates how to use the DoubleScaleDiscontinuousDateTimeAxis on a candlestick chart to collapse non-trading days, applying a custom DiscontinuousDateTimeCalendar (WeekDaysAxisCalendar) that skips Saturdays and Sundays and a selectable AxisBandsFrequency for the dual-scale axis bands.

### Dynamically Adding And Removing Vertically Stacked Y Axes
`DynamicVerticallyStackedAxis`

Demonstrates how to add and remove line series and their associated Y axes at runtime via MVVM (AxesBinding/SeriesBinding), using a UniformGrid LeftAxesPanelTemplate to render the multiple left Y axes as a vertically stacked layout.

### Eliminating Flicker In The DirectX (Visual Xccelerator) Renderer
`EliminatingFlickerInDirectXRenderer`

Demonstrates how to remove flicker seen on some PCs when the VisualXcceleratorEngine (DirectX) renderer is enabled during resize, by toggling the VisualXcceleratorEngine.UseAlternativeFillSource and EnableForceWaitForGPU flags on two real-time updating mountain charts split by a GridSplitter.

### Firing An Event When Zoom Extents Animation Completes
`EventOnZoomExtentsCompleted`

Demonstrates how to subclass ZoomExtentsModifier (ZoomExtentsModifierEx) to expose a ZoomExtentsCompleted event that fires only after the animated zoom-to-extents finishes, by watching the X axis VisibleRangeChanged event until IsAnimating is false.

### Hit-Testing In 2D And 3D Charts
`HitTestSandbox`

Demonstrates how to hit-test SciChart surfaces from a launcher window: a 2D example converts mouse pixel coordinates to a data value and adds a VerticalLineAnnotation on mouse-down, and a 3D example performs HitTest on a surface mesh to retrieve and display the clicked vertex XYZ coordinates.

### Applying Implicit (TargetType) Styles To Chart Elements
`ImplicitStyles`

Demonstrates how to style SciChartSurface and NumericAxis using implicit WPF styles (TargetType with BasedOn the default style) to override border, background, axis bands, and major/minor grid line brushes without naming each element explicitly.

### Keyboard WASD Camera Movement In A 3D Chart
`KeyboardMoveXozModifier3D`

Demonstrates how to create a custom 3D chart modifier (KeyboardMoveXozModifier3D) derived from FreeLookModifier3D that moves the camera across the X-O-Z plane using the W, A, S, D keys on a 3D scatter point cloud.

### MATLAB-Style 3D Surface Mesh Chart
`LabStyleCharts`

Demonstrates how to style a 3D SurfaceMeshRenderableSeries3D to mimic MATLAB-style scientific plots, using a height-based GradientColorPalette, a perspective camera, the BrightSpark theme, and heavily customized axis styles (grid lines, tick lines, titles, screen-rotated labels).

### Per-Label Individual Axis Tick Colouring
`LabelIndividualStylingColoring`

Demonstrates how to colour each axis tick label individually by creating a custom NumericLabelProvider (ColorLabelProvider) and NumericTickLabelViewModel that computes a per-value Foreground brush via a ColorGenerator, bound through a DefaultTickLabel TickLabelStyle template.

### Legend Checkboxes To Toggle Y Axis Visibility
`LegendAxisVisibilityCheckbox`

Demonstrates how to build a custom SciChartLegend item template with a checkbox that toggles the visibility of each series' associated Y axis (on a dual-Y-axis chart), using a TwoWayBooleanToVisibilityConverter to bind the checkbox to the axis Visibility.

### 3D LIDAR Point Cloud And Topography Surface Mesh
`Lidar3DPointCloudDemo`

Demonstrates how to load real LIDAR .asc elevation data (via AscReader) and render it as both a 3D scatter point cloud with height-mapped colours and a SurfaceMeshRenderableSeries3D topography map, with a LinearColorMap/HeatmapColorMap legend and a custom 3D legend offering visibility and opacity controls.

### Market Profile Trading Chart
`MarketProfileTradingExample`

Demonstrates how to build a real-time ticking market-profile trading chart, combining FastHistoBarRenderableSeries and FastMarketProfileRenderableSeries to draw volume-at-price histograms (MarketProfile, VolumeLadder and CumulativeVolume modes) around candlesticks, with a custom PaletteProvider, viewport manager, overview scrollbar and flyout controls to tune bar spacing, tick size and candle count.

### Mirrored (Dual) Y Axis
`MirroredYAxis`

Demonstrates how to display two Y axes, one aligned left and one aligned right, that mirror each other by sharing a single two-way-bound VisibleRange; provided in both an MVVM (AxisViewModel collections + AxesBinding) and a code/XAML variant.

### Mouse Events On Annotations
`MouseEventsOnAnnotations`

Demonstrates how to handle mouse events on annotations by wiring PreviewMouseDoubleClick handlers to a HorizontalLineAnnotation and to a VerticalLineAnnotation hosted inside a VerticalSliceModifier, showing a message box on double-click.

### Multi-Line DateTime Axis Labels
`MultiLineDateTimeAxisLabels`

Demonstrates how to render axis tick labels across multiple lines on a DateTimeAxis by using a TextFormatting string containing a line break ("dd MMM yyyy\r\nHH:mm:ss") together with a centered TextBlock DefaultTickLabel style.

### SciChart In Multiple AppDomains
`MutipleAppDomainsExample`

Demonstrates how to host SciChart charts in separate .NET AppDomains, each running its own STA thread and Dispatcher, disabling VisualXcceleratorEngine auto-shutdown and calling RestartEngine so the GPU engine works correctly across multiple domains.

### SciChart On Multiple UI Threads
`MutipleUIThreadExample`

Demonstrates how to run multiple SciChart windows on separate UI threads, launching a second window on its own STA thread with an independent Dispatcher while the main window renders its own SciChartSurface.

### Off-Screen Chart Export To Image
`OffScreenExportExample`

Demonstrates how to create SciChartSurface instances in memory (never shown in a Window) and export them to PNG files via ExportToFile, covering exporting a single chart, exporting a batch of multiple charts, and exporting with cloning at a custom output size.

### Oil And Gas Well-Log Dashboard
`OilAndGasExample`

Demonstrates how to build an oil-and-gas well-log dashboard that composes several chart panels via MVVM chart factories: a 2D grid-chart panel (mountain/scatter), a set of vertical well-log tracks (density, resistivity, shale, sonic, pore-space, texture with custom palette providers and axis legends), and a 3D scatter panel.

### 3D Plane Annotation
`Plane3DAnnotation`

Demonstrates how to add a custom semi-transparent vertical plane geometry into a SciChart3DSurface scene by building a VerticalPlaneGeometry and adding it to the Viewport3D RootEntity's children, effectively creating a 3D plane annotation.

### Resampling A 3D Grid Data Series
`ResamplingOfGridDataSeries3D`

Demonstrates how to down-sample a large WaterfallDataSeries3D (10 slices of 65,000 points) using PointResamplerFactory/ResamplingParams before assigning it to a 3D waterfall series rendered with a gradient color palette and a logarithmic Z axis.

### Rotated Axis Labels
`RotatedAxisLabels`

Demonstrates how to rotate axis tick labels by applying a custom DefaultTickLabel style with a -45 degree RotateTransform (plus custom font, color and major-tick line style) to a NumericAxis, with label culling disabled.

### 3D Surface Mesh Vertex Selection
`SciChartSurfaceMeshSelection`

Demonstrates how to write a custom ChartModifierBase3D that selects vertices of a SurfaceMeshRenderableSeries3D by point-click or rubber-band drag (using Viewport3D.PickScene), then highlights the picked mesh vertices through a bound metadata PaletteProvider.

### Synchronize Mouse And Ranges Across Multiple Charts (MVVM)
`SciChart_SyncMultiChartMvvm`

Demonstrates how to synchronize the mouse cursor/rollover and shared axis VisibleRanges across several SciChartSurface instances in an MVVM setup, via a reusable "SynchronizeMouseAcrossCharts" UserControl.

### Scrollbar Positioned Above The X Axis
`ScrollbarAboveAxis`

Demonstrates how to place a SciChartScrollbar between two stacked SciChartSurfaces so the scrollbar sits above the visible axis, using a hidden secondary surface whose XAxis is two-way bound to the primary chart's VisibleRange to render the axis beneath the scrollbar.

### Toggle Axis Scrollbars In MVVM
`ScrollbarMvvmAxis`

Demonstrates how to add or remove SciChartScrollbars on X and Y axes at runtime in MVVM, using a custom NumericAxisViewModelWithScrollbar exposing a HasScrollbar property that drives XAML DataTriggers on the axis styles.

### Select Series On Mouse Hover
`SelectSeriesOnHover`

Demonstrates how to write a custom ChartModifier (HoverSelectionModifier) that hit-tests the renderable series under the mouse on each move and sets IsSelected, applying a SelectedSeriesStyle to highlight the hovered line.

### Simple Line Chart In VB.NET
`SimpleLineChart_VB`

Demonstrates how to create a basic FastLineRenderableSeries line chart in Visual Basic, filling a UniformXyDataSeries with Fourier data on window load, zooming to extents, and applying a SweepAnimation.

### Custom Lightweight Line Renderable Series
`SlimLineRenderableSeries`

Demonstrates how to implement a minimal custom IRenderableSeries from scratch (with its own drawing provider and hit-test provider) to render a fast, low-overhead line series bound to a UniformXyDataSeries.

### State Series Rendering, Three Methods
`State Series Example`

Demonstrates three techniques for drawing a discrete state (Normal/Warning/Error) scatter strip alongside a line chart, colored by a custom StatePaletteProvider: Method 1 nests a second SciChartSurface inside a BoxAnnotation, Method 2 uses a separate left axis panel/axis, and Method 3 uses a custom CustomRenderableSeries pinned to the top of the viewport.

### Switch Stock Chart Between Linear And Logarithmic Y Axis
`StockChartLogAxis`

Demonstrates how to swap a candlestick/OHLC stock chart's Y axis between a NumericAxis and a LogarithmicNumericAxis at runtime using a custom SwitchAxisTypeBehavior attached property bound to a checkbox, alongside selectable series types and rollover/cursor modifiers.

### String Category Labels On The X Axis
`StringsOnXAxis`

Demonstrates how to show text labels (fruit names) on a NumericAxis by assigning a custom NumericLabelProvider (StringLabelProvider) that maps integer index values to strings for both axis and cursor labels.

### Scrolling Strip Chart With Relative Time Axis
`StripChart`

Demonstrates how to build a real-time scrolling strip chart that appends timed samples on a DispatcherTimer, auto-scrolls a fixed 10-second window, freezes/resumes scrolling on mouse-down/double-click, and formats the X axis with relative "t-N" labels via a custom RelativeTimeLabelProvider.

### Dashed-Stroke Custom Point Markers
`StrokeDashArrayPointMarkers`

Demonstrates how to create a custom BitmapSpriteBase point marker whose ellipse uses a dashed StrokeDashArray, and compares its output rendered by the HighQuality software render surface versus the VisualXccelerator (GPU) engine side by side.

### Sweeping ECG Trace
`SweepingEcgSeries`

Demonstrates how to render a real-time sweeping/wrapping ECG waveform by reusing a fixed-capacity XyzDataSeries (overwriting Y/Z values once full) and applying a custom IStrokePaletteProvider (DimTracePaletteProvider) to fade the older part of the trace, with a glowing CustomAnnotation marking the latest point.

### Custom Text Annotation Off-Screen Detection
`TextAnnotationDynamicSize`

Demonstrates how to subclass TextAnnotation and override Update() to read its Canvas position and detect/log when a rotated label pans off the top of the viewport.

### Draggable Threshold Band Series (MVVM)
`ThresholdedLineSeries`

Demonstrates how to bind an editable HorizontalLineAnnotation two-way to a ViewModel Threshold value so dragging it updates the Y1 values of an XyyDataSeries driving a FastBandRenderableSeries, with the current threshold shown in a custom legend template.

### Millimetre-Spaced Gridlines via Custom TickProvider
`TicklinesUniformGrid`

Demonstrates how to write a custom TickProvider that computes major (25mm) and minor (5mm) tick positions from physical screen size using the DPI transform, producing gridlines spaced by real-world millimetres.

### Custom Timeline Renderable Series
`TimelineControl`

Demonstrates how to build a CustomRenderableSeries that draws colored timeline blocks from an XyzDataSeries (X = start, Y = length, Z = color) via the IRenderContext2D Draw API, with configurable YOffset/Height and a custom GetXRange override.

### Touch-Screen Chart Modifiers Sandbox
`TouchScreenModifiers`

Demonstrates how to configure touch interaction modifiers (PinchZoom, ZoomPan, axis drag, Rollover, Tooltip, Legend) toggled via checkboxes, and how to implement a custom ChartModifierBase that handles touch down/move/up and manipulation-delta events.

### True Polar Chart With Polar Axes
`TruePolar`

Demonstrates how to create a polar chart using PolarXAxis (0-360) and PolarYAxis with a FastLineRenderableSeries that spirals outward over 720 points.

### Low-Level RenderContext Drawing API
`UsingRenderContextAPI`

Demonstrates how to use the VisualXccelerator render surface and its IVxRenderContext directly to draw primitives (a filled ellipse) and to create and render an image texture from raw pixel data.

### SciChart WPF on VS2022 / .NET 6.0 Boilerplate
`VS2022_Net60_Boilerplate`

Demonstrates how to set up a minimal SciChart WPF 6.5 project (single SciChartSurface with numeric axes and a centered text annotation) targeting .NET 6.0 in Visual Studio 2022.

### VerticalSliceModifier via MVVM
`VerticalSliceModifierMvvm`

Demonstrates how to drive a VerticalSliceModifier from a ViewModel by binding its VerticalLines to an ObservableCollection of VerticalLineAnnotationViewModel, with commands to add random and clear vertical slice lines.

### Enabling the VisualXccelerator GPU Engine
`VisualXcceleratorEnableTest`

Demonstrates how to enable the VisualXccelerator (GPU) rendering engine with DowngradeWithoutException and a HighSpeedRenderSurface fallback, and how to display the active render surface type and SciChart version/license info.

### Synchronizing Y-Axis Zero Lines Across Charts
`YAxisSameZeroLine`

Demonstrates (as a work-in-progress test case) how to use an attached property that groups Y axes on separate SciChartSurfaces and subscribes via Rx to VisibleRangeChanged events, intended to keep the axes' zero lines aligned.

### Zoom To Extents After Adding Series In MVVM
`ZoomExtentsAfterMvvmSeriesChanges`

Demonstrates how to call AnimateZoomExtents from a ViewModel through a bound DefaultViewportManager (dispatched after layout) so the chart re-zooms whenever a new series is added to the SeriesBinding collection.

### Auto Zoom Extents On Series Visibility Change
`ZoomExtentsOnVisibilityChanged`

Demonstrates how to use an attached behaviour that subscribes to each RenderableSeries' IsVisibleChanged event and calls AnimateZoomExtents, so toggling series visibility via checkboxes re-fits the chart automatically.
