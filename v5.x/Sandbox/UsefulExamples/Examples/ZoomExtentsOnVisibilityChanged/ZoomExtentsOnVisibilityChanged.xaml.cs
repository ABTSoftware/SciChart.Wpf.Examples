using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples.ZoomExtentsOnVisibilityChanged
{
    [TestCase("Zoom Extents on Visibility Changed")]
    public partial class ZoomExtentsOnVisibilityChanged : Window
    {
        public ZoomExtentsOnVisibilityChanged()
        {
            InitializeComponent();            
        }
    }

    public class ZoomExtentsOnVisibilityChangedViewModel : BindableObject
    {
        private ObservableCollection<IRenderableSeriesViewModel> _series = new ObservableCollection<IRenderableSeriesViewModel>();
        public ObservableCollection<IRenderableSeriesViewModel> Series => _series;

        public ZoomExtentsOnVisibilityChangedViewModel()
        {
            _series.Add(new LineRenderableSeriesViewModel() { DataSeries = GetData(5,0.01), Stroke = Colors.LightSteelBlue});
            _series.Add(new LineRenderableSeriesViewModel() { DataSeries = GetData(2, 0.2), Stroke= Colors.Orange });
        }

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

    public class ZoomOnVisibilityBehaviour
    {
        public static readonly DependencyProperty ZoomExtentsOnVisibilityChangedProperty = DependencyProperty.RegisterAttached(
            "ZoomExtentsOnVisibilityChanged", typeof(bool), typeof(ZoomOnVisibilityBehaviour), new PropertyMetadata(default(bool), OnPropertyChanged));        

        public static void SetZoomExtentsOnVisibilityChanged(DependencyObject element, bool value)
        {
            element.SetValue(ZoomExtentsOnVisibilityChangedProperty, value);
        }

        public static bool GetZoomExtentsOnVisibilityChanged(DependencyObject element)
        {
            return (bool) element.GetValue(ZoomExtentsOnVisibilityChangedProperty);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == true)
            {
                var scs = ((SciChartSurface) d);

                scs.Loaded += (sender, args) =>
                {
                    foreach (var rs in scs.RenderableSeries)
                    {
                        rs.IsVisibleChanged += (_, __) =>
                        {
                            // Animate zoom etents when IsVisibleChanged 
                            scs.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
                        };
                    }
                };

                // TODO: If you want to handle all cases, use PropertyNotifier to listen to when scs.RenderableSeries has a new 
                // collection assigned, and then subscribe to scs.RenderableSeries.CollectionChanged in case series are added/removed
            }
        }
    }
}
