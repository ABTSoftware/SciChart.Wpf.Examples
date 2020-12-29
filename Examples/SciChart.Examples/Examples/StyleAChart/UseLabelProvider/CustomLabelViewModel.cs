using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace SciChart.Examples.Examples.StyleAChart.UseLabelProvider
{
    public class CustomLabelViewModel : DefaultTickLabelViewModel
    {
        private double _fontSize;
        private FontWeight _fontWeight;

        private double _angle;
        private SolidColorBrush _foreground;

        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        public FontWeight FontWeight
        {
            get => _fontWeight;
            set
            {
                _fontWeight = value;
                OnPropertyChanged(nameof(FontWeight));
            }
        }

        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                OnPropertyChanged(nameof(Angle));
            }
        }

        public SolidColorBrush Foreground
        {
            get => _foreground;
            set
            {
                _foreground = value;
                OnPropertyChanged(nameof(Foreground));
            }
        }
    }
}