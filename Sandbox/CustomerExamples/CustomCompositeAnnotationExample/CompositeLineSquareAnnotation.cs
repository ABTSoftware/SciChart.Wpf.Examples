using System.Linq;
using System.Windows;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Events;
using SciChart.Core.Utility.Mouse;

namespace CustomCompositeAnnotationExampleExample
{
    public class CompositeLineSquareAnnotation : CompositeAnnotation
    {
        private Point[] _startDragAnnotationsMousePoints;

        public CompositeLineSquareAnnotation()
        {
            var firstLine = new LineAnnotation();
            var secondLine = new LineAnnotation();
            var thirdLine = new LineAnnotation();
            var forthLine = new LineAnnotation();

            Annotations.Add(firstLine);
            Annotations.Add(secondLine);
            Annotations.Add(thirdLine);
            Annotations.Add(forthLine);

            BorderThickness = new Thickness(0);
        }

        /// <summary>
        /// Updates the position and values of the annotation during a drag or resize operation, by setting X1,Y1,X2,Y2 and X,Y pixel coordinates together, 
        /// from a pixel coordinate input
        /// </summary>
        /// <param name="point1">The first input pixel coordinate</param>
        /// <param name="point2">The second input pixel coordinate</param>
        public override void UpdatePosition(Point point1, Point point2)
        {
            SetBasePoint(point1, 0, XAxis, YAxis);
            SetBasePoint(point2, 2, XAxis, YAxis);
            SetBasePoint(new Point(point2.X, point1.Y), 1, XAxis, YAxis);
            SetBasePoint(new Point(point1.X, point2.Y), 3, XAxis, YAxis);
        }

        /// <summary>
        /// This method is used in internally by the <see cref="AnnotationResizeAdorner" />. Gets the adorner point positions
        /// </summary>
        /// <param name="coordinates">The previously calculated <see cref="AnnotationCoordinates"/> in screen pixels.</param>
        /// <returns>A list of points in screen pixels denoting the Adorner corners</returns>
        protected override Point[] GetBasePoints(AnnotationCoordinates coordinates)
        {
            var lineAnnotations = Annotations;
            var count = lineAnnotations.Count();

            var points = new Point[count];

            for (int i = 0; i < count; i++)
            {
                var annotation = lineAnnotations[i];

                var xCoord = XAxis.GetCoordinate(annotation.X1);
                var yCoord = YAxis.GetCoordinate(annotation.Y1);

                points[i] = new Point(xCoord, yCoord);
            }

            return points;
        }

        /// <summary>
        /// Called internally to marshal pixel points to X1,Y1,X2,Y2 values. 
        /// Taking a pixel point (<paramref name="newPoint"/>) and base point <paramref name="index"/>, sets the X,Y data-values. 
        /// </summary>
        /// <param name="newPoint">The pixel point</param>
        /// <param name="index">The base point index, where 0, 1, 2, 3 refer to the four corners of an Annotation</param>
        /// <param name="yAxis">The current Y-Axis</param>
        /// <param name="xAxis">The current X-Axis </param>
        protected override void SetBasePoint(Point newPoint, int index, IAxis xAxis, IAxis yAxis)
        {
            var xValue = XAxis.GetDataValue(newPoint.X);
            var yValue = YAxis.GetDataValue(newPoint.Y);

            var lineAnnotations = Annotations;

            if (index == 0)
            {
                lineAnnotations[0].X1 = xValue;
                lineAnnotations[0].Y1 = yValue;

                lineAnnotations[3].X2 = xValue;
                lineAnnotations[3].Y2 = yValue;
            }
            else if (index == 1)
            {
                lineAnnotations[1].X1 = xValue;
                lineAnnotations[1].Y1 = yValue;

                lineAnnotations[0].X2 = xValue;
                lineAnnotations[0].Y2 = yValue;
            }
            else if (index == 2)
            {
                lineAnnotations[2].X1 = xValue;
                lineAnnotations[2].Y1 = yValue;

                lineAnnotations[1].X2 = xValue;
                lineAnnotations[1].Y2 = yValue;
            }
            else
            {
                lineAnnotations[3].X1 = xValue;
                lineAnnotations[3].Y1 = yValue;

                lineAnnotations[2].X2 = xValue;
                lineAnnotations[2].Y2 = yValue;
            }
        }

        ///<inheritdoc/> 
        protected override void OnAnnotationPointerPressed(ModifierMouseArgs e)
        {
            _startDragAnnotationsMousePoints = GetBasePoints(SavedCoordinates);

            base.OnAnnotationPointerPressed(e);
        }

        /// <summary>
        /// Called internally to marshal pixel points to X1,Y1,X2,Y2 values. 
        /// Taking a pixel point (<paramref name="newPoint"/>) and base point <paramref name="index"/>, sets the X,Y data-values. 
        /// </summary>
        protected override void MoveBasePointTo(Point newPoint, int index, IAxis xAxis, IAxis yAxis)
        {
            SetBasePoint(newPoint, index, xAxis, yAxis);
        }

        /// <summary>
        /// Moves the annotation to a specific horizontal and vertical offset
        /// </summary>
        /// <param name="coordinates">The initial coordinates.</param>
        /// <param name="horizOffset">The horizontal offset.</param>
        /// <param name="vertOffset">The vertical offset.</param>
        protected override void MoveAnnotationTo(AnnotationCoordinates coordinates, double horizOffset,
            double vertOffset)
        {
            var pointsCoords = _startDragAnnotationsMousePoints;

            for (int i = 0; i < pointsCoords.Length; i++)
            {
                var point = pointsCoords[i];

                point.X += horizOffset;
                point.Y += vertOffset;

                MoveBasePointTo(point, i, XAxis, YAxis);
            }

            OnAnnotationDragging(new AnnotationDragDeltaEventArgs(horizOffset, vertOffset));
        }
    }
}
