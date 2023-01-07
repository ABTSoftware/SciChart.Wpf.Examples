using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Drawing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StrokeDashArrayPointMarkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var ds0 = new XyDataSeries<double>();
            var ds1 = new XyDataSeries<double>();
            for (int i = 0; i < 30; i++)
            {
                ds0.Append(i, Math.Sin(i * 0.1));
                ds1.Append(i, Math.Sin(i * 0.1));
            }

            sciChart0.RenderableSeries[0].DataSeries = ds0;
            sciChart1.RenderableSeries[0].DataSeries = ds0;
        }
    }

    public class CirclePointMarker2 : BitmapSpriteBase
    {
        protected override ISprite2D CreateSprite(IRenderContext2D context, Color strokeColor, Color strokeBrush)
        {
            var ellipse = new Ellipse
            {
                Width = Width,
                Height = Height,
                Fill = Brushes.Green,
                StrokeThickness = StrokeThickness,
                Stroke = new SolidColorBrush(strokeColor),
                StrokeDashArray = new DoubleCollection(new double[] { 1, 1 }),
            };

            var sprite = context.CreateSprite(ellipse);
            return sprite;
        }

        protected override void RenderToCache(IRenderContext2D context, IPen2D strokePen, IBrush2D fillBrush)
        {
            throw new NotImplementedException();
        }
    }
}
