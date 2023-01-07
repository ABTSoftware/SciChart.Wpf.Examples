using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.Examples.AnnotateAChart.TradeAnnotations
{
    /// <summary>
    /// Interaction logic for TradeAnnotations.xaml
    /// </summary>
    public partial class TradeAnnotations : UserControl
    {
        public TradeAnnotations()
        {
            InitializeComponent();
            ManipulationMargins.AnnotationLineWidth = 20d;
        }

        // Code for dragging the thumb on the toolbar. Not related to functionality of SciChart 
        private void Thumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (e.Source is Thumb thumb && thumb.Parent is Canvas canvas)
            {
                var left = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var top = Canvas.GetTop(thumb) + e.VerticalChange;

                if (left <= 0d)
                {
                    Canvas.SetLeft(thumb, 0d);
                }
                else if (left + thumb.ActualWidth <= canvas.ActualWidth)
                {
                    Canvas.SetLeft(thumb, left);
                }

                if (top <= 0d)
                {
                    Canvas.SetTop(thumb, 0d);
                }
                else if (top + thumb.ActualHeight <= canvas.ActualHeight)
                {
                    Canvas.SetTop(thumb, top);
                }
            }
        }
    }
}