using ChartProviders.Common;
using SciChart.Drawing.DirectX.Context.D3D11;
using SciChart.Drawing.HighQualityRasterizer;
using SciChart.Drawing.HighSpeedRasterizer;
using System;
using System.Reflection;
using SciChart.Charting.Visuals;

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

        private const bool _enableExtremeResamplers = true;
        private const bool _enableExtremeDrawingManager = true;

        public ChartingProviderSciChart_Trunk(Renderer whichRenderer)
        {
            if (whichRenderer == Renderer.DirectX)
            {
                _selectedRenderSurface = typeof(Direct3D11RenderSurface);
            }
            else if (whichRenderer == Renderer.SoftwareHS)
            {
                _selectedRenderSurface = typeof(HighSpeedRenderSurface);
            }
            else
            {
                _selectedRenderSurface = typeof(HighQualityRenderSurface);
            }                      
        }

        public ISpeedTest ScatterPointsSpeedTest()
		{
            //return new SciChartScatterSeries
            //{
            //    DataContext = new
            //    {
            //        Version = Name,
            //        RenderSurface = _selectedRenderSurface.AssemblyQualifiedName,
            //        EnableExtremeResamplers = false,
            //    }
            //};

            return new SciChartExtremeScatterSeries
		    {
		        DataContext = new
		        {
		            Version = Name,
		            RenderSurface = _selectedRenderSurface.AssemblyQualifiedName,
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
                    RenderSurface = _selectedRenderSurface.AssemblyQualifiedName,
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
                    RenderSurface =_selectedRenderSurface.AssemblyQualifiedName,
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
                    RenderSurface = _selectedRenderSurface.AssemblyQualifiedName,
                    EnableExtremeResamplers = _enableExtremeDrawingManager,
                    EnableExtremeDrawingManager = _enableExtremeDrawingManager,
                }
            };
        }

        public string Name => $"SciChart {Assembly.GetAssembly(typeof(SciChartSurface)).GetName().Version} ({_selectedRenderSurface.Name})";    
    }
}