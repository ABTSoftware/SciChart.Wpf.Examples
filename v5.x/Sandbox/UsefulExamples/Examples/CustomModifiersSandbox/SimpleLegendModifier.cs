using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.ChartData;
using SciChart.Charting.Utility;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Sandbox.ExampleSandbox.CustomModifiers
{
    /// <summary>
    /// Custom Modifier which demonstrates creating a legend data-source
    /// </summary>
    public class SimpleLegendModifier : ChartModifierBase
    {
        /// <summary>
        /// Defines the LegendData Dependency property
        /// </summary>
        public static readonly DependencyProperty LegendDataProperty = DependencyProperty.Register("LegendData", typeof(ChartDataObject), typeof(SimpleLegendModifier));

        public SimpleLegendModifier()
        {
            this.SetCurrentValue(LegendDataProperty, new ChartDataObject());
        }

        /// <summary>
        /// Gets or sets the <see cref="ChartDataObject"/> which may be bound to
        /// </summary>
        public ChartDataObject LegendData
        {
            get { return (ChartDataObject)GetValue(LegendDataProperty); }
            set { SetValue(LegendDataProperty, value); }
        }

        public override void OnParentSurfaceRendered(SciChartRenderedMessage e)
        {
            base.OnParentSurfaceRendered(e);

            // TODO here: Modify which RenderableSeries are passed in to GetSeriesInfo to alter output
            var seriesInfos = this.GetSeriesInfo(ParentSurface.RenderableSeries);

            // TODO here: if you experience flicker, it is a .NET4.5 bug. This occurs when updating an ObservableCollection
            // the solution believe it or not is to do a differential update, e.g. only change the SeriesInfo instance if a series was added or removed
            LegendData.SeriesInfo = new ObservableCollection<SeriesInfo>(seriesInfos);
        }

        private ObservableCollection<SeriesInfo> GetSeriesInfo(IEnumerable<IRenderableSeries> allSeries)
        {
            var seriesInfo = new ObservableCollection<SeriesInfo>();

            if (allSeries != null)
            {
                foreach (var renderableSeries in allSeries)
                {
                    if (renderableSeries == null || renderableSeries.DataSeries == null)
                    {
                        continue;
                    }

                    var hitResult = renderableSeries.DataSeries.HasValues
                                        ? renderableSeries.HitTest(new Point(ModifierSurface.ActualWidth, 0))
                                        : default(HitTestInfo);

                    var s = renderableSeries.GetSeriesInfo(hitResult);

                    seriesInfo.Add(s);
                }
            }

            return seriesInfo;
        }
    }
}