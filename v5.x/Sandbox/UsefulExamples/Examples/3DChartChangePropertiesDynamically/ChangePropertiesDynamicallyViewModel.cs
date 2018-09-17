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
        private readonly NumericAxis3DViewModel _xAxis = new NumericAxis3DViewModel() { VisibleRange = new DoubleRange(0,10), StyleKey = "AxisStyle" };
        private readonly NumericAxis3DViewModel _yAxis = new NumericAxis3DViewModel() { VisibleRange = new DoubleRange(0,10), StyleKey = "AxisStyle" };
        private readonly NumericAxis3DViewModel _zAxis = new NumericAxis3DViewModel() { VisibleRange = new DoubleRange(0, 10), StyleKey = "AxisStyle" };
        private readonly ObservableCollection<IRenderableSeries3DViewModel> _series = new ObservableCollection<IRenderableSeries3DViewModel>();

        private string _chartTitle;
        private int _fontSize;
        private Brush _foreGround;        
        private Brush _gridlineColor;

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
    }
}