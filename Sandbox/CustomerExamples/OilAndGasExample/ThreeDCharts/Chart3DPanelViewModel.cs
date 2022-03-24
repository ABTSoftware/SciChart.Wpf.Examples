using OilAndGasExample.Common;
using OilAndGasExample.ThreeDCharts.ChartFactory;
using SciChart.Data.Model;

namespace OilAndGasExample.ThreeDCharts
{
    public class Chart3DPanelViewModel : BindableObject
    {
        public Chart3DViewModel Chart { get; }

        public Chart3DPanelViewModel()
        {
            Chart = new Chart3DViewModel(new ScatterChart3DFactory());
        }
    }
}