using System;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.AnnotateAChart.OverlayTradeMarkers
{
    public interface IBuySellMarkerAnnotation : IAnnotationViewModel
    {
        Trade TradeData { get; set; }
    }

    // A viewmodel to display a BuyMarkerAnnotation and provide properties to bind to
    public class BuyMarkerAnnotationViewModel : BaseAnnotationViewModel, IBuySellMarkerAnnotation
    {
        private Trade _tradeData;
        public override Type ViewType { get { return typeof(BuyMarkerAnnotation); } }

        public Trade TradeData
        {
            get { return _tradeData; }
            set
            {
                _tradeData = value;
                OnPropertyChanged("TradeData");
            }
        }
    }

    // A viewmodel to display a SellMarkerAnnotation and provide properties to bind to
    public class SellMarkerAnnotationViewModel : BaseAnnotationViewModel, IBuySellMarkerAnnotation
    {
        private Trade _tradeData;
        public override Type ViewType { get { return typeof(SellMarkerAnnotation); } }

        public Trade TradeData
        {
            get { return _tradeData; }
            set
            {
                _tradeData = value;
                OnPropertyChanged("TradeData");
            }
        }
    }
}