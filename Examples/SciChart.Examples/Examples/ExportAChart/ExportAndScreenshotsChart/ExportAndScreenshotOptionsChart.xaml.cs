using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using Microsoft.Win32;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Core;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ExportAChart.ExportAndScreenshotsChart
{
    /// <summary>
    /// Interaction logic for ExportAndScreenshotOptionsChart.xaml
    /// </summary>
    public partial class ExportAndScreenshotOptionsChart : UserControl
    {
        public ExportAndScreenshotOptionsChart()
        {
            InitializeComponent();
            sciChart.Loaded += OnLoaded_SciChartSurface;
        }

        private void OnLoaded_SciChartSurface(object sender, RoutedEventArgs e)
        {
            // Create multiple DataSeries to store OHLC candlestick data, and Xy moving average data
            var dataSeries0 = new OhlcDataSeries<DateTime, double>() {SeriesName = "Light Blue"};
            var dataSeries1 = new XyDataSeries<DateTime, double>() { SeriesName = "Violet" };
            var dataSeries2 = new XyDataSeries<DateTime, double>() { SeriesName = "Aqua" };
            var dataSeries3 = new XyDataSeries<DateTime, double>() { SeriesName = "Rosy" };

            // Prices are in the format Time, Open, High, Low, Close (all IList)
            var prices = DataManager.Instance.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);

            // Append data to series. 
            // First series is rendered as a Candlestick Chart so we append OHLC data
            dataSeries0.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);

            // Subsequent series append moving average of the close prices
            dataSeries1.Append(prices.TimeData, DataManager.Instance.ComputeMovingAverage(prices.CloseData, 100));
            dataSeries2.Append(prices.TimeData, DataManager.Instance.ComputeMovingAverage(prices.CloseData, 50));
            dataSeries3.Append(prices.TimeData, DataManager.Instance.ComputeMovingAverage(prices.CloseData, 20));

            sciChart.RenderableSeries[0].DataSeries = dataSeries0;
            sciChart.RenderableSeries[1].DataSeries = dataSeries1;
            sciChart.RenderableSeries[2].DataSeries = dataSeries2;
            sciChart.RenderableSeries[3].DataSeries = dataSeries3;

            boxAnnotation.X1 = new DateTime(2011, 7, 21, 17, 2, 39);
            boxAnnotation.X2 = new DateTime(2011, 3, 16, 6, 44, 18);

            buyTxtAnnot.X1 = new DateTime(2011, 3, 14, 0, 24, 11);
            buyTxtAnnot.X2 = buyTxtAnnot.X1;

            sellTxtAnnot.X1 = new DateTime(2011, 7, 15, 21, 19, 28);
            sellTxtAnnot.X2 = sellTxtAnnot.X1;

            sciChart.ZoomExtents();
        }

        private void ExportToXPS(object sender, RoutedEventArgs e)
        {
            string filePath;
            if(GetAndCheckPath("XPS | *.xps", out filePath))
            {
                sciChart.ExportToFile(filePath, ExportType.Xps, true);
            }
        }

        private void ExportToXPSBig(object sender, RoutedEventArgs e)
        {    
            string filePath;
            if(GetAndCheckPath("XPS | *.xps", out filePath))
            {
                sciChart.ExportToFile(filePath, ExportType.Xps, true, new Size(2000, 2000));
            }
        }

        private void ExportToPng(object sender, RoutedEventArgs e)
        {
            string filePath;
            if(GetAndCheckPath("PNG | *.png", out filePath))
            {
                sciChart.ExportToFile(filePath, ExportType.Png, false);
            }
        }

        private void ExportPngInMemory(object sender, RoutedEventArgs e)
        {
            string filePath;
            if(GetAndCheckPath("PNG | *.png", out filePath))
            {
                sciChart.ExportToFile(filePath, ExportType.Png, false, new Size(2000, 1500));
            }
        }

        public SaveFileDialog CreateFileDialog(string filter)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            return saveFileDialog;
        }

        private void OnPrintClick(object sender, RoutedEventArgs e)
        {
            sciChart.Print();
        }

        private bool GetAndCheckPath(string filter, out string path)
        {
            var ret = false;
            var isGoodPath = false;
            var saveFileDialog = CreateFileDialog(filter);
            path = null;

            while (!isGoodPath)
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (IsFileGoodForWriting(saveFileDialog.FileName))
                    {
                        path = saveFileDialog.FileName;
                        isGoodPath = true;
                        ret = true;
                    }
                    else
                    {
                        MessageBox.Show(
                            "File is inaccesible for writing or you can not create file in this location, please choose another one.");
                    }
                }
                else
                {
                    isGoodPath = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// Check if file is Good for writing
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns></returns>
        public static bool IsFileGoodForWriting(string filePath)
        {
            FileStream stream = null;
            FileInfo file = new FileInfo(filePath);

            try
            {
                stream = file.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
            }
            catch (Exception)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return true;
        }
    }
}
