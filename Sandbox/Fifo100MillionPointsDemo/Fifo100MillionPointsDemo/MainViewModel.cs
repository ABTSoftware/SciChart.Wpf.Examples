

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.UI.Reactive;

namespace Fifo100MillionPointsDemo
{
    public class MainViewModel : BindableObject
    {
        private bool _isStopped;
        private string _loadingMessage;

        public MainViewModel()
        {
            _isStopped = true;
            RunCommand = new ActionCommand(OnRun, () => this.IsStopped);
            StopCommand = new ActionCommand(OnStop, () => this.IsStopped == false);
        }


        public ObservableCollection<IRenderableSeries> Series { get; } = new ObservableCollection<IRenderableSeries>();

        public ActionCommand RunCommand { get; }
        public ActionCommand StopCommand { get; }

        public bool IsStopped
        {
            get => _isStopped;
            set
            {
                _isStopped = value;
                OnPropertyChanged("IsStopped");
                RunCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
        }

        public string LoadingMessage
        {
            get => _loadingMessage;
            set
            {
                _loadingMessage = value;
                OnPropertyChanged("LoadingMessage");
            }
        }

        private async void OnRun()
        {
            LoadingMessage = "Loading 100 Million Points...";
            IsStopped = false;

            // todo load the points
            await Task.Delay(TimeSpan.FromMilliseconds(1000));

            LoadingMessage = null;
        }

        private void OnStop()
        {
            IsStopped = true;
        }
    }
}
