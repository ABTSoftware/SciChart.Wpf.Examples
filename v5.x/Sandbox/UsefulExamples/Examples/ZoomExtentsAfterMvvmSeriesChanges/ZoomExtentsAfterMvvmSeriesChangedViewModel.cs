using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Data.Model;
using SciChart.Wpf.UI.Reactive;

namespace SciChart.Sandbox.Examples.ZoomExtentsAfterMvvmSeriesChanges
{
    public class ZoomExtentsAfterMvvmSeriesChangedViewModel : BindableObject
    {
        private ActionCommand _addSeriesCommand;
        private ObservableCollection<IRenderableSeriesViewModel> _series = new ObservableCollection<IRenderableSeriesViewModel>();
        private DefaultViewportManager _viewportManager = new DefaultViewportManager();
        private int _amplitude = 1;

        public ZoomExtentsAfterMvvmSeriesChangedViewModel()
        {
            _addSeriesCommand = new ActionCommand(AddSeries);
        }

        private void AddSeries()
        {
            var random = new Random();
            Series.Add(new LineRenderableSeriesViewModel()
            {
                Stroke= Colors.DarkOrange, DataSeries = GetData(++_amplitude, random.NextDouble()*0.1)
            });

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => _viewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500))), DispatcherPriority.Input, null);
        }

        public DefaultViewportManager ViewportManager => _viewportManager;
        public ObservableCollection<IRenderableSeriesViewModel> Series => _series;

        public ICommand AddSeriesCommand => _addSeriesCommand;

        private IDataSeries GetData(double amplitude, double damping)
        {
            var xyDataSeries = new XyDataSeries<double>();
            for (int i = 0; i < 1000; i++)
            {
                xyDataSeries.Append(i, Math.Sin(i * 0.1) * amplitude);
                amplitude *= (1.0 - damping);
            }

            return xyDataSeries;
        }
    }
}