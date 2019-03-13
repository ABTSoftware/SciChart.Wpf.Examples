using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Drawing.Common;
using Point = System.Windows.Point;

namespace SciChart.Sandbox.Examples.TimelineControl
{
    public class TimelinePointMetadata : IPointMetadata
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsSelected { get; set; }
        public Color Fill { get; set; }
    }

    public class TimelineRenderableSeries : CustomRenderableSeries  
    {
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            "Height", typeof(double), typeof(TimelineRenderableSeries), new PropertyMetadata(default(double), OnInvalidateParentSurface));

        public double Height
        {
            get { return (double) GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public static readonly DependencyProperty YOffsetProperty = DependencyProperty.Register(
            "YOffset", typeof(double), typeof(TimelineRenderableSeries), new PropertyMetadata(default(double), OnInvalidateParentSurface));

        public double YOffset
        {
            get { return (double) GetValue(YOffsetProperty); }
            set { SetValue(YOffsetProperty, value); }
        }

        public TimelineRenderableSeries()
        {
        }

        protected override void Draw(IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            base.Draw(renderContext, renderPassData);

            // Do the drawing for our timeline here
            var inputData = renderPassData.PointSeries;
            var xCalc = renderPassData.XCoordinateCalculator;
            var yCalc = renderPassData.YCoordinateCalculator;
            double yMid = YOffset;
            double halfHeight = Height * 0.5;
            var metadataCollection = DataSeries.Metadata;

            for (int i = 0; i < inputData.Count-1; i++)
            {
                double xStartCoord = xCalc.GetCoordinate(inputData.XValues[i]);
                double xEndCoord = xCalc.GetCoordinate(inputData.XValues[i + 1]);
                double yTop = yCalc.GetCoordinate(yMid + halfHeight);
                double yBottom = yCalc.GetCoordinate(yMid - halfHeight);

                var metadata = (TimelinePointMetadata)metadataCollection[inputData.Indexes[i]];
                using (var scichartBrush = renderContext.CreateBrush(metadata.Fill))
                {
                    renderContext.FillRectangle(scichartBrush, new Point(xStartCoord, yBottom), new Point(xEndCoord, yTop));
                }                
            }
        }
    }
}
