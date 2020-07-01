using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Data.Model;

namespace CustomSeriesMvvmExample
{
    public class MainViewModel : BindableObject
    {
        private Random random = new Random();
        private readonly ObservableCollection<IAnnotationViewModel> _verticalLines = new ObservableCollection<IAnnotationViewModel>();

        public MainViewModel()
        {
            _verticalLines.Add(new VerticalLineAnnotationViewModel() { X1 = 2, ShowLabel = true});
            _verticalLines.Add(new VerticalLineAnnotationViewModel() { X1 = 5, ShowLabel = true});
        }

        public ObservableCollection<IAnnotationViewModel> VerticalLines => _verticalLines;

        public ICommand AddLinesCommand
        {
            get
            {
                return new ActionCommand(() => _verticalLines.Add(new VerticalLineAnnotationViewModel() { X1 = random.NextDouble() * 10, ShowLabel = true }));
            }
        }

        public ICommand ClearLinesCommand
        {
            get
            {
                return new ActionCommand(() => _verticalLines.Clear());
            }
        }
    }
}
