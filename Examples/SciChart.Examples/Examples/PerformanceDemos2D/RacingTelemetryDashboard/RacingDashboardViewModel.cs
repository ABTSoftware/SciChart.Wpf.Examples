// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// RacingDashboardViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Common.Extensions;
using SciChart.Examples.Examples.PerformanceDemos2D.RacingTelemetryDashboard.ViewModels;

namespace SciChart.Examples.Examples.PerformanceDemos2D.RacingTelemetryDashboard
{
    public class RacingDashboardViewModel : BaseViewModel
    {
        private double[] _distanceX;
        private double[] _timeX;
        private bool _isSyncingSlice;
        private bool _isSyncingSelection;
        private RacingTelemetryDataGenerator _racingTelemetryDataGenerator;

        // RangeSyncGroupId constants
        private const string GroupXDomain = "XDomain";
        private const string GroupSpeedY = "Speed_Y";
        private const string GroupRpmY = "Rpm_Y";
        private const string GroupTempY = "Temp_Y";
        private const string GroupThrottleY = "Throttle_Y";

        private const int _maxDistanceInMeters = 4500;
        private const int _pointsCount = 1000;

        private static readonly Dictionary<string, string> YAxisUnits = new Dictionary<string, string>
        {
            { "SpeedY_Dist", "kph" },
            { "RpmY_Dist", "RPM" },
            { "TempY_Dist", "°C" },
            { "ThrottleY_Dist", "%" },
        };

        private static readonly double[,] DefaultCorners =
        {
            {  820,  900,  990, 140, 210 },  // C1: 80m brake,  70 kph drop
            { 1620, 1870, 2100,  38, 220 },  // C2: 250m brake, 182 kph drop
            { 2630, 2750, 2900,  58, 215 },  // C3: 120m brake, 157 kph drop — hard+short, blasts out
            { 3240, 3470, 3750,  92, 205 },  // C4: 230m brake, 113 kph drop — soft+long, crawls out
            { 4070, 4200, 4370, 110, 200 },  // C5: 130m brake,  90 kph drop
        };

        public ChartPanelViewModel DistanceChart { get; private set; }

        public ChartPanelViewModel TimeChart { get; private set; }

        public ObservableCollection<ChartPanelViewModel> CrossCharts { get; } = new ObservableCollection<ChartPanelViewModel>();

        public RacingDashboardViewModel()
        {
            _racingTelemetryDataGenerator = new RacingTelemetryDataGenerator(DefaultCorners);
            _distanceX = _racingTelemetryDataGenerator.GenerateDistanceAxis(_maxDistanceInMeters, _pointsCount);
            _timeX = _racingTelemetryDataGenerator.GenerateTimeAxisFromSpeed(_distanceX);
            var speedData = _racingTelemetryDataGenerator.GenerateSpeedPanelData(_distanceX);
            var rpmData = _racingTelemetryDataGenerator.GenerateRpmThrottlePanelData(_distanceX);
            var tempData = _racingTelemetryDataGenerator.GenerateTemperaturePanelData(_distanceX);

            DistanceChart = BuildDistanceChart(_distanceX, speedData, rpmData, tempData);
            TimeChart = BuildTimeChart(_timeX, speedData, rpmData, tempData);
            BuildCrossCharts(speedData, rpmData, tempData);

            // start position of verticalSlice on distance chart.
            DistanceChart.SliceX = 20.0;

            // Unit rides in Tag so the legend template (bound directly to the VMs) can display it.
            foreach (var rs in DistanceChart.RenderableSeries)
            {
                if (YAxisUnits.TryGetValue(rs.YAxisId, out var u))
                    rs.Tag = u;
            }

            DistanceChart.PropertyChanged += OnDistSliceChanged;
            TimeChart.PropertyChanged += OnTimeSliceChanged;

            SetupSelectionSync();

            DistanceChart.RenderableSeries.ZoomExtentsWhenReady();
            TimeChart.RenderableSeries.ZoomExtentsWhenReady();
        }

        private IDataSeries MakeXy(string name, double[] x, double[] y)
        {
            var ds = new XyDataSeries<double, double> { SeriesName = name, AcceptsUnsortedData = true };
            ds.Append(x, y);
            return ds;
        }

