using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.SciChartMVVMBinding
{
    public class SciChartMvvmBindingsViewModel : BaseViewModel
    {
        public SciChartMvvmBindingsViewModel()
        {
            AnnotationViewModels = new ObservableCollection<IAnnotationViewModel>();
            InitializeAnnotations();

            RenderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();
            InitializeRenderableSeries();

            XAxes = new ObservableCollection<IAxisViewModel>();
            YAxes = new ObservableCollection<IAxisViewModel>();
            InitializeAxes();
        }

        private void InitializeAnnotations()
        {
            AnnotationViewModels.Add(new TextAnnotationViewModel
            {
                Text = "Annotations are Easy!",
                FontSize = 24,
                X1 = 0.3,
                Y1 = 9.7
            });

            // Text with anchor points
            AnnotationViewModels.Add(new TextAnnotationViewModel
            {
                HorizontalAnchorPoint = HorizontalAnchorPoint.Center,
                Text = "Anchor Center (X1, Y1)",
                VerticalAnchorPoint = VerticalAnchorPoint.Bottom,
                X1 = 5.0,
                Y1 = 8
            });

            AnnotationViewModels.Add(new TextAnnotationViewModel
            {
                HorizontalAnchorPoint = HorizontalAnchorPoint.Right,
                Text = "Anchor Right",
                VerticalAnchorPoint = VerticalAnchorPoint.Top,
                X1 = 5.0,
                Y1 = 8.0
            });

            AnnotationViewModels.Add(new TextAnnotationViewModel
            {
                HorizontalAnchorPoint = HorizontalAnchorPoint.Left,
                VerticalAnchorPoint = VerticalAnchorPoint.Top,
                Text = "or Anchor Left",
                X1 = 5.0,
                Y1 = 8.0
            });

            // Watermark
            AnnotationViewModels.Add(new TextAnnotationViewModel
            {
                AnnotationCanvas = AnnotationCanvas.BelowChart,
                CoordinateMode = AnnotationCoordinateMode.Relative,
                HorizontalAnchorPoint = HorizontalAnchorPoint.Center,
                Text = "Create a Watermark",
                VerticalAnchorPoint = VerticalAnchorPoint.Center,
                X1 = 0.5,
                Y1 = 0.5,
                FontSize = 56,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(101, 247, 161, 161))
            });

            AnnotationViewModels.Add(new LineAnnotationViewModel
            {
                Stroke = (Colors.Chartreuse),
                StrokeThickness = 2,
                Tooltip = "Hi, I am tooltip!",
                X1 = 1,
                X2 = 2,
                Y1 = 4,
                Y2 = 6
            });

            AnnotationViewModels.Add(new HorizontalLineAnnotationViewModel
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                LabelPlacement = LabelPlacement.TopLeft,
                LabelValue = "Right Aligned, with text on left",
                ShowLabel = true,
                Stroke = (Colors.Orange),
                StrokeThickness = 2,
                X1 = 5,
                X2 = 6,
                Y1 = 3.2,
                IsEditable = true
            });

            AnnotationViewModels.Add(new LineArrowAnnotationViewModel
            {
                Stroke = Colors.Cyan,
                StrokeThickness = 2,
                X1 = 1.2,
                X2 = 2.5,
                Y1 = 3.8,
                Y2 = 6,
                HeadWidth = 8,
                HeadLength = 4
            });

            AnnotationViewModels.Add(new VerticalLineAnnotationViewModel
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                ShowLabel = true,
                Stroke = Colors.Brown,
                LabelValue = "Vertical Line, hello everybody",
                LabelPlacement = LabelPlacement.Bottom,
                StrokeThickness = 2,
                X1 = 9,
                Y1 = 4,
                IsEditable = true
            });

            AnnotationViewModels.Add(new BoxAnnotationViewModel
            {
                Background = new SolidColorBrush(Colors.LawnGreen),
                BorderBrush = new SolidColorBrush(Colors.DarkGreen),
                BorderThickness = new Thickness(5),
                CornerRadius = new CornerRadius(3),
                X1 = 5.5,
                X2 = 7,
                Y1 = -2,
                Y2 = -5,
                IsEditable = true
            });

            AnnotationViewModels.Add(new AxisMarkerAnnotationViewModel
            {
                X1 = 4,
                Y1 = 3,
                Background = new SolidColorBrush(Colors.Red),
                IsEditable = true
            });

            AnnotationViewModels.Add(new ArrowAnnotationViewModel
            {
                X1 = 6.96,
                Y1 = 5.05,
                IsEditable = true
            });

            AnnotationViewModels.Add(new BoxAnnotationViewModel
            {
                X1 = 2,
                X2 = 5,
                Y1 = -2,
                Y2 = -4,
                IsEditable = true,
                StyleKey = "CustomBoxAnnotationStyle"
            });
        }

        private void InitializeRenderableSeries()
        {
            var renderableSeries = new  LineRenderableSeriesViewModel
            {
                Stroke = Colors.Blue, 
                StrokeThickness = 2,
                PointMarker = new EllipsePointMarker()
            };

            var dataSeries = new XyDataSeries<double>();

            renderableSeries.DataSeries = dataSeries;

            for (int i = 0; i < 11; ++i)
            {
                dataSeries.Append(i, i - 3);
            }

            RenderableSeries.Add(renderableSeries);
        }

        private void InitializeAxes()
        {
            var xNumAxis = new NumericAxisViewModel
            {
                AxisAlignment = AxisAlignment.Bottom,
                AxisTitle = "XAxis",
                DrawMajorBands = false,
                TextFormatting = "0.00#",
                VisibleRange = new DoubleRange(0, 10),
                BorderThickness = new Thickness(3),
                BorderBrush = new SolidColorBrush(Colors.CadetBlue)
            };

            XAxes.Add(xNumAxis);

            var xDateTimeAxis = new DateTimeAxisViewModel
            {
                AxisAlignment = AxisAlignment.Top,
                Id = "DateTimeAxis",
                VisibleRange = new DateRange(new DateTime(2017, 1, 1), new DateTime(2017, 1, 31)),
                StyleKey = "DateTimeAxisStyle"
            };
        
            XAxes.Add(xDateTimeAxis);

            var yNumAxis = new NumericAxisViewModel
            {
                AxisTitle = "YAxis",
                DrawMajorBands = false,
                TextFormatting = "0.0#",
                VisibleRange = new DoubleRange(-10, 10)
   
            };

            YAxes.Add(yNumAxis);

            var yTimeSpanAxis = new TimeSpanAxisViewModel
            {
                Id = "TimeSpanAxis",
                DrawMajorBands = false,
                VisibleRange = new TimeSpanRange(TimeSpan.FromHours(1), TimeSpan.FromHours(24)),
                AxisAlignment = AxisAlignment.Left
            };

            YAxes.Add(yTimeSpanAxis);
        }

        public ObservableCollection<IAnnotationViewModel> AnnotationViewModels { get; set; }
        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries { get; set; }

        public ObservableCollection<IAxisViewModel> XAxes { get; set; }
        public ObservableCollection<IAxisViewModel> YAxes { get; set; }
    }
}