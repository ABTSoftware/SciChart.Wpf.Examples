using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Initialize the chart. 
            // License is initialized in Program.cs
            var scs = new SciChartSurface();
            scs.XAxis = new NumericAxis();
            scs.YAxis = new NumericAxis();

            var xyData = new XyDataSeries<double>();
            Enumerable.Range(0, 100).ForEachDo(x => xyData.Append(x, Math.Sin(x * 0.05)));
            scs.RenderableSeries.Add(new FastLineRenderableSeries() { DataSeries = xyData, Stroke = Colors.Crimson});

            elementHost.Child = scs;
        }
    }
}
