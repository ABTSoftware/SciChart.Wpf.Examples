// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomSeriesInfoProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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