// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomModifierSandboxViewModel.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using Shared;

namespace SciChart.Wpf.TestSuite.ExampleSandbox.CustomModifiers
{
    public class CustomModifierSandboxViewModel : BindableObject
    {
        private IDictionary<IRenderableSeries, List<DataPoint>> _selectedDataPoints;
        private XyDataSeries<DateTime, double> _bidDataSeries = new XyDataSeries<DateTime, double>() { SeriesName = "Bid"};
        private XyDataSeries<DateTime, double> _offerDataSeries = new XyDataSeries<DateTime, double>() { SeriesName = "Offer" };

        public CustomModifierSandboxViewModel()
        {
            // Generate some data
            var r = new RandomWalkGenerator();
            var data = r.GetRandomWalkSeries(500);
            _bidDataSeries.Append(data.XData.Select(x => DateTime.Today.AddDays(x)), data.YData);

            r = new RandomWalkGenerator();
            data = r.GetRandomWalkSeries(500);
            _offerDataSeries.Append(data.XData.Select(x => DateTime.Today.AddDays(x)), data.YData);
        }

        public XyDataSeries<DateTime, double> BidDataSeries { get { return _bidDataSeries; } }

        public XyDataSeries<DateTime, double> OfferDataSeries { get { return _offerDataSeries; } } 


        /// <summary>
        /// Gets or sets the SelectedDataPoints. This is bound to SimpleDataPointSelectionModifier.SelectedPoints
        /// </summary>
        public IDictionary<IRenderableSeries, List<DataPoint>> SelectedDataPoints
        {
            get { return _selectedDataPoints; }
            set
            {
                if (value != _selectedDataPoints)
                {
                    _selectedDataPoints = value;
                    OnPropertyChanged("SelectedDataPoints");

                    // TODO HERE: React to Selection Changed if you want 
                }
            }
        }
    }
}
