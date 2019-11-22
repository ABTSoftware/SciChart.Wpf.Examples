using System.Collections.ObjectModel;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples.AnnotationsBindingToTextBox
{
    public class MainViewModel : BindableObject
    {
        public MainViewModel()
        {
            TheAnnotation = new VerticalLineAnnotationViewModel() {X1 = 5, IsEditable = true, ShowLabel = true};
            Annotations = new ObservableCollection<BaseAnnotationViewModel>();
            Annotations.Add(TheAnnotation);
        }

        public ObservableCollection<BaseAnnotationViewModel> Annotations { get; }

        public VerticalLineAnnotationViewModel TheAnnotation { get; }
    }
}