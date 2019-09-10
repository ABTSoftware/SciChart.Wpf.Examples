using System;
using System.Windows.Controls;

namespace ChartProviders.Common
{
    /// <summary>
    /// An individual test. Returns some measure of performance.
    /// </summary>
    public interface ITestCase
    {
        /// <summary>
        /// Sets the chart provider for this test
        /// </summary>
        void SetChartingProvider(IChartingProvider chartingProvider);

        /// <summary>
        /// Sets up this test, providing the panel that host it. The test case
        /// will add its visuals to the panel on setup, and remove them on completion.
        /// </summary>
        void SetUp(Panel layoutRoot);

        /// <summary>
        /// Executes the test, invoking the complete action on completion
        /// </summary>
        /// <param name="complete"></param>
        void Execute(Action<double> complete);

        string TestName { get; }

        string ChartingProviderName { get; }

        bool CanExecute();

    }
}
