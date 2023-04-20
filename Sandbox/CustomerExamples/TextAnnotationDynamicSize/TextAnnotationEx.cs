using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Numerics.CoordinateCalculators;
using SciChart.Charting.Visuals.Annotations;

namespace TextAnnotation_dynamic_size
{
    public class TextAnnotationEx : TextAnnotation
    {
        public override void Update(ICoordinateCalculator<double> xCoordinateCalculator, ICoordinateCalculator<double> yCoordinateCalculator)
        {
            base.Update(xCoordinateCalculator, yCoordinateCalculator);

            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this);

            bool isOutsideTop = top < 0;

            Console.WriteLine($"Update positions: ({left:0.0}, {top:0.0}) " + (isOutsideTop ? "OUTSIDE TOP" : string.Empty));
        }
    }
}