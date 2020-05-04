using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.VitalSignsMonitor
{
    public partial class VitalSignsMonitorView : UserControl
    {
        private const int FifoCapacity = 800;

        private readonly XyDataSeries<double> _ecgDataSeries = CreateDataSeries(FifoCapacity);
        private readonly XyDataSeries<double> _ecgSweepDataSeries = CreateDataSeries(FifoCapacity);

        private readonly XyDataSeries<double> _bloodPressureDataSeries = CreateDataSeries(FifoCapacity);
        private readonly XyDataSeries<double> _bloodPressureSweepDataSeries = CreateDataSeries(FifoCapacity);

        private readonly XyDataSeries<double> _bloodVolumeDataSeries = CreateDataSeries(FifoCapacity);
        private readonly XyDataSeries<double> _bloodVolumeSweepDataSeries = CreateDataSeries(FifoCapacity);

        private readonly XyDataSeries<double> _bloodOxygenationDataSeries = CreateDataSeries(FifoCapacity);
        private readonly XyDataSeries<double> _bloodOxygenationSweepDataSeries = CreateDataSeries(FifoCapacity);

        private readonly XyDataSeries<double> _lastEcgSweepDataSeries = CreateDataSeries(1);
        private readonly XyDataSeries<double> _lastBloodPressureDataSeries = CreateDataSeries(1);

        private readonly XyDataSeries<double> _lastBloodVolumeDataSeries = CreateDataSeries(1);
        private readonly XyDataSeries<double> _lastBloodOxygenationSweepDataSeries = CreateDataSeries(1);

        private readonly VitalSignsDataProvider _dataProvider = new VitalSignsDataProvider(TimeSpan.FromMilliseconds(5));
        private readonly VitalSignsIndicatorsProvider _indicatorsProvider = new VitalSignsIndicatorsProvider();
        private readonly VitalSignsBatch _dataBatch = new VitalSignsBatch();

        private IDisposable _dataSubscription;

        public VitalSignsMonitorView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SetupCharts();
            UpdateIndicators();

            var dataSubscription = _dataProvider.Data
                .Buffer(TimeSpan.FromMilliseconds(50))
                .ObserveOn(SynchronizationContext.Current)
                .Do(UpdateCharts)
                .Subscribe();

            var indicatorSubscription = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .ObserveOn(SynchronizationContext.Current)
                .Do(UpdateIndicators)
                .Subscribe();

            _dataSubscription = new CompositeDisposable(dataSubscription, indicatorSubscription);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _dataSubscription.Dispose();
        }

        private void SetupCharts()
        {
            var xAxis = new NumericAxis
            {
                VisibleRange = new DoubleRange(0, 10),
                AutoRange = AutoRange.Never,
                DrawMajorGridLines = false,
                DrawMinorGridLines = false,
                Visibility = Visibility.Collapsed
            };

            const string ecgId = "ecgId";
            const string bloodPressureId = "bloodPressureId";
            const string bloodVolumeId = "bloodVolumeId";
            const string bloodOxygenationId = "bloodOxygenationId";

            var heartRateColor = GetResourceColor("HeartRateBrush");
            var bloodPressureColor = GetResourceColor("BloodPressureBrush");
            var bloodVolumeColor = GetResourceColor("BloodVolumeBrush");
            var bloodOxygenation = GetResourceColor("BloodOxygenationBrush");

            using (Surface.SuspendUpdates())
            {
                Surface.XAxes.Add(xAxis);
                Surface.YAxes.Add(CreateYAxis(ecgId, _dataProvider.EcgHeartRateRange, 1));
                Surface.YAxes.Add(CreateYAxis(bloodPressureId, _dataProvider.BloodPressureRange, 3));
                Surface.YAxes.Add(CreateYAxis(bloodVolumeId, _dataProvider.BloodVolumeRange, 5));
                Surface.YAxes.Add(CreateYAxis(bloodOxygenationId, _dataProvider.BloodOxygenationRange, 7));

                var ecgSeries = CreateRenderableSeries(ecgId, _ecgDataSeries,
                    _ecgSweepDataSeries, _lastEcgSweepDataSeries, heartRateColor);

                var bloodPressureSeries = CreateRenderableSeries(bloodPressureId, _bloodPressureDataSeries,
                    _bloodPressureSweepDataSeries, _lastBloodPressureDataSeries, bloodPressureColor);
                
                var bloodVolumeSeries = CreateRenderableSeries(bloodVolumeId, _bloodVolumeDataSeries,
                    _bloodVolumeSweepDataSeries, _lastBloodVolumeDataSeries, bloodVolumeColor);

                var bloodOxygenationSeries = CreateRenderableSeries(bloodOxygenationId, _bloodOxygenationDataSeries,
                    _bloodOxygenationSweepDataSeries, _lastBloodOxygenationSweepDataSeries, bloodOxygenation); 
                
                ecgSeries.ForEachDo(x => Surface.RenderableSeries.Add(x));
                bloodPressureSeries.ForEachDo(x => Surface.RenderableSeries.Add(x));

                bloodVolumeSeries.ForEachDo(x => Surface.RenderableSeries.Add(x));
                bloodOxygenationSeries.ForEachDo(x => Surface.RenderableSeries.Add(x));
            }
        }

        private void UpdateCharts(IList<VitalSignsData> data)
        {
            if (data.Count == 0) return;

            _dataBatch.UpdateData(data);

            using (Surface.SuspendUpdates())
            {
                var xValues = _dataBatch.XValues;

                _ecgDataSeries.Append(xValues, _dataBatch.ECGHeartRateValuesA);
                _ecgSweepDataSeries.Append(xValues, _dataBatch.ECGHeartRateValuesB);

                _bloodPressureDataSeries.Append(xValues, _dataBatch.BloodPressureValuesA);
                _bloodPressureSweepDataSeries.Append(xValues, _dataBatch.BloodPressureValuesB);

                _bloodOxygenationDataSeries.Append(xValues, _dataBatch.BloodOxygenationValuesA);
                _bloodOxygenationSweepDataSeries.Append(xValues, _dataBatch.BloodOxygenationValuesB);

                _bloodVolumeDataSeries.Append(xValues, _dataBatch.BloodVolumeValuesA);
                _bloodVolumeSweepDataSeries.Append(xValues, _dataBatch.BloodVolumeValuesB);

                var lastEcgData = _dataBatch.LastVitalSignsData;
                var xValue = lastEcgData.XValue;

                _lastEcgSweepDataSeries.Append(xValue, lastEcgData.ECGHeartRate);
                _lastBloodPressureDataSeries.Append(xValue, lastEcgData.BloodPressure);

                _lastBloodOxygenationSweepDataSeries.Append(xValue, lastEcgData.BloodOxygenation);
                _lastBloodVolumeDataSeries.Append(xValue, lastEcgData.BloodVolume);
            }
        }

        private void UpdateIndicators(long time = 0L)
        {
            HeartBeatIcon.Visibility = time % 2 == 0 ? Visibility.Hidden : Visibility.Visible;

            if (time % 5 == 0)
            {
                _indicatorsProvider.Update();

                HeartRateIndicator.Text = _indicatorsProvider.BpmValue;

                BloodPressureIndicator.Text = _indicatorsProvider.BpValue;
                BloodPressureBar.Value = _indicatorsProvider.BpbValue;

                BloodVolumeIndicator.Text = _indicatorsProvider.BvValue;
                BloodVolumeBar1.Value = _indicatorsProvider.BvBar1Value;
                BloodVolumeBar2.Value = _indicatorsProvider.BvBar2Value;

                BloodOxygenIndicator.Text = _indicatorsProvider.SpoValue;
                BloodOxygenClock.Text = _indicatorsProvider.SpoClockValue;
            }
        }

        private Color GetResourceColor(string resourceKey)
        {
            return (FindResource(resourceKey) as SolidColorBrush)?.Color ?? Colors.White;
        }

        private NumericAxis CreateYAxis(string axisId, IRange visibleRange, int stackedIndex)
        {
            var yAxis = new NumericAxis
            {
                Id = axisId,
                Width = 0d,
                VisibleRange = visibleRange,
                AutoRange = AutoRange.Never,
                AxisAlignment = AxisAlignment.Right,
                DrawMajorBands = false,
                DrawMinorGridLines = false,
                DrawMajorGridLines = false
            };

            Grid.SetRow(yAxis, stackedIndex);

            return yAxis;
        }

        private static XyDataSeries<double> CreateDataSeries(int fifoCapacity)
        {
            return new XyDataSeries<double>
            {
                FifoCapacity = fifoCapacity,
                AcceptsUnsortedData = true
            };
        }

        private IEnumerable<IRenderableSeries> CreateRenderableSeries(string yAxisId,
            IDataSeries baseSeries, IDataSeries sweepSeries, IDataSeries lastSeries, Color color)
        {
            const int strokeThickness = 1;
            const int pointMarkerSize = 4;

            var rs1 = new FastLineRenderableSeries
            {
                YAxisId = yAxisId,
                DataSeries = baseSeries,
                Stroke = color,
                StrokeThickness = strokeThickness,
                PaletteProvider = new VitalSignsPaletteProvider()
            };

            var rs2 = new FastLineRenderableSeries
            {
                YAxisId = yAxisId,
                DataSeries = sweepSeries,
                Stroke = color,
                StrokeThickness = strokeThickness,
                PaletteProvider = new VitalSignsPaletteProvider()
            };

            var rs3 = new XyScatterRenderableSeries
            {
                YAxisId = yAxisId,
                DataSeries = lastSeries,
                PointMarker = new EllipsePointMarker
                {
                    Width = pointMarkerSize,
                    Height = pointMarkerSize,
                    Fill = color,
                    Stroke = Colors.White,
                    StrokeThickness = strokeThickness
                }
            };

            return new IRenderableSeries[] { rs1, rs2, rs3 };
        }
    }
}