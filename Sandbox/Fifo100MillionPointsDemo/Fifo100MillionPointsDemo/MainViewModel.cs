

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
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


        public ObservableCollection<IRenderableSeriesViewModel> Series { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

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

            // Load the points
            const int seriesCount = 5;
            const int pointCount = 10;
            var series = await CreateSeries(seriesCount, pointCount);
            Series.AddRange(series);

            LoadingMessage = null;
        }

        private async Task<List<IRenderableSeriesViewModel>> CreateSeries(int seriesCount, int pointCount)
        {
            return await Task.Run(() =>
            {
                List<IRenderableSeriesViewModel> series = new List<IRenderableSeriesViewModel>();
                for (int i = 0; i < seriesCount; i++)
                {
                    var xyDataSeries = new XyDataSeries<float, float>() {FifoCapacity = pointCount};
                    int yOffset = i + 2;
                    for (int j = 0; j < pointCount; j++)
                    {
                        xyDataSeries.Append(j, Rand.Next() + yOffset);
                    }

                    series.Add(new LineRenderableSeriesViewModel()
                    {
                        DataSeries = xyDataSeries,
                        Stroke = Colors.RandomColor()
                    });
                }

                return series;
            });
        }

        private void OnStop()
        {
            IsStopped = true;
            Series.Clear();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
}
