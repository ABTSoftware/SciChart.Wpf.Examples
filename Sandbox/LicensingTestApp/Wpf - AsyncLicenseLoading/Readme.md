# SCICHART ASYNC LICENSING LOADING

This is a troubleshooting application for applying licenses to SciChart WPF Charts in WPF applications

**This version uses Async License Loading and a splash screen to demonstrate how to preload libraries before showing a SciChartSurface**

For more information, see the [SciChart Licensing Troubleshooting](https://www.scichart.com/scichart-licensing-troubleshooting/) page. 


### How to pre-load SciChart native libraries and async load licensing before a SciChartSurface is shown

Some customers have reported slow initial startup time with SciChart and asked for a way to do this initialization earlier in the application, e.g. when a splashscreen is shown. 

This sample shows how to do this using a new feature in SciChart WPF v6.1: 

 - SciChart2DInitializer.LoadLibrariesAndLicenseAsync
 - SciChart2D3DInitializer.LoadLibrariesAndLicenseAsync

### Usage 

 1. App.xaml.cs overrides OnStartup and calls SciChart2D3DInitializer.LoadLibrariesAndLicenseAsync
 2. MainWindow.xaml initially shows some loading content, no SciChartSurfaces are shown initially
 3. MainWindow.Loaded event handler awaits SciChart2D3DInitializer.Awaiter before swapping the content for SciChartSurfaces

In essence, it doesnt matter how you do it, just make sure you await the result of SciChart2D3DInitializer.LoadLibrariesAndLicenseAsync or SciChart2D3DInitializer.Awaiter (its the same task) before any SciChartSurface is called. 

This method is fully thread safe, it can be called as many times as you like, but will apply license and init only once. 

**Note**
 - SciChart2D3DInitializer initializes both 2D & 3D libraries. 
 - SciChart2DInitializer initializes just 2D 


### Steps to Troubleshoot Licenses

If you experience problems with trial expired warning after applying a license, try these troubleshooting steps
 
 1. Download this Licensing Test App and unzip it to your computer.
 2. Enter your Runtime License key from your Profile page at [www.scichart.com/profile](https://www.scichart.com/profile/) in App.xaml.cs & compile the Licensing Test App
 3. Run the Licensing Test App on the other computer where you are experiencing problems. Make sure this is a computer without an activated developer license. This is because the Activated Developer license will interfere with this test.
		- **Expected Result:** You should not see a ‘Powered by SciChart’ watermark or a trial expired when running the Licensing Test App.
		- If you do, contact us, you’ve probably found a bug 🙂
 4. Debug the Licensing Test App on another computer.Make sure this is a computer without an activated developer license. This is because the Activated Developer license will interfere with this test.
		- **Expected Result:** You should see a ‘Powered by SciChart’ watermark or a trial expired notice.
		- This is expected behaviour as SciChart has detected you are running inside Visual Studio and is searching for a developer license.
		
If either expectation above fails, please contact tech support and we will be glad to assist. Please tell us what you tried, and what version of SciChart you are using.

If the expectations above pass, it is likely that the Runtime key has been set incorrectly in your main application. Please double check you have set the Runtime Key once and once only in your app. It should be set before any SciChartSurface instances are created. A good guide to do this is our FAQ on [Licensing in a DLL](https://www.scichart.com/questions/question/license-in-dll).



