using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Data.Numerics;
using SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer
{
    public class ChannelViewModel : BaseViewModel
    {
        private Lazy<ObservableCollection<IRenderableSeries>> _renderableSeries;
        private readonly IDataSeries _dataSeries;

        private bool _isDigital;
        private DoubleRange _yRange;
        private string _channelName;
        private readonly int _channelIndex;
        private double _channelHeight;
        private uint _resamplingPrecision;
        private int _strokeThickness;

        public ChannelViewModel(IDataSeries dataSeries, DoubleRange yRange, int channelIndex, string channelName)
        {
            _dataSeries = dataSeries;
            _channelIndex = channelIndex;

            YRange = yRange;
            ChannelName = channelName;
            ChannelHeight = 60;

            _renderableSeries = new Lazy<ObservableCollection<IRenderableSeries>>(() =>
            {
                var coll = new ObservableCollection<IRenderableSeries>
                {
                    new FastLineRenderableSeries
                    {
                        DataSeries = _dataSeries,
                        StrokeThickness = _strokeThickness,
                        Stroke = ColorHelper.GetDimColor(_channelIndex, 0.8),
                        IsDigitalLine = _isDigital,
                        ResamplingPrecision = _resamplingPrecision,
                    }
                };

                return coll;
            });
        }

        public int DataCount => _dataSeries.Count;

        public bool IsDigital
        {
            get => _isDigital;
            set
            {
                _isDigital = value;

                if (_renderableSeries?.IsValueCreated == true && _renderableSeries?.Value.Any() == true)
                {
                    _renderableSeries.Value[0].IsDigitalLine = _isDigital;
                }
            }
        }

        public int StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;

                if (_renderableSeries?.IsValueCreated == true && _renderableSeries?.Value.Any() == true)
                {
                    _renderableSeries.Value[0].StrokeThickness = _strokeThickness;
                }
            }
        }

        public uint ResamplingPrecision
        {
            get => _resamplingPrecision;
            set
            {
                _resamplingPrecision = value;

                if (_renderableSeries?.IsValueCreated == true && _renderableSeries?.Value.Any() == true)
                {
                    _renderableSeries.Value[0].ResamplingPrecision = _resamplingPrecision;
                }
            }
        }

        public void Clear()
        {
            foreach(var rs in RenderableSeries)
            {
                rs.DataSeries.Clear();
                rs.DataSeries = null;
            }
            _renderableSeries = null;
            OnPropertyChanged(nameof(RenderableSeries));
        }

        public double ChannelHeight
        {
            get => _channelHeight;
            set
            {
                if (_channelHeight.CompareTo(value) == 0) return;
                _channelHeight = value;
                OnPropertyChanged(nameof(ChannelHeight));
            }
        }

        public Brush ChannelColor => RenderableSeries.Any() ? new SolidColorBrush(RenderableSeries[0].Stroke) : Brushes.Transparent;

        public ObservableCollection<IRenderableSeries> RenderableSeries => _renderableSeries?.Value;

        public DoubleRange YRange
        {
            get => _yRange;
            set
            {
                _yRange = value;
                OnPropertyChanged(nameof(YRange));
            }
        }

        public string ChannelName
        {
            get => _channelName;
            set
            {
                _channelName = value;
                OnPropertyChanged(nameof(ChannelName));
            }
        }


        public void SetChannelHeightDelta(double delta)
        {
            var newHeight = ChannelHeight + delta;
            if (newHeight >= 30 && newHeight <= 60)
            {
                ChannelHeight = newHeight;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { 
                Clear();
            }
        }
    }
}
