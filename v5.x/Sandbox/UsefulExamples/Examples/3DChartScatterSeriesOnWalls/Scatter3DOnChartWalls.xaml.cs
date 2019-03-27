using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.Model;
using SciChart.Data.Extensions;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;
using SciChart.Wpf.UI.Reactive.Annotations;

namespace SciChart.Sandbox.Examples._3DChartScatterSeriesOnWalls
{
    [TestCase("3D Chart Scatter Series on chart walls")]
    public partial class Scatter3DOnChartWalls : Window, INotifyPropertyChanged
    {
        private XyzDataSeries3D<double> _xyzData;
        private DispatcherTimer _timer;
        private int _pointCount = 100000;

        private readonly Random _random = new Random();
        private XyzDataSeries3D<double> _wallZyData;
        private DoubleRange _xRange;
        private DoubleRange _zRange;
        private XyzDataSeries3D<double> _wallXyData;

        public Scatter3DOnChartWalls()
        {
            InitializeComponent();

            OnStart();
        }

        // 1. We need the XAxis.VisibleRange to clamp the second series WallXyScatterSeries to the wall
        // so we bind to it in the view 
        public DoubleRange XRange
        {
            get => _xRange;
            set
            {
                _xRange = value;
                OnPropertyChanged("XRange");
            }
        }

        // 2. We need the ZAxis.VisibleRange to clamp the second series WallZyScatterSeries to the wall
        // so we bind to it in the view 
        public DoubleRange ZRange
        {
            get => _zRange;
            set
            {
                _zRange = value;
                OnPropertyChanged("ZRange");
            }
        }

        private void OnStart()
        {
            if (ScatterRenderableSeries3D.DataSeries == null)
            {
                _xyzData = new XyzDataSeries3D<double>();

                // 2. I create three series not one
                _wallZyData = new XyzDataSeries3D<double>();
                _wallXyData = new XyzDataSeries3D<double>();

                // First load, fill with some random values                    
                for (int i = 0; i < _pointCount; i++)
                {
                    double x = DataManager.Instance.GetGaussianRandomNumber(50, 15);
                    double y = DataManager.Instance.GetGaussianRandomNumber(50, 15);
                    double z = DataManager.Instance.GetGaussianRandomNumber(50, 15);

                    _xyzData.Append(x, y, z);
                    _wallZyData.Append(10, y, z);
                    _wallXyData.Append(x,y,10);
                }

                ScatterRenderableSeries3D.DataSeries = _xyzData;
                WallXyScatterRenderableSeries3D.DataSeries = _wallXyData;
                WallZyScatterRenderableSeries3D.DataSeries = _wallZyData;
            }

            if (_timer == null)
            {
                _timer = new DispatcherTimer(DispatcherPriority.Render);
                _timer.Interval = TimeSpan.FromMilliseconds(1);
                _timer.Tick += OnTimerTick;
            }

            _timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            // Subsequent load, update point positions using a sort of brownian motion by using random
            // 

            // Access Raw 'arrays' to the inner data series. This is the fastest way to read and access data, however
            // any operations will not be thread safe, and will not trigger a redraw. This is why we invalidate below
            //
            // See also XyzDataSeries.Append, Update, Remove, Insert which are atomic operations
            // 
            // Note that the count of raw arrays may be greater than _xyzData.Count
            double[] xDataRaw = _xyzData.XValues.ToUncheckedList();
            double[] yDataRaw = _xyzData.YValues.ToUncheckedList();
            double[] zDataRaw = _xyzData.ZValues.ToUncheckedList();

            double[] wallZyX = _wallZyData.XValues.ToUncheckedList();
            double[] wallZyY = _wallZyData.YValues.ToUncheckedList();
            double[] wallZyZ = _wallZyData.ZValues.ToUncheckedList();

            double[] wallXyX = _wallXyData.XValues.ToUncheckedList();
            double[] wallXyY = _wallXyData.YValues.ToUncheckedList();
            double[] wallXyZ = _wallXyData.ZValues.ToUncheckedList();

            double maxX = _xRange != null ? _xRange.Max : 10;
            double maxZ = _zRange != null ? _zRange.Max : 10;

            // Update the data positions simulating 3D random walk / brownian motion 
            for (int i = 0, count = _xyzData.Count; i < count; i++)
            {
                xDataRaw[i] += _random.NextDouble() - 0.5;
                yDataRaw[i] += _random.NextDouble() - 0.5;
                zDataRaw[i] += _random.NextDouble() - 0.5;

                // 3. Update the wall positions 
                // by setting the series Y,Z values to be the same as the main series, but X-values always equal to XAxis.VisibleRange.Max

                wallZyX[i] = maxX;
                wallZyY[i] = yDataRaw[i];
                wallZyZ[i] = zDataRaw[i];

                wallXyX[i] = xDataRaw[i];
                wallXyY[i] = yDataRaw[i];
                wallXyZ[i] = maxZ;
            }                        

            // Raise DataSeriesChanged event and trigger chart updates
            _xyzData.IsDirty = true;
            _xyzData.OnDataSeriesChanged(DataSeriesUpdate.DataChanged, DataSeriesAction.Update);
            _wallZyData.IsDirty = true;
            _wallZyData.OnDataSeriesChanged(DataSeriesUpdate.DataChanged, DataSeriesAction.Update);
            _wallXyData.IsDirty = true;
            _wallXyData.OnDataSeriesChanged(DataSeriesUpdate.DataChanged, DataSeriesAction.Update);
        }


        private void Scatter3DOnChartWalls_Unloaded(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= OnTimerTick;
                _timer = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Camera3D_OnCameraUpdated(object sender, EventArgs e)
        {            
        }
    }
}
