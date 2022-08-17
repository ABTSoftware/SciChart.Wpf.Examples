using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core;
using SciChart.Data.Model;

namespace OffScreenExportExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OffScreenExportView : Window
    {
        public OffScreenExportView()
        {
            InitializeComponent();
        }

        private void OnExportSingleChart_Clicked(object sender, RoutedEventArgs e)
        {
            // Create SciChartSurface
            var surface = new SciChartSurface
            {
                XAxis = new NumericAxis { GrowBy = new DoubleRange(0.1, 0.1) },
                YAxis = new NumericAxis { GrowBy = new DoubleRange(0.1, 0.1) },

                // Add RenderableSerieses
                RenderableSeries = new ObservableCollection<IRenderableSeries>
                {
                    new FastLineRenderableSeries
                    {
                        DataSeries = GetLineDataseries(0.1, 0.1),
                        Stroke = Colors.DarkGreen,
                        StrokeThickness = 2,
                    }
                },

                // Add Annotations
                Annotations = new AnnotationCollection()
                {
                    new BoxAnnotation()
                    {
                        X1 = 2,
                        X2 = 8,
                        Y1 = 0.1,
                        Y2 = 0.3,
                        Background = new SolidColorBrush(Colors.Red),
                    },

                    new VerticalLineAnnotation()
                    {
                        X1 = 3,
                        Stroke = new SolidColorBrush(Colors.Yellow),
                        StrokeThickness = 3,
                        ShowLabel = true,
                        LabelPlacement = LabelPlacement.Axis
                    }
                }
            };

            // Set Theme
            ThemeManager.SetTheme(surface, "Chrome");

            // When rendering in memory without showing, we must specify a size
            // WPF has no idea what the size of the chart should be unless it
            // is hosted in a Window
            surface.Width = 460;
            surface.Height = 460;

            // Perform export
            var path = $"{FolderTxtblck.Text}/singleOffscreen_0.png";
            surface.ExportToFile(path, ExportType.Png, false);
        }

        private void OnExportMultipleCharts_Clicked(object sender, RoutedEventArgs e)
        {
            var counter = 0;

            // Create SciChartSurface
            var surfacesCollection = GetMultippleCharts();
            foreach (var surface in surfacesCollection)
            {
                // When rendering in memory without showing, we must specify a size
                // WPF has no idea what the size of the chart should be unless it
                // is hosted in a Window
                surface.Width = 460;
                surface.Height = 460;

                // Perform export
                var path = $"{FolderTxtblck.Text}/multipleOffscreen_{counter}.png";
                surface.ExportToFile(path, ExportType.Png, false);
                counter++;
            }
        }

        private void OnExportWithCloning_Clicked(object sender, RoutedEventArgs e)
        {
            var surface = new SciChartSurface
            {
                XAxis = new NumericAxis { GrowBy = new DoubleRange(0.1, 0.1) },
                YAxis = new NumericAxis { GrowBy = new DoubleRange(0.1, 0.1) },

                // Add RenderableSerieses
                RenderableSeries = new ObservableCollection<IRenderableSeries>
                {
                    new FastLineRenderableSeries
                    {
                        DataSeries = GetLineDataseries(0.1, 0.1),
                        Stroke = Colors.DarkGreen,
                        StrokeThickness = 2,
                    }
                },

                // Add Annotations
                Annotations = new AnnotationCollection()
                {
                    new BoxAnnotation()
                    {
                        X1 = 2,
                        X2 = 8,
                        Y1 = 0.1,
                        Y2 = 0.3,
                        Background = new SolidColorBrush(Colors.Red),
                    },

                    new VerticalLineAnnotation()
                    {
                        X1 = 3,
                        Stroke = new SolidColorBrush(Colors.Yellow),
                        StrokeThickness = 3,
                        ShowLabel = true,
                        LabelPlacement = LabelPlacement.Axis
                    }
                }
            };

            // Set Theme
            ThemeManager.SetTheme(surface, "Chrome");

            // When rendering in memory without showing, we must specify a size
            // WPF has no idea what the size of the chart should be unless it
            // is hosted in a Window
            surface.Width = 460;
            surface.Height = 460;

            // Perform export
            var path = $"{FolderTxtblck.Text}/cloningOffscreen.png";
            surface.ExportToFile(path, ExportType.Png, false, new Size(1200, 1200));
        }

        private IEnumerable<SciChartSurface> GetMultippleCharts()
        {
            var collectionToReturn = new List<SciChartSurface>();

            Color[] colors = 
            {
                Color.FromRgb(215, 248, 173),
                Color.FromRgb(223, 183, 61),
                Color.FromRgb(45, 192, 85),
                Color.FromRgb(77, 59, 169),
                Color.FromRgb(55, 214, 229),
            };

            for (var i = 0; i < 5; i++)
            {
                var surface = new SciChartSurface
                {
                    XAxis = new NumericAxis { GrowBy = new DoubleRange(0.1, 0.1) },
                    YAxis = new NumericAxis { GrowBy = new DoubleRange(0.1, 0.1) },

                    // Add RenderableSerieses
                    RenderableSeries = new ObservableCollection<IRenderableSeries>
                    {
                        new FastLineRenderableSeries
                        {
                            DataSeries = GetLineDataseries(0.1, 0.1, 3000),
                            Stroke = colors[i],
                            StrokeThickness = 8
                        }
                    },
                    // Add Annotations
                    Annotations = new AnnotationCollection()
                    {
                        new BoxAnnotation()
                        {
                            X1 = 2,
                            X2 = 8,
                            Y1 = 0.1,
                            Y2 = 0.3,
                            Background = new SolidColorBrush(Colors.Red),
                        },

                        new VerticalLineAnnotation()
                        {
                            X1 = 3,
                            Stroke = new SolidColorBrush(Colors.Yellow),
                            StrokeThickness = 3,
                            ShowLabel = true,
                            LabelPlacement = LabelPlacement.Axis
                        }
                    },
                };

                collectionToReturn.Add(surface);
            }

            return collectionToReturn;
        }

        private XyDataSeries<double, double> GetLineDataseries(double amplitude, double phaseShift, int count = 5000)
        {
            var dataSeries = new XyDataSeries<double,double>();
            for (int i = 0; i < count; i++)
            {
                var xValue = 10 * i / (double)count;
                var wn = 2 * Math.PI / (count / 10);

                var yValue = Math.PI * amplitude *
                            (Math.Sin(i * wn + phaseShift) +
                             0.33 * Math.Sin(i * 3 * wn + phaseShift) +
                             0.20 * Math.Sin(i * 5 * wn + phaseShift) +
                             0.14 * Math.Sin(i * 7 * wn + phaseShift) +
                             0.11 * Math.Sin(i * 9 * wn + phaseShift) +
                             0.09 * Math.Sin(i * 11 * wn + phaseShift));
                dataSeries.Append(xValue, yValue);
            }

            return dataSeries;
        }

        private void OnPickFolder_Clicked(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FolderTxtblck.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
