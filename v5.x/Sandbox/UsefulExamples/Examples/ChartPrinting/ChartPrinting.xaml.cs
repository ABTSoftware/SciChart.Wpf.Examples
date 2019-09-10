using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SciChart.Charting;
using SciChart.Charting.Model;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core;

namespace SciChart.Sandbox.Examples.ChartPrinting
{
    /// <summary>
    /// Interaction logic for ChartPrinting.xaml
    /// </summary>
    [TestCase("Chart Printing Tests")]
    public partial class ChartPrinting : Window
    {
        private RenderTargetBitmap _renderTargetBitmap;
        private Random random = new Random();

        public ChartPrinting()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataSeries = GetDataSeries();

            lineRenderSeries.DataSeries = dataSeries;

            surface.RenderableSeries[0].DataSeries = GetDataSeries("Line 1", 1);
            surface.RenderableSeries[1].DataSeries = GetDataSeries("Line 2", 2);
            surface.RenderableSeries[2].DataSeries = GetDataSeries("Line 2", 3);
        }

        private  XyDataSeries<double, double> GetDataSeries(string name,int coef)
        {
            var dataSeries = new XyDataSeries<double, double>(){SeriesName = name};

            var data = Enumerable.Range(0, 101).Select(x => new Tuple<double, double>(x, x*coef));

            dataSeries.Append(data.Select(x=>x.Item1),data.Select(x=>x.Item2));

            return dataSeries;
        }

        private SciChartSurface Surface { get { return InMemoryCheckBox.IsChecked.Value ? CreateSurface() : sciChart; } }
        private XyDataSeries<double, double> GetDataSeries()
        {
            var dataSeries = new XyDataSeries<double, double>();
            
            for (int i = 0; i < 100; i++)
            {
                dataSeries.Append(i, random.Next(i - 10, i + 10));
            }

            return dataSeries;
        }

        private SciChartSurface CreateSurface()
        {
            var series = new FastLineRenderableSeries()
            {
                Stroke = Colors.Red,
                DataSeries = GetDataSeries()
            };

            var xAxes = new AxisCollection()
            {
                new NumericAxis()
            };

            var yAxes = new AxisCollection()
            {
                new NumericAxis()
            };

            var surface = new SciChartSurface()
            {
                ChartTitle = "Rendered In Memory",
                XAxes = xAxes,
                YAxes = yAxes,
                RenderableSeries = new ObservableCollection<IRenderableSeries>() { series },
                Annotations =
                    new AnnotationCollection()
                    {
                        new BoxAnnotation()
                        {
                            X1 = 10,
                            X2 = 30,
                            Y1 = 10,
                            Y2 = 40,
                            Background = new SolidColorBrush(Colors.Green),
                        },
                        new VerticalLineAnnotation()
                        {
                            X1 = 35,
                            Stroke = new SolidColorBrush(Colors.Yellow),
                            StrokeThickness = 3,
                            ShowLabel = true,
                            LabelPlacement = LabelPlacement.Axis
                        }
                    }
            };

            ThemeManager.SetTheme(surface, "Chrome");

            surface.Width = 800;
            surface.Height = 400;

            return surface;
        }

        private void ExportToBitmap_OnClick(object sender, RoutedEventArgs e)
        {
            ChartImage.Source = Surface.ExportToBitmapSource();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Png | *.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                Surface.ExportToFile(saveFileDialog.FileName, ExportType.Png, false);
            }      
        }

        private void XpsPrinting(object sender, RoutedEventArgs e)
        {
            Surface.Print();
        }
    }
}
