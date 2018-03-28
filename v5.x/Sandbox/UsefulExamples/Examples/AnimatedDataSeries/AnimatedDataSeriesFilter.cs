using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Core.Utility;

namespace SciChart.Sandbox.Examples.AnimatedDataSeries
{
    public class AnimatedDataSeriesFilter : FilterBase
    {
        private readonly XyDataSeries<double, double> _originalDataSeries;
        private readonly XyDataSeries<double, double> _filteredDataSeries = new XyDataSeries<double, double>();

        public AnimatedDataSeriesFilter(XyDataSeries<double, double> originalDataSeries) : base(originalDataSeries)
        {
            _originalDataSeries = originalDataSeries;

            FilteredDataSeries = _filteredDataSeries;

            FilterAll();
        }

        public override void FilterAll()
        {
            _filteredDataSeries.Clear();

            int index = 0;
            int animationStepMillisconds = 10;

            Action appendPoint = null;

            Action onAppendCallback = () =>
            {
                // 2.) Append the point
                _filteredDataSeries.Append(_originalDataSeries.XValues[index], _originalDataSeries.YValues[index]);
                _filteredDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);

                // 3.) Schedule another until complete
                if (++index < _originalDataSeries.Count)
                {
                    appendPoint();
                }
            };

            appendPoint = () =>
            {                
                TimedMethod.Invoke(onAppendCallback).After(animationStepMillisconds).Go();
            };

            // 1.) Schedule one point to be appended
            appendPoint();
        }
    }
}