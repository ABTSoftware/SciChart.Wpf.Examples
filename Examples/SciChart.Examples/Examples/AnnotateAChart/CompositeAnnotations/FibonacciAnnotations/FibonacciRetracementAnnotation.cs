using System.Windows.Media;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.FibonacciAnnotations
{
    public class FibonacciRetracementAnnotation : FibonacciCompositeAnnotation
    {
        public FibonacciRetracementAnnotation()
        {
            CreateAnnotations(new RatioModel(0d, new SolidColorBrush(Color.FromArgb(0xFF, 0x87, 0x77, 0x77))),
                new RatioModel(0.236, new SolidColorBrush(Color.FromArgb(0xFF, 0xC7, 0x38, 0x28))),
                new RatioModel(0.382, new SolidColorBrush(Color.FromArgb(0xFF, 0x8A, 0xCC, 0x28))),
                new RatioModel(0.5, new SolidColorBrush(Color.FromArgb(0xFF, 0x28, 0xCC, 0x33))),
                new RatioModel(0.618, new SolidColorBrush(Color.FromArgb(0xFF, 0x28, 0xC7, 0x9A))),
                new RatioModel(0.764, new SolidColorBrush(Color.FromArgb(0xFF, 0x31, 0x93, 0xC5))),
                new RatioModel(1d, new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0x77, 0x87))));
        }
    }
}