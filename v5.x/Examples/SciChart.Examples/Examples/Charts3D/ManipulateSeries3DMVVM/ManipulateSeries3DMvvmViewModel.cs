using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Model.DataSeries.Waterfall;
using SciChart.Charting3D.PointMarkers;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;


namespace SciChart.Examples.Examples.Charts3D.ManipulateSeries3DMVVM
{
    public class ManipulateSeries3DMvvmViewModel : BaseViewModel
    {
        //private ColumnRenderableSeries3DViewModel _columnSeries;
        //private MountainRenderableSeries3DViewModel _mountainSeries;
        //private PointLineRenderableSeries3DViewModel _pointLineSeries;
        //private SurfaceMeshRenderableSeries3DViewModel _surfaceMeshSeries;
        //private WaterfallRenderableSeries3DViewModel _waterfallSeries;
        //private ScatterRenderableSeries3DViewModel _scatterSeries;
        //private ImpulseRenderableSeries3DViewModel _impulseSeries;

        private FasterRandom _random;
        private FFT2 _transform;

        private string _SelectedType;

        public ManipulateSeries3DMvvmViewModel()
        {
            _random = new FasterRandom();
            _transform = new FFT2();

            //_columnSeries = new ColumnRenderableSeries3DViewModel { ColumnShape = typeof(CylinderPointMarker3D), DataPointWidthX = 0.5, Opacity = 1.0, DataSeries = GetColumnDataSeries() };
            //_impulseSeries = new ImpulseRenderableSeries3DViewModel { PointMarker = new EllipsePointMarker3D { Fill = Colors.White, Size = 4, Opacity = 1 }, Opacity = 1.0, DataSeries = GetImpulseDataSeries() };
            //_pointLineSeries = new PointLineRenderableSeries3DViewModel { IsAntialiased = true, PointMarker = new EllipsePointMarker3D { Fill = Colors.LimeGreen, Size = 2.0f, Opacity = 1 }, DataSeries = GetPointLineDataSeries() };
            //_surfaceMeshSeries = new SurfaceMeshRenderableSeries3DViewModel { StyleKey = "SurfaceMeshStyle", DrawMeshAs = DrawMeshAs.SolidWireFrame, StrokeThickness = 2, DrawSkirt = false, Opacity = 1, DataSeries = GetSurfaceMeshDataSeries() };
            //_waterfallSeries = new WaterfallRenderableSeries3DViewModel { StyleKey = "WaterfallStyle", Stroke = Colors.Blue, Opacity = 0.8, StrokeThickness = 1, SliceThickness = 0, DataSeries = GetWaterfallDataSeries() };
            //_scatterSeries = new ScatterRenderableSeries3DViewModel { PointMarker = new EllipsePointMarker3D { Fill = Colors.LimeGreen, Size = 2.0f, Opacity = 1 }, DataSeries = GetScatterDataSeries() };
            //_mountainSeries = new MountainRenderableSeries3DViewModel { DataSeries = GetColumnDataSeries() };

            RenderableSeries = new ObservableCollection<IRenderableSeries3DViewModel>();

            SeriesTypes = new ObservableCollection<string>
            {
                "Column Series",
                "Impulse Series",
                "Mountain Series",
                "PointLine Series",
                "SurfaceMesh Series",
              //  "Waterfall Series",
                "Scatter Series"
            };

            SelectedType = SeriesTypes[1];

            AddCommand = new ActionCommand(() =>
            {
                RenderableSeries.Add(ViewModelFactory3D.New(GetSeriesType(SelectedType), 0));
                RemoveCommand.RaiseCanExecuteChanged();
            
            });

            RemoveCommand = new ActionCommand(() =>
            {
                var typeToDelete = GetSeriesType(SelectedType);
                RenderableSeries.Remove(RenderableSeries.Last());
                ClearCommand.RaiseCanExecuteChanged();
                RemoveCommand.RaiseCanExecuteChanged();
            }, () => RenderableSeries.Any());

            ClearCommand = new ActionCommand(() =>
            {
                RenderableSeries.Clear();
                ClearCommand.RaiseCanExecuteChanged();
            });
        }

        public ObservableCollection<IRenderableSeries3DViewModel> RenderableSeries { get; set; }
        public ObservableCollection<string> SeriesTypes { get; set; }

        public string SelectedType
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _SelectedType = value;
                    OnPropertyChanged("SelectedType");
                }
            }
            get { return _SelectedType; }
        }

        public ActionCommand AddCommand { get; private set; }
        public ActionCommand RemoveCommand { get; private set; }
        public ActionCommand ClearCommand { get; private set; }

        private Type GetSeriesType(string seriesType)
        {
            //RenderableSeries.Clear();

            if (seriesType.Equals("Column Series"))
            {
                return typeof(ColumnRenderableSeries3DViewModel);
            }
            else if (seriesType.Equals("Impulse Series"))
            {
                return typeof (ImpulseRenderableSeries3DViewModel);
            }
            else if (seriesType.Equals("Mountain Series"))
            {
                return typeof (MountainRenderableSeries3DViewModel);
            }
            else if (seriesType.Equals("PointLine Series"))
            {
                return typeof (PointLineRenderableSeries3DViewModel);
            }
            else if (seriesType.Equals("SurfaceMesh Series"))
            {
                return typeof (SurfaceMeshRenderableSeries3DViewModel);
            }
            // Temporary
            //else if (seriesType.Equals("Waterfall Series"))
            //{
            //    RenderableSeries.Add(_waterfallSeries);
            //}
            else if (seriesType.Equals("Scatter Series"))
            {
                return typeof (ScatterRenderableSeries3DViewModel);
            }

            throw  new ArgumentException("Not supported type!");
        }
    }
}
