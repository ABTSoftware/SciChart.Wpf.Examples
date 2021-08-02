using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;

namespace SciChartSurfaceMeshSelection
{

    public class SurfaceMeshMetadataPaletteProvider : ISurfaceMeshPaletteProvider3D
    {
        public int ZSize { get; set; }
        public int XSize { get; set; }

        public List<SurfaceMeshVertexInfo> SelectedIndexes { get; set; } = new List<SurfaceMeshVertexInfo>();

        public IBaseGridDataSeries3D DataSeries { get; set; }

        public void OnAttach(IRenderableSeries3D renderSeries)
        {
            var ds = renderSeries.DataSeries as UniformGridDataSeries3D<double>;
            DataSeries = ds;

            XSize = ds.XSize;
            ZSize = ds.ZSize;
        }

        public void OnDetached()
        {
            
        }

        public Color? OverrideCellColor(IRenderableSeries3D series, int xIndex, int zIndex)
        {
            if (SelectedIndexes.Any(x => x.XIndex == xIndex && x.ZIndex == zIndex))
            {
                return Colors.White;
            }
            else
            {
                return Colors.Red;
            }
        }
    }
}
