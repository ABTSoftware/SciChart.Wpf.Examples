# SciChart.WPF.Examples

Examples, Showcase Applications and Tutorials for [SciChart.WPF](https://www.scichart.com): High Performance Realtime [WPF Chart Library](https://www.scichart.com/wpf-chart-features). 

![SciChart WPF Collage](https://www.scichart.com/wp-content/uploads/2016/01/SciChart-WPF-Chart-Features-Collage-sml.png)

# Build Instructions 

See end of Readme.md for instructions on how to build the examples. 

# SciChart WPF v6 Released! 

This branch is for the .NET Core 3 and .NET Framework 4.5.2+ version of SciChart, v6.x

* Requires [.NET Core 3.0.100 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.0) or later
* Requires .NET Framework v4.5.2 or later
* Requires Visual Studio 2019 or later

To find out [what's new in SciChart WPF v6 please see this link](https://www.scichart.com/documentation/win/current/What's%20New%20in%20SciChart%20SDK%20v6.html).

# Repository Contents

### Scichart Examples Suite

The SciChart WPF Examples Suite demonstrates 2D & 3D WPF Chart types, as well as featured apps which show the speed, power and flexibility of the SciChart.WPF Chart library. 

**The examples suite source code is found under the [/Examples folder](https://github.com/ABTSoftware/SciChart.Wpf.Examples/tree/SciChart_v6_Release/Examples)).**

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

![SciChart WPF Tutorials](https://www.scichart.com/wp-content/uploads/2020/01/scichart-wpf-tutorial-thumb.png)

* [WPF v6 Tutorials (Code behind)](https://www.scichart.com/documentation/win/current/webframe.html#Tutorial%2001%20-%20Referencing%20SciChart%20DLLs.html)
* [WPF v6 Tutorials (MVVM)](https://www.scichart.com/documentation/win/current/webframe.html#Tutorial%2002b%20-%20Creating%20a%20SciChartSurface%20with%20MVVM.html)

Source code for the tutorials is found under the [/Tutorials folder](https://github.com/ABTSoftware/SciChart.Wpf.Examples/tree/SciChart_v6_Release/Tutorials)

### Sandbox 

A place to put ideas, examples for users to answer support requests and more. See [Sandbox/CustomerExamples](https://github.com/ABTSoftware/SciChart.Wpf.Examples/tree/SciChart_v6_Release/Sandbox/CustomerExamples) for a list of useful examples of customisation of SciChart. 


# Build Instructions 

### NuGet feed setup

The SciChart.Wpf.Examples repository uses nightly builds and is updated automatically during our devops / deployment process. 
To build, you will need to set the correct NuGet feeds for SciChart WPF nightly builds. 

* In Visual Studio go to Tools -> Options -> NuGet Package Manager -> Package Sources
* Add a Package Source called SciChart Nightly 
* Set the feed URL to https://www.myget.org/F/abtsoftware-bleeding-edge/api/v3/index.json

![Nuget Feed Setup](http://www.scichart.com/wp-content/uploads/2015/05/ToolsOptionsNuget.png)

Further instructions for NuGet Feed setup at the page [Getting Nightly Builds with NuGet](http://support.scichart.com/index.php?/Knowledgebase/Article/View/17232/37/getting-nightly-builds-with-nuget)

### Licensing
SciChart WPF is commercial software, and requires a trial or paid license of SciChart WPF to run. 
Get a trial from https://www.scichart.com/licensing-scichart-wpf if needed

### Visual Studio 
After that, open Examples/SciChart2D3D.Examples.sln in Visual Studio 2019 or later, restore NuGet Packages, build and run! 

