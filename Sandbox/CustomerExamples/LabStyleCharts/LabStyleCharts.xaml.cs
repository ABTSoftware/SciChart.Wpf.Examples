using System.Windows;
using SciChart.Charting3D.Model;

namespace LabStyleChartsExample
{
    /// <summary>
    /// Mimics 3D Chart styling from a well known *Lab software 
    /// </summary>    
    public partial class LabStyleCharts : Window
    {
        public LabStyleCharts()
        {
            InitializeComponent();

            var ds = new UniformGridDataSeries3D<double>(11, 4);
            ds.StartX = 0;
            ds.StepX = 0.09;
            ds.StartZ = 0;
            ds.StepZ = 0.75;

            // Initialize some nice looking data
            ds[0, 0] = -1.43; ds[0, 1] = -2.95; ds[0, 2] = -2.97; ds[0, 3] = -1.81; ds[0, 4] = -1.33; ds[0, 5] = -1.53; ds[0, 6] = -2.04; ds[0, 7] = 2.08; ds[0, 8] = 1.94; ds[0, 9] = 1.42; ds[0, 10] = 1.58; ds[1, 0] = 1.77;
            ds[1, 1] = 1.76; ds[1, 2] = -1.1; ds[1, 3] = -0.26; ds[1, 4] = 0.72; ds[1, 5] = 0.64; ds[1, 6] = 3.26; ds[1, 7] = 3.2; ds[1, 8] = 3.1; ds[1, 9] = 1.94; ds[1, 10] = 1.54;
            ds[2, 0] = 0; ds[2, 1] = 0; ds[2, 2] = 0; ds[2, 3] = 0; ds[2, 4] = 0; ds[2, 5] = 3.7; ds[2, 6] = 3.7; ds[2, 7] = 3.7; ds[2, 8] = 3.7; ds[2, 9] = -0.48; ds[2, 10] = -0.48;
            ds[3, 0] = 0; ds[3, 1] = 0; ds[3, 2] = 0; ds[3, 3] = 0; ds[3, 4] = 0; ds[3, 5] = 0; ds[3, 6] = 0; ds[3, 7] = 0; ds[3, 8] = 0; ds[3, 9] = 0; ds[3, 10] = 0;
            
            scs.RenderableSeries[0].DataSeries = ds;
        }
    }
}
