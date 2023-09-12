using System.Windows.Media;
using SciChart.Charting.Visuals;

namespace DpiAware_SciChartSurface;

public struct DpiInfo
{
    public double DpiScaleX;
    public double DpiScaleY;
}

public interface IDpiAwarenessHelper
{
    DpiInfo GetDpiScale(ISciChartSurface sciChartSurace);
}

internal class DpiAwarenessHelper : IDpiAwarenessHelper
{
    public static IDpiAwarenessHelper Instance = new DpiAwarenessHelper();

    private DpiAwarenessHelper() { }

    public DpiInfo GetDpiScale(ISciChartSurface sciChartSurace)
    {
        var dpiInfo = new DpiInfo { DpiScaleX = 1.0, DpiScaleY = 1.0 };

        if (sciChartSurace is Visual visual)
        {
            var res = VisualTreeHelper.GetDpi(visual);
            dpiInfo.DpiScaleY = res.DpiScaleY;
            dpiInfo.DpiScaleX = res.DpiScaleX;
        }

        return dpiInfo;
    }
}