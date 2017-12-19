using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;

namespace SciChart.Examples.Examples.FiltersAPI
{
    /// <summary>
    /// To create a custom filter, inherit FilterBase and override FilterALl. For best performance, you can also override FilterOnAppend
    /// </summary>
    public class CustomFilter : FilterBase
    {
        private readonly XyDataSeries<TimeSpan, double> _originalDataSeries;
        private readonly XyDataSeries<TimeSpan, double> _filteredDataSeries = new XyDataSeries<TimeSpan, double>();

        public CustomFilter(XyDataSeries<TimeSpan,double> originalDataSeries) : base(originalDataSeries)
        {
            _originalDataSeries = originalDataSeries;

            // Store a reference in the base class to the FilteredDataSeries
            FilteredDataSeries = _filteredDataSeries;

            // Update the filter
            FilterAll();
        }

        protected override void FilterOnAppend(int index)
        {
            // Override FilterOnAppend to update just the latest data-point in _filteredDataSeries for a more efficient filter
            base.FilterOnAppend(index);
        }

        protected override void FilterOnInsert(int startIndex, int count)
        {
            // Override FilterOnInsert to update just the inserted data-point in _filteredDataSeries for a more efficient filter
            base.FilterOnInsert(startIndex, count);
        }

        protected override void FilterOnRemove(int startIndex, int count)
        {
            // Override FilterOnRemove to update just the removed data-point in _filteredDataSeries for a more efficient filter
            base.FilterOnRemove(startIndex, count);
        }

        protected override void FilterOnUpdate(int index)
        {
            // Override FilterOnUpdate to update just the updated data-point in _filteredDataSeries for a more efficient filter
            base.FilterOnUpdate(index);
        }

        public override void FilterAll()
        {
            // When FilterAll is called, recreate the FilteredDataSeries and apply the filtering. 
            _filteredDataSeries.Clear();
            _filteredDataSeries.Append(_originalDataSeries.XValues[0], _originalDataSeries.YValues[0]);

            const double beta = 0.2;

            // Implementing a simple low pass filter https://kiritchatterjee.wordpress.com/2014/11/10/a-simple-digital-low-pass-filter-in-c/
            for (int i = 1; i < _originalDataSeries.Count; i++)
            {
                TimeSpan xValue = _originalDataSeries.XValues[i];
                double yValue = beta * _originalDataSeries.YValues[i] + (1 - beta) * _filteredDataSeries.YValues[i - 1];
                _filteredDataSeries.Append(xValue, yValue);
            }
        }
    }
}