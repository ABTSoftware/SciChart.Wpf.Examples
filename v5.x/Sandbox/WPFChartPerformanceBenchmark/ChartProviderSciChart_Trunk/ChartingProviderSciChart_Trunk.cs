using System;
using ChartProviders.Common;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Data;
using SciChart.Drawing.DirectX.Context.D3D11;
using SciChart.Drawing.HighQualityRasterizer;
using SciChart.Drawing.HighSpeedRasterizer;

namespace ChartProviderSciChart_Trunk
{
    public enum Renderer
    {
        DirectX, 
        SoftwareHS,
        SoftwareHQ,
    }

    public class ChartingProviderSciChart_Trunk : IChartingProvider
    {
        private readonly Type _selectedRenderSurface;

        public ChartingProviderSciChart_Trunk(Renderer whichRenderer)
        {
            _selectedRenderSurface = whichRenderer == Renderer.DirectX ? typeof(Direct3D11RenderSurface) :
               whichRenderer == Renderer.SoftwareHS ? typeof(HighSpeedRenderSurface) :
               typeof(HighQualityRenderSurface);                        
        }

        public ISpeedTest ScatterPointsSpeedTest()
		{
//            return new SciChartScatterSeries() { DataContext = new
//                {
//                    Version = Name,
//                    RenderSurface = _selectedRenderSurface.AssemblyQualifiedName,
//                    EnableExtremeResamplers = false,
//                } };

		    return new SciChartExtremeScatterSeries()
		    {
		        DataContext = new
		        {
		            Version = Name,
		            RenderSurface = _selectedRenderSurface.AssemblyQualifiedName,
		            EnableExtremeResamplers = false,
                    EnableExtremeDrawingManager = false,
		        }
		    };
        }

        public ISpeedTest FifoLineSpeedTest()
        {
            return new FifoLineSeriesSpeedTest() { DataContext = new { Version = Name, RenderSurface = _selectedRenderSurface.AssemblyQualifiedName, EnableExtremeResamplers = true, EnableExtremeDrawingManager = true,} };
        }

        public ISpeedTest LineAppendSpeedTest()
		{
            return new LineSeriesAppendingSpeedTest() { DataContext = new { Version = Name, RenderSurface = _selectedRenderSurface.AssemblyQualifiedName, EnableExtremeResamplers = true, EnableExtremeDrawingManager = true, } };
        }

        public ISpeedTest LoadNxNRefreshTest()
        {
            return new Load500x500SeriesRefreshTest() { DataContext = new {RenderSurface = _selectedRenderSurface.AssemblyQualifiedName, EnableExtremeResamplers = true, EnableExtremeDrawingManager = true, } };
        }

        public string Name
        {
            get { return "SciChart v5 (" + _selectedRenderSurface.Name + ")"; }
        }
    }
}
