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
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Drawing.Common;
using Point = System.Windows.Point;

namespace SciChart.Sandbox.Examples.TimelineControl
{
    /// <summary>
    /// A series which displays a timeline of different colours. This series will display from left to right, showing blocks of colour according to the following
    /// values in an XyzDataSeries
    ///
    /// X-value: the start point of the block
    /// Y-value: used as the length of the block in the X-Axis
    /// Z-value: used as the color of the block
    ///
    /// NOTE The height of the block is defined by properties YOffset and Height which must be set in XAML or code before this chart will draw
    /// </summary>
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

        protected override void Draw(IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            base.Draw(renderContext, renderPassData);

            // Do the drawing for our timeline here
            // 

            // Input data is type XyzPointSeries so must be cast
            var inputData = renderPassData.PointSeries as XyzPointSeries;

            // Get X,Y axis calculators 
            var xCalc = renderPassData.XCoordinateCalculator;
            var yCalc = renderPassData.YCoordinateCalculator;

            // Compute some constants
            double yMid = YOffset;
            double halfHeight = Height * 0.5;
            
            // Iterate over the data 
            for (int i = 0; i < inputData.Count; i++)
            {
                // Now compute the bounds of the box to draw for one data-points
                // XStart = X, XEnd = XStart + Y (we use Y for the length of the box) 
                // YTop, YBottom defined by Height + YOffset properties
                double xStartCoord = xCalc.GetCoordinate(inputData.XValues[i]);
                double xEndCoord = xStartCoord + xCalc.GetCoordinate(inputData.YValues[i]);
                double yTop = yCalc.GetCoordinate(yMid + halfHeight);
                double yBottom = yCalc.GetCoordinate(yMid - halfHeight);

                // Get the color for this block
                int iColor = (int)inputData.ZPoints[i];
                Color fill = iColor.ToColor();

                // Brush creation can be expensive, cache brushes keyed on int color if you find this
                using (var scichartBrush = renderContext.CreateBrush(fill))
                {
                    // Draw a rectangle 
                    renderContext.FillRectangle(scichartBrush, new Point(xStartCoord, yBottom), new Point(xEndCoord, yTop));

                    // NOTE: 
                    // You can draw fill, stroke, and fills can be linear gradient brushes if you want. 
                    // Extra data can be passed through the Metadata parameter in XyzDataSeries
                }                
            }
        }

        public override IRange GetXRange()
        {
            if (DataSeries == null || DataSeries.Count == 0)
            {
                return base.GetXRange();
            }

            // Override the XRange calculation for this series based on the 'width' of the last point remembering that Width = Y
            var xOut = new DoubleRange((double)DataSeries.XValues[0], 
                (double)DataSeries.XValues[DataSeries.Count - 1] + (double)DataSeries.YValues[DataSeries.Count - 1]);
            return xOut;
        }
    }
}
