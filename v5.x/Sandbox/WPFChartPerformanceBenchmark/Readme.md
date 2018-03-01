#SCICHART WPF CHARTING PERFORMANCE BENCHMARK

This is the software used in the article [How Fast is SciChart's WPF Chart? DirectX vs. Software Comparison](https://www.scichart.com/how-fast-is-scichart-wpf-chart/). 

This article shows several test cases of SciChart's drawing performance, including:

 1. NxM series test. Displays 100 series x 100 points, then 500 x 500 and 1000 x 1000. Tests scichart under extremely high series counts
 
 ![WPF Chart Performance Test | SciChart](https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2014/12/SciChart-Performance-Comparison-Test1-NxM-Series.png)
 
 2. Scatter chart test. Displays 1k scatter points, then 10k, 100k and finally 1 Million. Tests scichart's ability to display high counts on scatter charts. 
 
 ![WPF Scatter chart performance test | SciChart](https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2014/12/SciChart-Performance-Comparison-Test2-Scatter-Series.png)
 
 3. FIFO chart test. Displays 1k, 100k, 1M and 10M (million) points in a FIFO (First in first out) scrolling strip chart. 
 
 ![WPF Chart Scrolling performance test | SciChart](https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2014/12/SciChart-Performance-Comparison-Test3-Fifo-Series.png)
 
 4. Append test. Appends to a series in blocks of 100, 1k, 10k until millions of points are observed. Tests SciChart's big-data capabilities 
 
 ![WPF Chart Appending Performance test | SciChart](https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2014/12/SciChart-Performance-Comparison-Test4-Append-Noisy100.png)


#To run the application & benchmark performance 

 1. Open the WPFChartPerformanceBenchmark-SciChart_DirectX_vs_Software.sln

 2. Right click the solution node and Enable Nuget Package Restore. This is required to download the SciChart WPF DLLs

 3. Build in Release Mode 

 4. Run without debugger attached. Maximise the window and click 'Start'
 
 The application will run through a series of tests and will report the results to a grid at the bottom. 
 
 Numeric results are FPS (Frames per second). higher is better! 
 

#Note on Performance Improvements in SciChart WPF v5.1 

SciChart WPF v5.1 has a number of performance improvements to the line and scatter series types. 

To learn more about these, and to learn how to enable them in your own applicaiton, check the following article: [Performance Improvements in SciChart WPF v5.1 ](https://www.scichart.com/performance-improvements-scichart-wpf-v5-1/)

![WPF Chart Performance](https://abtsoftware-wpengine.netdna-ssl.com/wp-content/uploads/2018/02/SciChart-WPF-Chart-5-1-Performance-Tests-Highlighted-1200x570.png)

#Troubleshooting!

We recommend performing a full Clean/Rebuild in release mode before running without debugger attached for best perf. 

Any problems just ask! support@scichart.com 