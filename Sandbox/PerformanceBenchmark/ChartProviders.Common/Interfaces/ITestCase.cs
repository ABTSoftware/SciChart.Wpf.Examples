using System;
using System.Windows.Controls;

namespace ChartProviders.Common.Interfaces
{
    /// <summary>
    /// An individual test. Returns some measure of performance.
    /// </summary>
    public interface ITestCase
    {
        string TestName { get; }

        string ChartProviderName { get; }

        /// <summary>
        /// Sets the chart provider for this test.
        /// </summary>
        void SetChartProvider(IChartProvider chartProvider);

        /// <summary>
        /// Sets up this test, providing the panel that host it. The test case
        /// will add its visuals to the panel on setup, and remove them on completion.
        /// </summary>
        void Setup(Panel layoutRoot);

        /// <summary>
        /// Checks if the test can be executed.
        /// </summary>
        bool CanExecute();

        /// <summary>
        /// Executes the test, invoking the complete action on completion.
        /// </summary>
        /// <param name="complete"></param>
        void Execute(Action<double> complete);
    }
}
