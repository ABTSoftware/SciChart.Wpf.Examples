using SciChart.Charting.Model.ChartData;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.InspectDatapoints.CustomSeriesValueMarkers
{
    public class CustomSeriesInfoProvider : DefaultSeriesInfoProvider
    {
        public override void OnAppendDataColumnInfo(SeriesInfo seriesInfo, HitTestInfo hitTestInfo)
        {
            if (seriesInfo.RenderableSeries is FastCandlestickRenderableSeries candleSeries)
            {
                var dataSeries = candleSeries.DataSeries as IOhlcDataSeries;

                seriesInfo.DataColumnInfo.Add(new DataColumnInfo(candleSeries)
                {
                    DataColumnName = "Open",
                    ColorSource = candleSeries.StrokeDown,
                    LastDataValue = DataColumnInfo.GetLastDataValue(dataSeries?.OpenValues),
                    HitTestValue = hitTestInfo.OpenValue
                });

                seriesInfo.DataColumnInfo.Add(new DataColumnInfo(candleSeries)
                {
                    DataColumnName = "Close",
                    ColorSource = candleSeries.StrokeUp,
                    LastDataValue = DataColumnInfo.GetLastDataValue(dataSeries?.CloseValues),
                    HitTestValue = hitTestInfo.CloseValue
                });
            }
        }
    }
}