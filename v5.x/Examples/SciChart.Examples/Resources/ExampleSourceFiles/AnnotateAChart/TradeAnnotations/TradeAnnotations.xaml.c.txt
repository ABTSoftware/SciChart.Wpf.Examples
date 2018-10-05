using System.Windows;
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
            ManipulationMargins.AnnotationLineWidth = 20;
        }

        // code for dragging the thumb on the toolbar. Not related to functionality of scichart 
        private void Thumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = e.Source as Thumb;
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                var horiz = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var vertic = Canvas.GetTop(thumb) + e.VerticalChange;

                var translatePoint = this.TranslatePoint(new System.Windows.Point(horiz, vertic), mainWindow);
                if (translatePoint.X > 0 && translatePoint.X + thumb.ActualWidth + 16 <= mainWindow.ActualWidth)
                {
                    Canvas.SetLeft(thumb, horiz);
                }
                else if (translatePoint.X < 0)
                {
                    Canvas.SetLeft(thumb, 0);
                }
                else if (translatePoint.X + thumb.ActualWidth + 16 > mainWindow.ActualWidth)
                {
                    Canvas.SetLeft(thumb, mainWindow.ActualWidth - 16 - thumb.ActualWidth);
                }

                if (translatePoint.Y >= 0 && translatePoint.Y + thumb.ActualHeight + 69 <= mainWindow.ActualHeight)
                {
                    Canvas.SetTop(thumb, vertic);
                }
                else if (translatePoint.Y < 0)
                {
                    Canvas.SetTop(thumb, mainWindow.TranslatePoint(new System.Windows.Point(0, 0), this).Y);
                }
                else if (translatePoint.Y > mainWindow.ActualHeight - 69)
                {
                    Canvas.SetTop(thumb,
                        mainWindow.TranslatePoint(
                            new System.Windows.Point(0, mainWindow.ActualHeight - 69 - thumb.ActualHeight), this).Y);
                }
            }
        }
    }
}