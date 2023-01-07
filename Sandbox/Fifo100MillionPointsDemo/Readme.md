# Fifo 100 Million Points Demo

This demo showcases SciChart WPF v6's incredible performance when rendering streaming (scrolling) line charts.
 
![enter image description here](https://raw.githubusercontent.com/ABTSoftware/SciChart.Wpf.Examples/SciChart_v6_Release/Sandbox/Fifo100MillionPointsDemo/fifo-1-billion-points-demo.PNG)

To run the demo, ensure that you have setup the NuGet package feed below. Compilation requires Visual Studio 2019 or later. 

## NuGet Package Feed setup

In order to build the application, you will need to add our private NuGet feed for BETA packages. 

* In Visual Studio, go to Tools -> Options -> NuGet 
*  Under Package Sources, add a feed called SciChart Bleeding-Edge
* Add this URL to the nuget feed https://www.myget.org/F/abtsoftware-bleeding-edge/api/v3/index.json 

When you build, Visual Studio will now automatically restore NuGet packages. For more information see our page [Getting Nightly Builds with NuGet](https://support.scichart.com/index.php?/Knowledgebase/Article/View/17232/37/getting-nightly-builds-with-nuget).

## Running the Application 

For best performance, run the application in Release Mode without debugger attached. Debug mode will result in a 50% slowdown, and the debugger adds (requires) extra memory and CPU to run the application. 

## Hardware Requirements

We recommend 16 GB RAM and a DirectX11 capable GPU. The sample will automatically limit point-counts based on your system RAM (4GB RAM is requred for 100M points and 16GB or more for 500M)

Our tests were performed on a system with nVidia 1070 GTX / 32GB RAM and we get a stable 30 FPS at 100,000,000 points.  

Customers with [AVX SIMD capable Processors](https://en.wikipedia.org/wiki/Advanced_Vector_Extensions#CPUs_with_AVX) (Intel Sandy Bridge or later) will see 2-3x faster results than those without, as we make use of Streaming Vector Extensions on the CPU.
