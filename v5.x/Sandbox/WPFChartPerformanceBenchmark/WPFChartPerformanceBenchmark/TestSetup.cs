using ChartProviders.Common;
using ChartProviderSciChart_Trunk;
//using ChartProvider_LightningChart;

namespace WPFChartPerformanceBenchmark
{
    public class TestSetup
    {
        public static IChartingProvider[] GetChartProviders()
        {
           
            return new IChartingProvider[]
            {
                
                new ChartingProviderSciChart_Trunk(Renderer.DirectX), // Runs tests for SciChart with DirectX

                //new ChartingProviderLightningChart(),     // Runs tests for LightningChart Ultimate 

            };
        }
    }
}