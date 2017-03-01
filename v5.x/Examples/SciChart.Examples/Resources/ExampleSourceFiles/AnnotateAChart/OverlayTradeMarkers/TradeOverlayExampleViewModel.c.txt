// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// TradeOverlayExampleViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;

using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.AnnotateAChart.OverlayTradeMarkers
{
    public class TradeOverlayExampleViewModel : BaseViewModel
    {
        private IDataSeries<DateTime, double> _chartDataSeries;
        private AnnotationCollection _annotations;
        private DefaultViewportManager _viewportManager;

        public TradeOverlayExampleViewModel()
        {
            // Create some data to show on the chart            
            _chartDataSeries = new XyDataSeries<DateTime, double>();
            _chartDataSeries.SeriesName = "CL FUT JUN15 2013";

            // Get some price data, trades
            List<Trade> trades;
            List<NewsEvent> newsEvents;
            var priceData = DataManager.Instance.GetRandomTrades(out trades, out newsEvents);

            // Buffer above and append all in one go to avoid multiple recalculations of series range
            _chartDataSeries.Append(priceData.TimeData, priceData.CloseData);

            // Create annotations           
            TradeAnnotations = CreateAnnotations(trades, newsEvents);

            _chartDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }

        public AnnotationCollection TradeAnnotations
        {
            get { return _annotations; }
            set 
            { 
                _annotations = value;
                OnPropertyChanged("TradeAnnotations");
            }
        }

        public IDataSeries<DateTime, double> ChartDataSeries
        {
            get { return _chartDataSeries; }
            set
            {
                _chartDataSeries = value;
                OnPropertyChanged("ChartDataSeries");
            }
        }        

        private static AnnotationCollection CreateAnnotations(IEnumerable<Trade> trades, List<NewsEvent> newsEvents)
        {
            var annotations = new AnnotationCollection();
            foreach (var trade in trades)
            {
                IAnnotation annotation = trade.BuySell == BuySell.Buy ? new BuyMarkerAnnotation() : (IAnnotation)new SellMarkerAnnotation();

                // The datacontext allows the tooltip inside the buy or sell marker to bind to elements of the Trade
                annotation.DataContext = trade;

                // X1,Y1 we set up manually
                annotation.X1 = trade.TradeDate;
                annotation.Y1 = trade.DealPrice;

                annotations.Add(annotation);
            }

            foreach(var newsEvent in newsEvents)
            {
                var annotation = new NewsBulletAnnotation();

                // The datacontext allows the tooltip to bind to news data
                annotation.DataContext = newsEvent;

                // X1 is equal to the news date
                annotation.X1 = newsEvent.EventDate;

                // Y1 is set to 0.99, which is the just inside the vertical bottom edge of the chart in relative mode
                annotation.Y1 = 0.99;

                // Finally we use CoordinateMode.RelativeY to have a fractional coordinate on Y-Axis
                // this ensures the news bullets always appear at the bottom of the chart regardless
                // of YAxis.VisibleRange
                annotation.CoordinateMode = AnnotationCoordinateMode.RelativeY;

                annotations.Add(annotation);
            }
            return annotations;
        }
    }
}
