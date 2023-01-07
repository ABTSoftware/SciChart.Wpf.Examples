using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Core.Utility;

namespace AnimatedDataSeriesFilterExample
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
            double animationStepMillisconds = 1;

            Action appendPoint = null;

            Action onAppendCallback = () =>
            {
                // 2.) Append the point
                _filteredDataSeries.Append(_originalDataSeries.XValues[index], _originalDataSeries.YValues[index]);
                _filteredDataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);

                // 3.) Schedule another until complete
                if (++index < _originalDataSeries.Count)
                {
                    // Achieve some rudimentary easing 
                    animationStepMillisconds *= 1.05;
                    animationStepMillisconds = Math.Min(animationStepMillisconds, 10);

                    // Next point 
                    appendPoint();                    
                }
            };

            appendPoint = () =>
            {                
                TimedMethod.Invoke(onAppendCallback).After((int)animationStepMillisconds).Go();
            };

            // 1.) Schedule one point to be appended
            appendPoint();
        }
    }
}