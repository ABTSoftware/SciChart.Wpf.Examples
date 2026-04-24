using SciChart.Charting.Model.ChartSeries;
using SciChart.Data.Model;

namespace SciChart.Mvvm.Tutorial
{
    /// <summary>
    /// Converts between Fahrenheit (this axis's range) and Celsius (the canonical group range).
    /// Attach this transform to the Fahrenheit axis. The Celsius axis needs no transform
    /// because its range IS the group range (identity).
    /// </summary>
    public class CelsiusFahrenheitTransform : IRangeSyncTransform
    {
        /// <summary>
        /// Converts the axis range (Fahrenheit) to the canonical group range (Celsius).
        /// Called when THIS axis changes and the new range must be broadcast to the group.
        /// </summary>
        public IRange ToGroupRange(IRange axisRange)
        {
            if (axisRange is DoubleRange r)
            {
                return new DoubleRange(
                    (r.Min - 32) * 5.0 / 9.0,
                    (r.Max - 32) * 5.0 / 9.0);
            }

            return axisRange;
        }

        /// <summary>
        /// Converts the canonical group range (Celsius) to the axis range (Fahrenheit).
        /// Called when ANOTHER axis in the group changes and this axis must update.
        /// </summary>
        public IRange FromGroupRange(IRange groupRange)
        {
            if (groupRange is DoubleRange r)
            {
                return new DoubleRange(
                    r.Min * 9.0 / 5.0 + 32,
                    r.Max * 9.0 / 5.0 + 32);
            }

            return groupRange;
        }
    }
}
