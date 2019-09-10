using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples.TouchScreenModifiers
{
    public class TouchInputSandboxViewModel : BindableObject
    {
        private readonly ObservableCollection<IRenderableSeriesViewModel> _series;
        private readonly ObservableCollection<IAnnotationViewModel> _annotations;

        public TouchInputSandboxViewModel()
        {
            _series = new ObservableCollection<IRenderableSeriesViewModel>();
            _annotations = new ObservableCollection<IAnnotationViewModel>();

            _series.Add(new LineRenderableSeriesViewModel()
            {
                DataSeries = GetSomeData(),
                PointMarker = new EllipsePointMarker() {  Width = 7, Height=7, Stroke = Colors.White}
            });

            _annotations.Add(new TextAnnotationViewModel()
            {
                HorizontalAnchorPoint = HorizontalAnchorPoint.Center,
                VerticalAnchorPoint = VerticalAnchorPoint.Center,
                IsEditable = false,
                CoordinateMode = AnnotationCoordinateMode.Relative,
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x66, 0x00)),
                X1 =0.5,
                Y1 =0.5,
                Text ="This chart should be interactive via Touch or Mouse"
            });
        }

        private IDataSeries GetSomeData()
        {
            var xyData = new XyDataSeries<double>();
            for (int i = 0; i < 100; i++)
            {
                xyData.Append(i, Math.Sin(i*0.05));    
            }
            return xyData;
        }

        public ObservableCollection<IRenderableSeriesViewModel> Series => _series;
        public ObservableCollection<IAnnotationViewModel> Annotations => _annotations;
    }
}