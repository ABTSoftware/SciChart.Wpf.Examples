using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Schema;
using SciChart.Charting.Model.ChartData;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.RenderableSeries.Animations;
using SciChart.Charting.Visuals.RenderableSeries.DrawingProviders;
using SciChart.Charting.Visuals.RenderableSeries.HitTesters;
using SciChart.Core.Extensions;
using SciChart.Core.Utility;
using SciChart.Data.Model;
using SciChart.Drawing.Common;
using ResamplingMode = SciChart.Data.Numerics.ResamplingMode;

namespace SlimLineRenderableSeriesExample
{
    public class SlimLineRenderableSeries : DependencyObject, IRenderableSeries
    {
        private bool _isSelected;
        private bool _isVisible;

        private Color _stroke;
        private double _opacity;

        private int _strokeThickness;
        private int _selectedStrokeThickness;

        private int? _cachedThickness;
        private double[] _strokeDashArray;

        private string _xAxisId;
        private string _yAxisId;

        private IAxis _xAxis;
        private IAxis _yAxis;

        private IDataSeries _dataSeries;
        private LineDrawMode _drawNaNAs;

        private IPointMarker _pointMarker;
        private IPaletteProvider _paletteProvider;

        public event EventHandler SelectionChanged;
        public event EventHandler IsVisibleChanged;

        public IHitTestProvider HitTestProvider { get; protected set; }
        public IEnumerable<ISeriesDrawingProvider> DrawingProviders { get; protected set; }

        public IServiceContainer Services { get; set; }

        public IPointMarker GetSelectedPointMarker()
        {
            return PointMarker;
        }

        public bool AntiAliasing { get; set; }
        public bool IsDigitalLine { get; set; }

        public ResamplingMode ResamplingMode { get; set; }
        public uint ResamplingPrecision { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public Style Style { get; set; }
        public Style SelectedSeriesStyle { get; set; }

        public object DataContext { get; set; }
        public double ZeroLineY { get; set; }

        public string XAxisId
        {
            get => _xAxisId;
            set
            {
                if (SetValue(ref _xAxisId, value))
                    OnInvalidateParentSurface(nameof(XAxisId));
            }
        }

        public string YAxisId
        {
            get => _yAxisId;
            set
            {
                if (SetValue(ref _yAxisId, value))
                    OnInvalidateParentSurface(nameof(YAxisId));
            }
        }

        public IAxis XAxis
        {
            get => _xAxis;
            set
            {
                if (SetValue(ref _xAxis, value))
                    OnInvalidateParentSurface(nameof(XAxis));
            }
        }

        public IAxis YAxis
        {
            get => _yAxis;
            set
            {
                if (SetValue(ref _yAxis, value))
                    OnInvalidateParentSurface(nameof(YAxis));
            }
        }

        public Color Stroke
        {
            get => _stroke;
            set
            {
                if (SetValue(ref _stroke, value))
                    OnInvalidateParentSurface(nameof(Stroke));
            }
        }

        public double Opacity
        {
            get => _opacity;
            set
            {
                if (SetValue(ref _opacity, value))
                    OnInvalidateParentSurface(nameof(Stroke));
            }
        }

        public int StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                if (SetValue(ref _strokeThickness, value))
                    OnInvalidateParentSurface(nameof(StrokeThickness));
            }
        }

        public int SelectedStrokeThickness
        {
            get => _selectedStrokeThickness;
            set => SetValue(ref _selectedStrokeThickness, value);
        }

        public double[] StrokeDashArray
        {
            get => _strokeDashArray;
            set
            {
                if (SetValue(ref _strokeDashArray, value))
                    OnInvalidateParentSurface(nameof(StrokeDashArray));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (SetValue(ref _isSelected, value))
                    OnSelectionChanged();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (SetValue(ref _isVisible, value))
                    OnIsVisibleChanged();
            }
        }

        public IDataSeries DataSeries
        {
            get => _dataSeries;
            set
            {
                if (!Equals(_dataSeries, value))
                {
                    Dispatcher.BeginInvokeIfRequired(() =>
                    {
                        CurrentRenderPassData = null;
                        OnDataSeriesChanged(_dataSeries, value);
                        _dataSeries = value;
                    });
                }
            }
        }

        public IPointMarker PointMarker
        {
            get => _pointMarker;
            set
            {
                if (!Equals(_pointMarker, value))
                {
                    OnPointMarkerChanged(_pointMarker, value);
                    SetValue(ref _pointMarker, value);
                }
            }
        }

        public FrameworkElement RolloverMarker { get; protected set; }

        public ISeriesAnimation SeriesAnimation
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public IPaletteProvider PaletteProvider
        {
            get => _paletteProvider;
            set
            {
                if (SetValue(ref _paletteProvider, value))
                    OnInvalidateParentSurface(nameof(PaletteProvider));
            }
        }

