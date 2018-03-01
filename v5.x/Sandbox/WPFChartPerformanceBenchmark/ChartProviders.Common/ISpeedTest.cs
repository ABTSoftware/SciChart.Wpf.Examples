using System;
using System.Windows;

namespace ChartProviders.Common
{
    public interface ISpeedTest : IDisposable
    {
        /// <summary>
        /// Gets the chart element
        /// </summary>
        FrameworkElement Element { get; }

        /// <summary>
        /// Executes the test for the specified duration, callback with the fps result when done
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="fpsResult"></param>
        void Execute(TestParameters testParameters, TimeSpan duration, Action<double> fpsResult);
    }
}