        private void OnDistSliceChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ChartPanelViewModel.SliceX)) return;
            if (_isSyncingSlice) return;
            _isSyncingSlice = true;
            TimeChart.SliceX = InterpolatingRangeSyncTransform.Interpolate(_distanceX, _timeX, DistanceChart.SliceX);
            _isSyncingSlice = false;
        }

        private void OnTimeSliceChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ChartPanelViewModel.SliceX)) return;
            if (_isSyncingSlice) return;

            _isSyncingSlice = true;
            DistanceChart.SliceX = InterpolatingRangeSyncTransform.Interpolate(_timeX, _distanceX, TimeChart.SliceX);
            _isSyncingSlice = false;
        }

        private void SetupSelectionSync()
        {
            SubscribeSelection(DistanceChart.RenderableSeries, TimeChart.RenderableSeries);
            SubscribeSelection(TimeChart.RenderableSeries, DistanceChart.RenderableSeries);
        }

        private void SubscribeSelection(ObservableCollection<IRenderableSeriesViewModel> source, ObservableCollection<IRenderableSeriesViewModel> target)
        {
            foreach (var vm in source.OfType<LineRenderableSeriesViewModel>())
            {
                var captured = vm;

                vm.SelectionChanged += (s, e) =>
                {
                    if (_isSyncingSelection) return;
                    _isSyncingSelection = true;
                    try
                    {
                        var seriesName = captured.DataSeries?.SeriesName;

                        var mirror = target.OfType<LineRenderableSeriesViewModel>().FirstOrDefault(t => t.DataSeries?.SeriesName == seriesName);
                        if (mirror != null)
                        {
                            mirror.IsSelected = captured.IsSelected;
                        }
                    }
                    finally
                    {
                        _isSyncingSelection = false;
                    }
                };
            }
        }

        private ChartPanelViewModel BuildDistanceChart(double[] x, Dictionary<string, double[]> speed, Dictionary<string, double[]> rpm, Dictionary<string, double[]> temp)
        {
            var c = new ChartPanelViewModel { ChartTitle = "Distance Domain" };

            // X and Y axes
            c.XAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "DistX", AxisAlignment = AxisAlignment.Bottom, AxisTitle = "Distance (m)", RangeSyncGroupId = GroupXDomain, RangeSyncSourceOnZoomExtents = true });
            c.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "SpeedY_Dist", AxisAlignment = AxisAlignment.Left, AxisTitle = "Speed (kph)", RangeSyncGroupId = GroupSpeedY, RangeSyncSourceOnZoomExtents = true, GrowBy = new DoubleRange(0.05, 0.05), VisibleRangeLimit = new DoubleRange(0, 0), VisibleRangeLimitMode = RangeClipMode.Min });
            c.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "RpmY_Dist", AxisAlignment = AxisAlignment.Right, AxisTitle = "RPM", RangeSyncGroupId = GroupRpmY, RangeSyncSourceOnZoomExtents = true, GrowBy = new DoubleRange(0.05, 0.05) });
            c.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "TempY_Dist", AxisAlignment = AxisAlignment.Left, AxisTitle = "Temp (°C)", RangeSyncGroupId = GroupTempY, RangeSyncSourceOnZoomExtents = true, GrowBy = new DoubleRange(0.05, 0.05), VisibleRangeLimit = new DoubleRange(0, 0), VisibleRangeLimitMode = RangeClipMode.Min });
            c.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "ThrottleY_Dist", AxisAlignment = AxisAlignment.Right, AxisTitle = "%", RangeSyncGroupId = GroupThrottleY, RangeSyncSourceOnZoomExtents = true, GrowBy = new DoubleRange(0.05, 0.05), VisibleRangeLimit = new DoubleRange(0, 100) });
            c.YAxes.Add(new NumericAxisViewModel { Id = "SliceY_Dist", AxisAlignment = AxisAlignment.Left, AutoRange = AutoRange.Always, Visibility = Visibility.Collapsed });

            // Speed
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("GPS_Speed", x, speed["GPS_Speed"]), XAxisId = "DistX", YAxisId = "SpeedY_Dist", Stroke = Color.FromRgb(0x50, 0xC7, 0xE0), StyleKey = "GpsSpeedStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("GPS_Speed_B", x, speed["GPS_Speed_B"]), XAxisId = "DistX", YAxisId = "SpeedY_Dist", Stroke = Color.FromRgb(0x2F, 0xA4, 0xC0), StyleKey = "GpsSpdB_Style" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("WheelSpeed_FL", x, speed["WheelSpeed_FL"]), XAxisId = "DistX", YAxisId = "SpeedY_Dist", Stroke = Color.FromRgb(0xF4, 0x84, 0x20), StyleKey = "WheelFL_Style" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("WheelSpeed_FR", x, speed["WheelSpeed_FR"]), XAxisId = "DistX", YAxisId = "SpeedY_Dist", Stroke = Color.FromRgb(0xE8, 0xD4, 0x4D), StyleKey = "WheelFR_Style" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("WheelSpeed_RL", x, speed["WheelSpeed_RL"]), XAxisId = "DistX", YAxisId = "SpeedY_Dist", Stroke = Color.FromRgb(0x92, 0xD0, 0x50), StyleKey = "WheelRL_Style" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("WheelSpeed_RR", x, speed["WheelSpeed_RR"]), XAxisId = "DistX", YAxisId = "SpeedY_Dist", Stroke = Color.FromRgb(0xFF, 0x6B, 0x6B), StyleKey = "WheelRR_Style" });

            // RPM / Gear / DRS
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("RPM_Engine", x, rpm["RPM_Engine"]), XAxisId = "DistX", YAxisId = "RpmY_Dist", Stroke = Color.FromRgb(0xFF, 0xFF, 0xFF), StyleKey = "EngineRpmStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("RPM_Driveshaft", x, rpm["RPM_Driveshaft"]), XAxisId = "DistX", YAxisId = "RpmY_Dist", Stroke = Color.FromRgb(0xA0, 0xA0, 0xFF), StyleKey = "DriveRpmStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Gear", x, rpm["Gear"]), XAxisId = "DistX", YAxisId = "RpmY_Dist", Stroke = Color.FromRgb(0xFF, 0xD7, 0x00), StyleKey = "GearStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("DRS", x, rpm["DRS"]), XAxisId = "DistX", YAxisId = "RpmY_Dist", Stroke = Color.FromRgb(0x00, 0xFF, 0x00), StyleKey = "DrsStyle" });

            // Temperature
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Temp_ExhaustGas", x, temp["Temp_ExhaustGas"]), XAxisId = "DistX", YAxisId = "TempY_Dist", Stroke = Color.FromRgb(0xFF, 0x20, 0x20), StyleKey = "ExhaustTempStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Temp_Coolant", x, temp["Temp_Coolant"]), XAxisId = "DistX", YAxisId = "TempY_Dist", Stroke = Color.FromRgb(0x40, 0x80, 0xFF), StyleKey = "CoolantTempStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Temp_Oil", x, temp["Temp_Oil"]), XAxisId = "DistX", YAxisId = "TempY_Dist", Stroke = Color.FromRgb(0xFF, 0x90, 0x40), StyleKey = "OilTempStyle" });

            // Throttle / Brake
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Throttle", x, rpm["Throttle"]), XAxisId = "DistX", YAxisId = "ThrottleY_Dist", Stroke = Color.FromRgb(0x90, 0xEE, 0x90), StyleKey = "ThrottleStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Brake", x, rpm["Brake"]), XAxisId = "DistX", YAxisId = "ThrottleY_Dist", Stroke = Color.FromRgb(0xFF, 0x60, 0x60), StyleKey = "BrakeStyle" });

            return c;
        }

        private ChartPanelViewModel BuildTimeChart(double[] x, Dictionary<string, double[]> speed, Dictionary<string, double[]> rpm, Dictionary<string, double[]> temp)
        {
            var c = new ChartPanelViewModel { ChartTitle = "Time Domain" };

            c.XAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "TimeX", AxisAlignment = AxisAlignment.Bottom, AxisTitle = "Time (s)", RangeSyncGroupId = GroupXDomain, RangeSyncTransform = new InterpolatingRangeSyncTransform(_timeX, _distanceX), RangeSyncSourceOnZoomExtents = true });
            c.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "SpeedY_Time", AxisAlignment = AxisAlignment.Left, AxisTitle = "Speed (kph)", RangeSyncGroupId = GroupSpeedY, RangeSyncSourceOnZoomExtents = true, GrowBy = new DoubleRange(0.05, 0.05), VisibleRangeLimit = new DoubleRange(0, 0), VisibleRangeLimitMode = RangeClipMode.Min });
            c.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "RpmY_Time", AxisAlignment = AxisAlignment.Right, AxisTitle = "RPM", RangeSyncGroupId = GroupRpmY, RangeSyncSourceOnZoomExtents = true, GrowBy = new DoubleRange(0.05, 0.05) });
            c.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "TempY_Time", AxisAlignment = AxisAlignment.Left, AxisTitle = "Temp (°C)", RangeSyncGroupId = GroupTempY, RangeSyncSourceOnZoomExtents = true, GrowBy = new DoubleRange(0.05, 0.05), VisibleRangeLimit = new DoubleRange(0, 0), VisibleRangeLimitMode = RangeClipMode.Min });
            c.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "ThrottleY_Time", AxisAlignment = AxisAlignment.Right, AxisTitle = "%", RangeSyncGroupId = GroupThrottleY, RangeSyncSourceOnZoomExtents = true, GrowBy = new DoubleRange(0.05, 0.05), VisibleRangeLimit = new DoubleRange(0, 100) });
            c.YAxes.Add(new NumericAxisViewModel { Id = "SliceY_Time", AxisAlignment = AxisAlignment.Left, AutoRange = AutoRange.Always, Visibility = Visibility.Collapsed });

            // Speed
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("GPS_Speed", x, speed["GPS_Speed"]), XAxisId = "TimeX", YAxisId = "SpeedY_Time", Stroke = Color.FromRgb(0x50, 0xC7, 0xE0), StyleKey = "GpsSpeedStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("GPS_Speed_B", x, speed["GPS_Speed_B"]), XAxisId = "TimeX", YAxisId = "SpeedY_Time", Stroke = Color.FromRgb(0x2F, 0xA4, 0xC0), StyleKey = "GpsSpdB_Style" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("WheelSpeed_FL", x, speed["WheelSpeed_FL"]), XAxisId = "TimeX", YAxisId = "SpeedY_Time", Stroke = Color.FromRgb(0xF4, 0x84, 0x20), StyleKey = "WheelFL_Style" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("WheelSpeed_FR", x, speed["WheelSpeed_FR"]), XAxisId = "TimeX", YAxisId = "SpeedY_Time", Stroke = Color.FromRgb(0xE8, 0xD4, 0x4D), StyleKey = "WheelFR_Style" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("WheelSpeed_RL", x, speed["WheelSpeed_RL"]), XAxisId = "TimeX", YAxisId = "SpeedY_Time", Stroke = Color.FromRgb(0x92, 0xD0, 0x50), StyleKey = "WheelRL_Style" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("WheelSpeed_RR", x, speed["WheelSpeed_RR"]), XAxisId = "TimeX", YAxisId = "SpeedY_Time", Stroke = Color.FromRgb(0xFF, 0x6B, 0x6B), StyleKey = "WheelRR_Style" });

            // RPM / Gear / DRS
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("RPM_Engine", x, rpm["RPM_Engine"]), XAxisId = "TimeX", YAxisId = "RpmY_Time", Stroke = Color.FromRgb(0xFF, 0xFF, 0xFF), StyleKey = "EngineRpmStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("RPM_Driveshaft", x, rpm["RPM_Driveshaft"]), XAxisId = "TimeX", YAxisId = "RpmY_Time", Stroke = Color.FromRgb(0xA0, 0xA0, 0xFF), StyleKey = "DriveRpmStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Gear", x, rpm["Gear"]), XAxisId = "TimeX", YAxisId = "RpmY_Time", Stroke = Color.FromRgb(0xFF, 0xD7, 0x00), StyleKey = "GearStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("DRS", x, rpm["DRS"]), XAxisId = "TimeX", YAxisId = "RpmY_Time", Stroke = Color.FromRgb(0x00, 0xFF, 0x00), StyleKey = "DrsStyle" });

            // Temperature
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Temp_ExhaustGas", x, temp["Temp_ExhaustGas"]), XAxisId = "TimeX", YAxisId = "TempY_Time", Stroke = Color.FromRgb(0xFF, 0x20, 0x20), StyleKey = "ExhaustTempStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Temp_Coolant", x, temp["Temp_Coolant"]), XAxisId = "TimeX", YAxisId = "TempY_Time", Stroke = Color.FromRgb(0x40, 0x80, 0xFF), StyleKey = "CoolantTempStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Temp_Oil", x, temp["Temp_Oil"]), XAxisId = "TimeX", YAxisId = "TempY_Time", Stroke = Color.FromRgb(0xFF, 0x90, 0x40), StyleKey = "OilTempStyle" });

            // Throttle / Brake
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Throttle", x, rpm["Throttle"]), XAxisId = "TimeX", YAxisId = "ThrottleY_Time", Stroke = Color.FromRgb(0x90, 0xEE, 0x90), StyleKey = "ThrottleStyle" });
            c.RenderableSeries.Add(new LineRenderableSeriesViewModel { DataSeries = MakeXy("Brake", x, rpm["Brake"]), XAxisId = "TimeX", YAxisId = "ThrottleY_Time", Stroke = Color.FromRgb(0xFF, 0x60, 0x60), StyleKey = "BrakeStyle" });

            return c;
        }

        private void BuildCrossCharts(Dictionary<string, double[]> speed, Dictionary<string, double[]> rpm, Dictionary<string, double[]> temp)
        {
            // Chart 9 — RPM vs Speed
            var c9 = new ChartPanelViewModel { ChartTitle = "RPM vs Speed" };
            c9.XAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "Cross_SpeedX_9", AxisAlignment = AxisAlignment.Bottom, AxisTitle = "Speed (kph)" });
            c9.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "Cross_RpmY_9", AxisAlignment = AxisAlignment.Left, AxisTitle = "RPM", AutoTicks = false, MajorDelta = 1000.0, MinorDelta = 500.0, IsLabelCullingEnabled = false, GrowBy = new DoubleRange(0.1, 0.1) });
            c9.RenderableSeries.Add(new XyScatterRenderableSeriesViewModel { DataSeries = MakeXy("RPM vs Speed", speed["GPS_Speed"], rpm["RPM_Engine"]), XAxisId = "Cross_SpeedX_9", YAxisId = "Cross_RpmY_9", StyleKey = "ScatterC9_Style" });
            CrossCharts.Add(c9);

            // Chart 10 — Temperature vs RPM
            var c10 = new ChartPanelViewModel { ChartTitle = "Temperature vs RPM" };
            c10.XAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "Cross_RpmX_10", AxisAlignment = AxisAlignment.Bottom, AxisTitle = "RPM", AutoTicks = false, MajorDelta = 1000.0, MinorDelta = 500.0, IsLabelCullingEnabled = false, GrowBy = new DoubleRange(0.1, 0.1) });
            c10.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "Cross_TempY_10", AxisAlignment = AxisAlignment.Left, AxisTitle = "Temp (°C)" });
            c10.RenderableSeries.Add(new XyScatterRenderableSeriesViewModel { DataSeries = MakeXy("Exhaust vs RPM", rpm["RPM_Engine"], temp["Temp_ExhaustGas"]), XAxisId = "Cross_RpmX_10", YAxisId = "Cross_TempY_10", StyleKey = "ScatterExhaust_Style" });
            c10.RenderableSeries.Add(new XyScatterRenderableSeriesViewModel { DataSeries = MakeXy("Coolant vs RPM", rpm["RPM_Engine"], temp["Temp_Coolant"]), XAxisId = "Cross_RpmX_10", YAxisId = "Cross_TempY_10", StyleKey = "ScatterCoolant_Style" });
            c10.RenderableSeries.Add(new XyScatterRenderableSeriesViewModel { DataSeries = MakeXy("Oil vs RPM", rpm["RPM_Engine"], temp["Temp_Oil"]), XAxisId = "Cross_RpmX_10", YAxisId = "Cross_TempY_10", StyleKey = "ScatterOil_Style" });
            CrossCharts.Add(c10);

            // Chart 11 — Temperature vs Speed
            var c11 = new ChartPanelViewModel { ChartTitle = "Temperature vs Speed" };
            c11.XAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "Cross_SpeedX_11", AxisAlignment = AxisAlignment.Bottom, AxisTitle = "Speed (kph)" });
            c11.YAxes.Add(new NumericAxisViewModel { FontSize = 10, Id = "Cross_TempY_11", AxisAlignment = AxisAlignment.Left, AxisTitle = "Temp (°C)" });
            c11.RenderableSeries.Add(new XyScatterRenderableSeriesViewModel { DataSeries = MakeXy("Exhaust vs Speed", speed["GPS_Speed"], temp["Temp_ExhaustGas"]), XAxisId = "Cross_SpeedX_11", YAxisId = "Cross_TempY_11", StyleKey = "ScatterExhaust_Style" });
            c11.RenderableSeries.Add(new XyScatterRenderableSeriesViewModel { DataSeries = MakeXy("Coolant vs Speed", speed["GPS_Speed"], temp["Temp_Coolant"]), XAxisId = "Cross_SpeedX_11", YAxisId = "Cross_TempY_11", StyleKey = "ScatterCoolant_Style" });
            c11.RenderableSeries.Add(new XyScatterRenderableSeriesViewModel { DataSeries = MakeXy("Oil vs Speed", speed["GPS_Speed"], temp["Temp_Oil"]), XAxisId = "Cross_SpeedX_11", YAxisId = "Cross_TempY_11", StyleKey = "ScatterOil_Style" });
            CrossCharts.Add(c11);
        }
    }
}
