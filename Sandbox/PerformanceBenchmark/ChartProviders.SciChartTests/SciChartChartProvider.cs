using System.Diagnostics;
using System.Text.RegularExpressions;
using ChartProviders.Common.Interfaces;
using SciChart.Charting.Visuals;
using SciChart.Core.Extensions;

namespace ChartProviders.SciChartTests
{
    public enum Renderer
    {
        VisualXccelerator,
        SoftwareHighSpeed,
        SoftwareHighQuality
    }

    public class SciChartChartProvider : IChartProvider
    {
        private readonly string _selectedRenderSurface;

        private const bool EnableExtremeResamplers = true;
        private const bool EnableExtremeDrawingManager = true;

        public string Name => $"SciChart {GetVersion()} ({_selectedRenderSurface})";

        public SciChartChartProvider(Renderer whichRenderer)
        {
#if XPF
            _selectedRenderSurface = "VisualXcceleratorRenderSurface";
#else
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
#endif
        }

        public ISpeedTest ScatterPointsSpeedTest()
        {
            return new ExtremeScatterSeriesSpeedTest
            {
                DataContext = new
                {
                    Version = Name,
                    RenderSurface = _selectedRenderSurface,
                    EnableExtremeResamplers,
                    EnableExtremeDrawingManager
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
                    EnableExtremeResamplers,
                    EnableExtremeDrawingManager
                }
            };
        }

        public ISpeedTest LineAppendSpeedTest()
        {
            return new LineSeriesAppendSpeedTest
            {
                DataContext = new
                {
                    Version = Name,
                    RenderSurface = _selectedRenderSurface,
                    EnableExtremeResamplers,
                    EnableExtremeDrawingManager
                }
            };
        }

        public ISpeedTest LoadNxNRefreshTest()
        {
            return new Load500x500SeriesSpeedTest
            {
                DataContext = new
                {
                    RenderSurface = _selectedRenderSurface,
                    EnableExtremeResamplers,
                    EnableExtremeDrawingManager
                }
            };
        }

        private static string GetVersion()
        {
            var sciChartVersion = SciChartSurface.VersionInfo;
            var sciChartAssembly = typeof(SciChartSurface).GetAssembly();

            if (sciChartAssembly != null)
            {
                var fileLocation = sciChartAssembly.Location;
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(fileLocation);

                if (fileVersionInfo != null)
                {
                    var regexPattern = "^([0-9]+\\.[0-9]+\\.[0-9]+\\.[0-9]+)(-beta)?.*$";
                    var regexMatch = Regex.Match(fileVersionInfo.ProductVersion, regexPattern);

                    if (regexMatch.Success && regexMatch.Groups?.Count >= 3)
                    {
                        var version = regexMatch.Groups[1].Value;
                        var betaFlag = regexMatch.Groups[2].Value;

                        sciChartVersion = $"v{version}{betaFlag}";
                    }
                }
            }

            return sciChartVersion;
        }
    }
}