        public IRenderPassData CurrentRenderPassData { get; set; }

        public LineDrawMode DrawNaNAs
        {
            get => _drawNaNAs;
            set
            {
                if (SetValue(ref _drawNaNAs, value))
                    OnInvalidateParentSurface(nameof(DrawNaNAs));
            }
        }

        public SlimLineRenderableSeries()
        {
            _xAxisId = AxisCore.DefaultAxisId;
            _yAxisId = AxisCore.DefaultAxisId;

            _opacity = 1;
            _isVisible = true;
            _stroke = Colors.White;

            _strokeThickness = 1;
            _selectedStrokeThickness = 4;

            ResamplingPrecision = default;
            ResamplingMode = ResamplingMode.Auto;

            AntiAliasing = true;
            DrawNaNAs = LineDrawMode.Gaps;

            HitTestProvider = new SlimLineHitTestProvider(this);
            DrawingProviders = new[] { new SlimLineDrawingProvider(this) };
        }

        private bool SetValue<T>(ref T property, T value)
        {
            if (Equals(property, value))
                return false;

            property = value;
            return true;
        }

        private void OnDataSeriesChanged(IDataSeries oldDataSeries, IDataSeries newDataSeries)
        {
            var parentSurface = GetParentSurface();

            if (parentSurface != null)
            {
                parentSurface.DetachDataSeries(oldDataSeries, this);
                parentSurface.AttachDataSeries(newDataSeries);
            }

            OnInvalidateParentSurface(nameof(DataSeries));
        }

        private void OnPointMarkerChanged(IPointMarker oldPointMarker, IPointMarker newPointMarker)
        {
            oldPointMarker?.Detach();
            newPointMarker.Attach(this);

            OnInvalidateParentSurface(nameof(PointMarker));
        }

        private void OnSelectionChanged()
        {
            if (IsSelected)
            {
                _cachedThickness = _strokeThickness;
                StrokeThickness = SelectedStrokeThickness;
            }
            else if (_cachedThickness.HasValue)
            {
                StrokeThickness = _cachedThickness.Value;
                _cachedThickness = null;
            }

            SelectionChanged?.Invoke(this, EventArgs.Empty);
            OnInvalidateParentSurface(nameof(IsSelected));
        }

        private void OnIsVisibleChanged()
        {
            IsVisibleChanged?.Invoke(this, EventArgs.Empty);
            OnInvalidateParentSurface(nameof(IsVisible));
        }

        public void OnInvalidateParentSurface(string propertyName = null)
        {
            DrawingProviders?.ForEachDo(x => x.OnNotifySeriesPropertyChanged(propertyName));

            Services?.GetService<ISciChartSurface>().InvalidateElement();
        }

        public ISciChartSurface GetParentSurface()
        {
            return Services?.GetService<ISciChartSurface>();
        }

        public IRange GetXRange()
        {
            return DataSeries?.XRange;
        }

        public IRange GetYRange(IRange xRange)
        {
            return GetYRange(xRange, false);
        }

        public IRange GetYRange(IRange xRange, bool getPositiveRange)
        {
            return DataSeries.GetWindowedYRange(xRange, getPositiveRange);
        }

        public IndexRange GetIndicesRange(IRange xRange)
        {
            return DataSeries?.GetIndicesRange(xRange);
        }

        public SeriesInfo GetSeriesInfo(HitTestInfo hitTestInfo)
        {
            return new XySeriesInfo(this, hitTestInfo);
        }

        public void OnAttached()
        {
        }

        public void OnDetached()
        {
        }

        void IDrawable.OnDraw(IRenderContext2D renderContext, IRenderPassData renderPassData)
        {
            CurrentRenderPassData = renderPassData;

            if (IsValidForDrawing() && !renderContext.ViewportSize.IsEmpty)
            {
                PaletteProvider?.OnBeginSeriesDraw(this);

                DrawingProviders.ForEachDo(dp =>
                {
                    dp.OnBeginRenderPass(renderContext);
                    dp.OnDraw(renderContext, renderPassData);
                });
            }
        }

        private bool IsValidForDrawing()
        {
            return IsVisible &&
                   DataSeries?.HasValues == true &&
                   CurrentRenderPassData?.PointSeries != null &&
                   ((Stroke.A != 0 && StrokeThickness > 0) || PointMarker != null || PaletteProvider != null);
        }

        public virtual bool ShowsTooltipForModifier(Func<BaseRenderableSeries, bool> func)
        {
            return true;
        }

        public IPointMarker GetPointMarker() => PointMarker;

        public XmlSchema GetSchema()
        {
            throw new NotSupportedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotSupportedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
