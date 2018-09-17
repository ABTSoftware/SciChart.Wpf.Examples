using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.Axis;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.PointMarkers;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples._3DChartChangePropertiesDynamically
{
    public class ChangePropertiesDynamicallyViewModel : BindableObject
    {
        private readonly NumericAxis3DViewModel _xAxis = new NumericAxis3DViewModel() { AxisTitleOffset=75, VisibleRange = new DoubleRange(0,20), StyleKey = "AxisStyle" };
        private readonly NumericAxis3DViewModel _yAxis = new NumericAxis3DViewModel() { AxisTitleOffset=75, VisibleRange = new DoubleRange(0,5), StyleKey = "AxisStyle" };
        private readonly NumericAxis3DViewModel _zAxis = new NumericAxis3DViewModel() { AxisTitleOffset=75, VisibleRange = new DoubleRange(0, 7), StyleKey = "AxisStyle" };
        private readonly ObservableCollection<IRenderableSeries3DViewModel> _series = new ObservableCollection<IRenderableSeries3DViewModel>();

        private string _chartTitle;
        private int _fontSize;
        private Brush _foreGround;        
        private Brush _gridlineColor;
        private DoubleRange _visibleRange;
        private string _axisTitle
            ;

        public ChangePropertiesDynamicallyViewModel()
        {
            var xyzData = new XyzDataSeries3D<double>();
            Enumerable.Range(0, 10).ForEachDo(x => xyzData.Append(x, x, x));
            _series.Add(new ScatterRenderableSeries3DViewModel()
            {
                DataSeries = xyzData,
                PointMarker = new SpherePointMarker3D() { Width=5, Height=5},
                Stroke = Colors.Green,
            });

            ChartTitle = "This is a test to see if binding works, yes it does";
            AxisTitle = "An Axis";
        }

        public ObservableCollection<IRenderableSeries3DViewModel> SeriesCollection3D => _series;

        public NumericAxis3DViewModel XAxis3D => _xAxis;
        public NumericAxis3DViewModel YAxis3D => _yAxis;
        public NumericAxis3DViewModel ZAxis3D => _zAxis;

        public string ChartTitle
        {
            get => _chartTitle;
            set
            {
                _chartTitle = value;
                OnPropertyChanged("ChartTitle");
            }            
        }

        public int FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        public Brush Foreground
        {
            get => _foreGround;
            set
            {
                _foreGround = value;
                OnPropertyChanged("Foreground");
            }
        }


        public Brush GridlineColor
        {
            get => _gridlineColor;
            set
            {
                _gridlineColor = value;
                OnPropertyChanged("GridlineColor");
            }
        }

        public DoubleRange VisibleRange
        {
            get => _visibleRange;
            set
            {
                _visibleRange = value;
                OnPropertyChanged("VisibleRange");
                XAxis3D.VisibleRange = value;
                YAxis3D.VisibleRange = value;
                ZAxis3D.VisibleRange = value;
            }
        }

        public string AxisTitle
        {
            get => _axisTitle;
            set
            {
                _axisTitle = value;
                OnPropertyChanged("AxisTitle");
                XAxis3D.AxisTitle = value;
                YAxis3D.AxisTitle = value;
                ZAxis3D.AxisTitle = value;
            }
        }
    }
}