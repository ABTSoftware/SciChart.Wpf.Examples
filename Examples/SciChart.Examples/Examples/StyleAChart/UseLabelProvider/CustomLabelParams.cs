using System.Windows.Media;

namespace SciChart.Examples.Examples.StyleAChart.UseLabelProvider
{
    public class CustomLabelParams
    {
        public Color ColorFrom { get; set; }
        public Color ColorTo { get; set; }
        
        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }

        public double LabelAngle { get; set; }
        public string LabelFormat { get; set; }
    }
}