using System;
using System.Collections.Generic;
using System.Linq;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Sandbox.Shared;

namespace SciChart.Sandbox.ExampleSandbox.CustomModifiers
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
