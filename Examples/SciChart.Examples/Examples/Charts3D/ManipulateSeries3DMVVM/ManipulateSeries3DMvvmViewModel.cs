using System.Collections.ObjectModel;
using System.Linq;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Common;


namespace SciChart.Examples.Examples.Charts3D.ManipulateSeries3DMVVM
{
    public class ManipulateSeries3DMvvmViewModel : BaseViewModel
    {
        private string _selectedType;

        public ManipulateSeries3DMvvmViewModel()
        {
            RenderableSeries = new ObservableCollection<IRenderableSeries3DViewModel>();

            SeriesTypes = new ObservableCollection<string>
            {
                "Column Series",
                "Impulse Series",
                "PointLine Series",
                "SurfaceMesh Series",
                "Waterfall Series",
                "Scatter Series"
            };
            _selectedType = SeriesTypes[5];

            AddCommand = new ActionCommand(() =>
            {
                RenderableSeries.Add(ViewModelFactory3D.New(SelectedType));

                RemoveCommand.RaiseCanExecuteChanged();
                ClearCommand.RaiseCanExecuteChanged();

            });

            RemoveCommand = new ActionCommand(() =>
            {
                RenderableSeries.Remove(RenderableSeries.Last());

                ClearCommand.RaiseCanExecuteChanged();
                RemoveCommand.RaiseCanExecuteChanged();
            }, () => RenderableSeries.Any());

            ClearCommand = new ActionCommand(() =>
            {
                RenderableSeries.Clear();

                ClearCommand.RaiseCanExecuteChanged();
                RemoveCommand.RaiseCanExecuteChanged();
            }, () => RenderableSeries.Any());
        }

        public ObservableCollection<IRenderableSeries3DViewModel> RenderableSeries { get; set; }
        public ObservableCollection<string> SeriesTypes { get; set; }

        public string SelectedType
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _selectedType = value;
                    OnPropertyChanged("SelectedType");
                }
            }
            get { return _selectedType; }
        }

        public ActionCommand AddCommand { get; private set; }
        public ActionCommand RemoveCommand { get; private set; }
        public ActionCommand ClearCommand { get; private set; }

    
    }
}
