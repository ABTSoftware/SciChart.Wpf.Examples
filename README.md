# SciChart WPF Demo, Examples, Tutorials and Boilerplates

## What's in this repo?

1. **Examples Source code** for [SciChart.WPF](https://www.scichart.com): High Performance Realtime [WPF Chart Library](https://www.scichart.com/wpf-chart-features) found under the **[/Examples](Examples)** folder
2. Source code for **tutorials** for SciChart WPF, found under the **[/Tutorials](Tutorials)** folder
3. **Sandbox examples**, including a [/Sandbox/LicensingTestApp](Licensing Test App) plus several useful examples found under the **[/Sandbox](Sandbox)** folder

> &nbsp;&nbsp;&nbsp;  
> **Scroll down for Build Instructions!**   
> &nbsp;&nbsp;&nbsp;

# Using the SciChart.Wpf.Examples Repository
## [/Examples Folder](Examples)

The SciChart WPF Examples Suite has over 130 examples of 2D & 3D [WPF Charts](https://www.scichart.com/wpf-chart-features) with & without MVVM, as well as featured apps which show the speed, power and flexibility of the SciChart.WPF Chart library!

Browse SciChart WPF examples, search by keyword, code or description, view source-code and export examples to stand-alone Visual Studio solutions.

[![SciChart WPF Examples - WPF Chart library](https://www.scichart.com/wp-content/uploads/2023/11/wpf-scichart-examples-small.jpg)](https://demo.scichart.com)

## Build Instructions

> To compile the Examples App, you will need:
> 
> - To start a SciChart WPF Trial [find out how](https://www.scichart.com/getting-started/scichart-wpf) or purchase a license.
>    - Although the examples app will work without a trial license, modifying or creating your own examples won't
> - To setup the NuGet package source before you can compile. Here's How 
>  
>    1. After cloning the repo, open SciChart2D3D.Examples.sln found in the [/Examples](Examples) folder in Visual Studio.
>    2. Setup the Nuget Package source. While SciChart is hosted on Nuget.org, Hotfix builds (required for this examples app repo) are hosted at MyGet. 
>       * In Visual Studio go to Tools -> Options -> NuGet Package Manager -> Package Sources
>       * Add a Package Source called "SciChart Hotfixes"
>       * Set the feed URL to https://www.myget.org/F/abtsoftware-bleeding-edge/api/v3/index.json, like this
> 
>       ![Nuget Feed Setup](https://www.scichart.com/wp-content/uploads/2023/11/hotfix-scichart-myget.png)
>   
>  - **Once you've done that, compile and run the app as usual**    
> &nbsp; &nbsp; &nbsp; 



#### WPF 2D Chart Types 

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
* [WPF Radar Chart](https://www.scichart.com/example/wpf-chart/wpf-chart-example-using-radar-chart/)
* [WPF Heatmap Chart](https://www.scichart.com/wpf-chart-example-heatmap-chart)
* [WPF Spectrogram Chart](https://www.scichart.com/wpf-chart-example-spectrogram-demo-chart)
* [WPF Polar Chart](https://www.scichart.com/wpf-chart-example-polar-chart)

#### WPF 3D Chart Types

SciChart WPF also has an array of DirectX-powered Realtime 3D Charts for WPF, including:

* [WPF 3D Bubble Chart](https://www.scichart.com/wpf-3d-chart-example-simple-bubble-3d-chart)
* [WPF 3D Point Cloud Chart](https://www.scichart.com/wpf-3d-chart-example-simple-point-cloud-3d-chart)
* [WPF 3D Scatter Chart](https://www.scichart.com/wpf-3d-chart-example-simple-scatter-chart-3d)
* [WPF 3D Column Chart](https://www.scichart.com/wpf-3d-chart-example-uniform-column-3d)
* [WPF 3D Surface Mesh Chart](https://www.scichart.com/wpf-3d-chart-example-simple-uniform-mesh-3d-chart)
* [WPF 3D Impulse Chart](https://www.scichart.com/wpf-3d-chart-example-uniform-impulse-series-3d)
* [WPF 3D Waterfall Chart](https://www.scichart.com/example/wpf-chart/wpf-3d-chart-example-simple-waterfall-3d-chart/)

## [/Tutorials Folder](Tutorials)

SciChart WPF Comes with a number of tutorials to help you get started quickly using our powerful & flexible chart library! Please see below:

![SciChart WPF Tutorials](https://www.scichart.com/wp-content/uploads/2020/01/scichart-wpf-tutorial-thumb.png)

* [WPF Tutorials (Code behind)](https://www.scichart.com/documentation/win/current/webframe.html#Tutorial%2001%20-%20Referencing%20SciChart%20DLLs.html)
* [WPF Tutorials (MVVM)](https://www.scichart.com/documentation/win/current/webframe.html#Tutorial%2002b%20-%20Creating%20a%20SciChartSurface%20with%20MVVM.html)

Source code for the tutorials is found under the [/Tutorials folder](/Tutorials)

## [/Sandbox Folder](Sandbox)

A place to put ideas, examples for users to answer support requests and more. 

- See [Sandbox/CustomerExamples](/Sandbox/CustomerExamples/) for a list of useful examples of customisation of SciChart. 
- See [Sandbox/LicensingTestApp](/Sandbox/LicensingTestApp/) for a test-app to help debug SciChart licenses









## ... And Finally!

### License

> [SciChart WPF](https://scichart.com/wpf-chart-features) is commercial software with a free 30-day trial. Built by a dedicated team of developers, were an independent business which strives to do the best for our users!
>
> Anything in [this Repository](https://github.com/abtsoftware/scichart.wpf.examples) is covered by MIT license - meaning you can freely use our example/demo/tutorial code in your applications.
> 
> **SciChart WPF Licensing Links**
>
> - [Find out how to start a **FREE 30-day Trial** here](https://www.scichart.com/getting-started/scichart-wpf/)  
>   - Academic usage, universities and schools, non-profits and bloggers/course-writers qualify for a **free** educational license.
>   - Trial licensing is granted as-is without tech support, but we welcome feedback & bug reports via our [forums](https://www.scichart.com/questions).
> - [Read about SciChart's **commercial license terms** here](https://www.scichart.com/scichart-eula)
> - [**Purchase commercial licenses** plus technical support here](https://store.scichart.com)

&nbsp;

### Useful links

> We've prepared a short [Getting Started guide](https://scichart.com/getting-started/scichart-wpf) for SciChart WPF here.
>
> This will walk you through the entire process of getting started and show you where tutorials and documentation are and examples.
> 
> Other useful links below:
> - [Changelog for SciChart WPF](https://www.scichart.com/changelog/scichart-wpf/)
> - [MVVM Tutorials](https://www.scichart.com/documentation/win/current/webframe.html#Tutorial%2002b%20-%20Creating%20a%20SciChartSurface%20with%20MVVM.html)
> - [Code-Behind Tutorials](https://www.scichart.com/documentation/win/current/webframe.html#Tutorial%2001%20-%20Referencing%20SciChart%20DLLs.html)
> - [SciChart WPF Documentation](https://www.scichart.com/documentation/js/current/webframe.html)
> - [SciChart Community forums](https://scichart.com/questions)
> - [SciChart Stackoverflow tag](https://stackoverflow.com/tags/scichart)
> - [Contact Us (Technical support or sales)](https://scichart.com/contact-us)

















