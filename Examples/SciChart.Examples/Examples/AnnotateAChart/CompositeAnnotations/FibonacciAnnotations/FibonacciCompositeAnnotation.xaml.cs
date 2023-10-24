using System;
using System.Windows.Media;
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.FibonacciAnnotations
{
    public partial class FibonacciCompositeAnnotation : CompositeAnnotation
    {
        public FibonacciCompositeAnnotation()
        {
            InitializeComponent();
        }

        protected void CreateAnnotations(params RatioModel[] ratios)
        {
            Annotations.Add(CreateFibonacciRatioLine(ratios[0].Value, ratios[0].Brush));

            for (int i = 1; i < ratios.Length; i++)
            {
                Annotations.Add(CreateFibonacciRatioBox(ratios[i - 1].Value, ratios[i].Value, ratios[i].Brush));
                Annotations.Add(CreateFibonacciRatioLine(ratios[i].Value, ratios[i].Brush));
            }

            Annotations.Add(CreateFibonacciTrendLine(new DoubleCollection { 2d, 4d }, 2d, new SolidColorBrush(Color.FromArgb(0xFF, 0xBA, 0xBA, 0xBA))));
        }

        private IAnnotation CreateFibonacciRatioLine(double y1, Brush stroke)
        {
            return new FibonacciRatioLine
            {
                Y1 = y1,
                Stroke = stroke,
                LabelValue = string.Format("{0:#0.##%}", y1 >= 0 && y1 <= 1 ? 1 - y1 : Math.Abs(y1 - 1))
            };
        }

        private IAnnotation CreateFibonacciRatioBox(double y1, double y2, Brush background)
        {
            return new FibonacciRatioBox
            {
                Y1 = y1,
                Y2 = y2,
                Background = background
            };
        }

        private IAnnotation CreateFibonacciTrendLine(DoubleCollection strokeDashArray, double strokeThickness, Brush stroke)
        {
            return new FibonacciTrendLine
            {
                Stroke = stroke,
                StrokeThickness = strokeThickness,
                StrokeDashArray = strokeDashArray
            };
        }
    }
}