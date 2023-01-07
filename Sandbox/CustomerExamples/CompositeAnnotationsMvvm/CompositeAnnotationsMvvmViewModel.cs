using System.Collections.ObjectModel;
using SciChart.Charting;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Data.Model;

namespace CompositeAnnotationsMvvmExample
{
    public class CompositeAnnotationsMvvmViewModel : BindableObject
    {
        public CompositeAnnotationsMvvmViewModel()
        {
            Annotations = new ObservableCollection<IAnnotationViewModel>();

            var measAnnotation = new RangeXAnnotationViewModel
            {
                X1 = 5,
                X2 = 7,
                Y1 = 0,
                Y2 = 1,
                CoordinateMode = AnnotationCoordinateMode.RelativeY,
                IsEditable = true,
                IsSelected = true,
                DragDirections = XyDirection.XDirection,
                ResizeDirections = XyDirection.XDirection,
            };
            Annotations.Add(measAnnotation);
        }   

        public ObservableCollection<IAnnotationViewModel> Annotations { get; }
    }
}