using System;
using System.Collections.Generic;
using System.Linq;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace CustomModifierSandboxExample
{
    public class CustomModifierSandboxViewModel : BindableObject
    {
        private IDictionary<IRenderableSeries, List<DataPoint>> _selectedDataPoints;

        public XyDataSeries<DateTime, double> BidDataSeries { get; }

        public XyDataSeries<DateTime, double> OfferDataSeries { get; }

        public CustomModifierSandboxViewModel()
        {
            // Generate some data
            var r = new RandomWalkGenerator();
            var data = r.GetRandomWalkSeries(500);

            BidDataSeries = new XyDataSeries<DateTime, double>() { SeriesName = "Bid" };
            BidDataSeries.Append(data.XData.Select(DateTime.Today.AddDays), data.YData);

            r = new RandomWalkGenerator();
            data = r.GetRandomWalkSeries(500);

            OfferDataSeries = new XyDataSeries<DateTime, double>() { SeriesName = "Offer" };
            OfferDataSeries.Append(data.XData.Select(DateTime.Today.AddDays), data.YData);
        }

        /// <summary>
        /// Gets or sets the SelectedDataPoints. This is bound to SimpleDataPointSelectionModifier.SelectedPoints
        /// </summary>
        public IDictionary<IRenderableSeries, List<DataPoint>> SelectedDataPoints
        {
            get => _selectedDataPoints;
            set
            {
                if (_selectedDataPoints != value)
                {
                    _selectedDataPoints = value;

                    OnPropertyChanged(nameof(SelectedDataPoints));

                    // HERE: React to Selection Changed if you want 
                }
            }
        }
    }
}
