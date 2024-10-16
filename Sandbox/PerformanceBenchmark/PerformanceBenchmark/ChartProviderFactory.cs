using ChartProviders.Common.Interfaces;
using ChartProviders.SciChartTests;
using SciChart.Charting;

namespace PerformanceBenchmark
{
    public static class ChartProviderFactory
    {
        public static IChartProvider[] GetChartProviders()
        {
            VisualXcceleratorEngine.UseAutoShutdown = false;

            return new IChartProvider[]
            {
                // Runs tests for SciChart with Visual Xccelerator
                new SciChartChartProvider(Renderer.VisualXccelerator),                                       

                // Runs tests for SciChart with Software (High Speed)
                // new SciChartChartProvider(Renderer.SoftwareHighSpeed),

                // Runs tests for SciChart with Software (High Quality)
                // new SciChartChartProvider(Renderer.SoftwareHighQuality)
            };
        }
    }
}