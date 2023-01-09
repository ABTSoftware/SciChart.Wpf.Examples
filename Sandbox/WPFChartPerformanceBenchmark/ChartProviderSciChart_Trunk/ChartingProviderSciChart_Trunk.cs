using System.Reflection;
using ChartProviders.Common;
using SciChart.Charting.Visuals;

namespace ChartProviderSciChart_Trunk
{
    public enum Renderer
    {
        VisualXccelerator,
        SoftwareHighSpeed,
        SoftwareHighQuality
    }

    public class ChartingProviderSciChart_Trunk : IChartingProvider
    {
        private readonly string _selectedRenderSurface;

        private const bool _enableExtremeResamplers = true;
        private const bool _enableExtremeDrawingManager = true;

        public ChartingProviderSciChart_Trunk(Renderer whichRenderer)
        {
            if (whichRenderer == Renderer.VisualXccelerator)
            {
                _selectedRenderSurface = "VisualXcceleratorRenderSurface";
            }
            else if (whichRenderer == Renderer.SoftwareHighSpeed)
            {
                _selectedRenderSurface = "HighSpeedRenderSurface";
            }
            else
            {
                _selectedRenderSurface = "HighQualityRenderSurface";
            }                      
        }

        public ISpeedTest ScatterPointsSpeedTest()
		{
            return new SciChartExtremeScatterSeries
		    {
		        DataContext = new
		        {
		            Version = Name,
		            RenderSurface = _selectedRenderSurface,
		            EnableExtremeResamplers = _enableExtremeResamplers,
                    EnableExtremeDrawingManager = _enableExtremeDrawingManager,
		        }
		    };
        }

        public ISpeedTest FifoLineSpeedTest()
        {
            return new FifoLineSeriesSpeedTest
            {
                DataContext = new
                {
                    Version = Name,
                    RenderSurface = _selectedRenderSurface,
                    EnableExtremeResamplers = _enableExtremeResamplers,
                    EnableExtremeDrawingManager = _enableExtremeDrawingManager,
                }
            };
        }

        public ISpeedTest LineAppendSpeedTest()
        {
            return new LineSeriesAppendingSpeedTest
            {
                DataContext = new
                {
                    Version = Name,
                    RenderSurface =_selectedRenderSurface,
                    EnableExtremeResamplers = _enableExtremeDrawingManager,
                    EnableExtremeDrawingManager = _enableExtremeDrawingManager,
                }
            };
        }

        public ISpeedTest LoadNxNRefreshTest()
        {
            return new Load500x500SeriesRefreshTest
            {
                DataContext = new
                {
                    RenderSurface = _selectedRenderSurface,
                    EnableExtremeResamplers = _enableExtremeDrawingManager,
                    EnableExtremeDrawingManager = _enableExtremeDrawingManager,
                }
            };
        }

        public string Name => $"SciChart {Assembly.GetAssembly(typeof(SciChartSurface)).GetName().Version} ({_selectedRenderSurface})";    
    }
}