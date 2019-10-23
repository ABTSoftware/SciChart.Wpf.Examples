
# SciChart.WPF.Examples
## Branch Structure

> **NOTE:** Master branch contains v4, v5 examples. For SciChart WPF v6 Examples, please use [branch SciChart_v6_Release](https://github.com/ABTSoftware/SciChart.Wpf.Examples/tree/SciChart_v6_Release)

## What's in this Repo? 

Examples, Showcase Applications and Tutorials for [SciChart.WPF](https://www.scichart.com): High Performance Realtime [WPF Chart Library](https://www.scichart.com/wpf-chart-features). 

WPF Chart Examples are provided in C# / WPF. If you are looking for other platforms then please see here:

* [iOS Charts](https://github.com/ABTSoftware/SciChart.iOS.Examples) (Swift / Objective C)
* [Android Charts](https://github.com/ABTSoftware/SciChart.Android.Examples) (Java / Kotlin)
* [Xamarin Charts](https://github.com/ABTSoftware/SciChart.Xamarin.Examples) (C#) BETA!

### Note: NuGet feed setup

To build, you will need to set the correct NuGet feeds for SciChart WPF v4.x and v5.x. 
NuGet Feed setup instructions are found at the page [Getting Nightly Builds with NuGet](http://support.scichart.com/index.php?/Knowledgebase/Article/View/17232/37/getting-nightly-builds-with-nuget)

> *Add these feeds to Visual Studio -> Tools -> Options -> NuGet Package*
> *Manager -> Package Sources to ensure you can get nightly builds:*
> 
> -   ***Name:** SciChart* 
> -   ***Source:**  [https://www.myget.org/F/abtsoftware/api/v3/index.json](https://www.myget.org/F/abtsoftware/api/v3/index.json)*
> -   ***Name:** SciChart Bleeding-Edge (Beta/Alpha packages)*
> -   ***Source:**  [https://www.myget.org/F/abtsoftware-bleeding-edge/api/v3/index.json](https://www.myget.org/F/abtsoftware-bleeding-edge/api/v3/index.json)*

![How to add NuGet Feeds to Visual Studio](http://www.scichart.com/wp-content/uploads/2015/05/ToolsOptionsNuget.png)

# Repository Contents

### Scichart Examples Suite

The SciChart WPF Examples Suite demonstrates 2D & 3D WPF Chart types, as well as featured apps which show the speed, power and flexibility of the SciChart.WPF Chart library. 

![SciChart WPF Collage](https://www.scichart.com/wp-content/uploads/2016/01/SciChart-WPF-Chart-Features-Collage-sml.png)

This showcase is written in WPF with MVVM, Unity Container and Reactive Extensions and is designed to be a demonstration of what SciChart WPF can do. 

#### WPF Chart Types 

SciChart WPF Includes the following 2D & 3D chart types, as well as an wide set of features, excellent performance and a poweful, flexible API.

* [WPF Line Chart](https://www.scichart.com/wpf-chart-example-line-chart)
* [WPF Band Chart](https://www.scichart.com/wpf-chart-example-band-series-chart)
* [WPF Candlestick Chart](https://www.scichart.com/wpf-chart-example-candlestick-chart)
* WPF OHLC Chart 
* [WPF Column Chart](https://www.scichart.com/wpf-chart-example-column-chart)
* [WPF Pie Chart](https://www.scichart.com/wpf-pie-chart-example/)
* [WPF Donut Chart](https://www.scichart.com/wpf-donut-chart-example/)
* [WPF Mountain / Area Chart](https://www.scichart.com/wpf-chart-example-mountain-chart)
* [WPF Scatter Chart](https://www.scichart.com/wpf-chart-example-scatter-chart)
* [WPF Impulse / Stem Chart](https://www.scichart.com/wpf-chart-example-impulse-(stem)-chart)
* [WPF Bubble Chart](https://www.scichart.com/wpf-chart-example-bubble-chart)
* [WPF Error Bars Chart](https://www.scichart.com/wpf-chart-example-error-bars)
* [WPF Stacked Mountain Chart](https://www.scichart.com/wpf-chart-example-stacked-mountain-chart)
* [WPF Stacked Column Chart](https://www.scichart.com/wpf-chart-example-stacked-column-chart)
* [WPF 100% Stacked Mountain Chart](https://www.scichart.com/wpf-chart-example-dashboard-style-charts)
* [WPF 100% Stacked Column Chart](https://www.scichart.com/wpf-chart-example-dashboard-style-charts)
* WPF Radar Chart (v5+ Only)
* [WPF Heatmap Chart](https://www.scichart.com/wpf-chart-example-heatmap-chart)
* [WPF Spectrogram Chart](https://www.scichart.com/wpf-chart-example-spectrogram-demo-chart)
* [WPF Polar Chart](https://www.scichart.com/wpf-chart-example-polar-chart)

#### WPF 3D Chart Types

SciChart WPF also has an array of DirectX-powered Realtime 3D Charts for WPF, including:

![SciChart WPF3D Collage](https://www.scichart.com/wp-content/uploads/2017/03/3d-charts-dash.jpg)

* [WPF 3D Bubble Chart](https://www.scichart.com/wpf-3d-chart-example-simple-bubble-3d-chart)
* [WPF 3D Point Cloud Chart](https://www.scichart.com/wpf-3d-chart-example-simple-point-cloud-3d-chart)
* [WPF 3D Scatter Chart](https://www.scichart.com/wpf-3d-chart-example-simple-scatter-chart-3d)
* [WPF 3D Column Chart](https://www.scichart.com/wpf-3d-chart-example-uniform-column-3d)
* [WPF 3D Surface Mesh Chart](https://www.scichart.com/wpf-3d-chart-example-simple-uniform-mesh-3d-chart)
* [WPF 3D Impulse Chart](https://www.scichart.com/wpf-3d-chart-example-uniform-impulse-series-3d)
* WPF 3D Waterfall Chart (v5+ Only)

### Tutorials 

SciChart WPF Comes with a number of tutorials to help you get started quickly using our powerful & flexible chart library! Please see below:

![SciChart WPF Tutorials](https://www.scichart.com/wp-content/uploads/2012/06/scichart-tutorials-thumb.png)

* [Tutorial 01 Referencing the SciChart Dlls](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2001%20-%20Referencing%20SciChart%20DLLs.html)
* [Tutorial 02 Creating A SciChartSurface](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2002%20-%20Creating%20a%20SciChartSurface.html)
* [Tutorial 03 Adding Series](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2003%20-%20Adding%20Series%20to%20a%20Chart.html)
* [Tutorial 04 Adding Zooming Panning](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2004%20-%20Adding%20Zooming,%20Panning%20Behavior.html)
* [Tutorial 05 Adding ToolTips And Legends](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2004%20-%20Adding%20Zooming,%20Panning%20Behavior.html)
* [Tutorial 06 Adding RealTime Updates](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2006%20-%20Adding%20Realtime%20Updates.html)
* [Tutorial 07 Annotations](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2007%20-%20Adding%20Annotations.html)
* [Tutorial 08 Adding Multiple Axis](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2008%20-%20Adding%20Multiple%20Axis.html)
* [Tutorial 09 Adding Multiple Charts](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2009%20-%20Linking%20Multiple%20Charts.html)

A number of MVVM Tutorials are also available, see here:

* [Tutorial 02b Creating A SciChartSurface with MVVM](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2002b%20-%20Creating%20a%20SciChartSurface%20with%20MVVM.html)
* [Tutorial 03b Adding Series with MVVM](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2003b%20-%20Adding%20Series%20to%20a%20Chart%20with%20MVVM.html)
* [Tutorial 04b Adding Zooming Panning with MVVM](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2004b%20-%20Adding%20Zooming,%20Panning%20to%20a%20Chart%20with%20MVVM.html)
* [Tutorial 05b Adding ToolTips And Legends with MVVM](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2005b%20-%20Adding%20Tooltips,%20Legends%20with%20MVVM.html)
* [Tutorial 06b Adding RealTime Updates with MVVM](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2006b%20-%20Adding%20Realtime%20Updates%20with%20MVVM.html)
* [Tutorial 07b Annotations with MVVM](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2007b%20-%20Adding%20Annotations%20with%20MVVM.html)
* [Tutorial 08b Adding Multiple Axis with MVVM](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2008b%20-%20Adding%20Multiple%20Axis%20with%20MVVM.html)
* [Tutorial 09b Adding Multiple Charts with MVVM](https://www.scichart.com/documentation/v5.x/webframe.html#Tutorial%2009b%20-%20Linking%20Multiple%20Charts%20with%20MVVM.html)

### Sandbox 

A place to put ideas, examples for users to answer support requests and more. 

### Note: NuGet feed setup

To build, you will need to set the correct NuGet feeds for SciChart WPF v4.x and v5.x. 
NuGet Feed setup instructions are found at the page [Getting Nightly Builds with NuGet](http://support.scichart.com/index.php?/Knowledgebase/Article/View/17232/37/getting-nightly-builds-with-nuget)


> *Add these feeds to Visual Studio -> Tools -> Options -> NuGet Package*
> *Manager -> Package Sources to ensure you can get nightly builds:*
> 
> -   ***Name:** SciChart* 
> -   ***Source:**  [https://www.myget.org/F/abtsoftware/api/v3/index.json](https://www.myget.org/F/abtsoftware/api/v3/index.json)*
> -   ***Name:** SciChart Bleeding-Edge (Beta/Alpha packages)*
> -   ***Source:**  [https://www.myget.org/F/abtsoftware-bleeding-edge/api/v3/index.json](https://www.myget.org/F/abtsoftware-bleeding-edge/api/v3/index.json)*

![How to add NuGet Feeds to Visual Studio](http://www.scichart.com/wp-content/uploads/2015/05/ToolsOptionsNuget.png)