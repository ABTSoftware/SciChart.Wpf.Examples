using ChartProviders.Common;
using ChartProviderSciChart_Trunk;

namespace WPFChartPerformanceBenchmark
{
    public class TestSetup
    {
        public static IChartingProvider[] GetChartProviders()
        {
            return new IChartingProvider[]
            {
                // Runs tests for SciChart with DirectX
                new ChartingProviderSciChart_Trunk(Renderer.DirectX),                                       

                // Runs tests for SciChart with Software (High Speed)
                //new ChartingProviderSciChart_Trunk(Renderer.SoftwareHS),

                // Runs tests for SciChart with Software (High Quality)
                //new ChartingProviderSciChart_Trunk(Renderer.SoftwareHQ),   
            };
        }
    }
}