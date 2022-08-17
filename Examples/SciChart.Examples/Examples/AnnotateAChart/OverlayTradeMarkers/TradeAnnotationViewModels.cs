// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
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
using SciChart.Charting.Model.ChartSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.AnnotateAChart.OverlayTradeMarkers
{
    public interface IBuySellAnnotationViewModel : IAnnotationViewModel
    {
        Trade TradeData { get; set; }
    }

    // Viewmodel for the annotation type NewsBulletAnnotation
    public class NewsBulletAnnotationViewModel : BaseAnnotationViewModel
    {
        private NewsEvent _newsEvent;

        public NewsEvent NewsData
        {
            get { return _newsEvent; }
            set
            {
                _newsEvent = value;
                OnPropertyChanged("NewsData");
            }
        }

        public override Type ViewType { get { return typeof(NewsBulletAnnotation); } }
    }

    // Viewmodel for the annotation type BuyMarkerAnnotation
    public class BuyMarkerAnnotationViewModel : BaseAnnotationViewModel, IBuySellAnnotationViewModel
    {
        private Trade _tradeData;

        public Trade TradeData
        {
            get { return _tradeData; }
            set
            {
                _tradeData = value;
                OnPropertyChanged("TradeData");
            }
        }
        public override Type ViewType { get { return typeof(BuyMarkerAnnotation); } }
    }

    // Viewmodel for the annotation type SellMarkerAnnotation
    public class SellMarkerAnnotationViewModel : BaseAnnotationViewModel, IBuySellAnnotationViewModel
    {
        private Trade _tradeData;

        public Trade TradeData
        {
            get { return _tradeData; }
            set
            {
                _tradeData = value;
                OnPropertyChanged("TradeData");
            }
        }
        public override Type ViewType { get { return typeof(SellMarkerAnnotation); } }
    }
}