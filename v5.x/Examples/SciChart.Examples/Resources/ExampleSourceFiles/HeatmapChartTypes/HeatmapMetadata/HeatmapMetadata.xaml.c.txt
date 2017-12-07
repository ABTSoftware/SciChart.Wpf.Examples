using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.ChartData;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Helpers;
using SciChart.Drawing.Common;

namespace SciChart.Examples.Examples.HeatmapChartTypes.HeatmapMetadata
{
    /// <summary>
    /// Interaction logic for HeatmapMetadata.xaml
    /// </summary>
    public partial class HeatmapMetadata : UserControl
    {
        private const int _width = 31;
        private const int _height = 22;

        public HeatmapMetadata()
        {
            InitializeComponent();

            heatmapSeries.DataSeries = CreateSeries(); ;
        }

        private IDataSeries CreateSeries()
        {
            var data = new double[_height, _width];
            IPointMetadata[,] metaDatas = new IPointMetadata[_height, _width];

            var zValues = FillZValues();

            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                {
                    var dataValue = zValues[y * _width + x];

                    //MetaData 
                    var metadata = new HeatmapMetaData
                    {
                        IsSelected = false,
                        IsBody = dataValue > 0,
                        CellColor = (dataValue == 0) ? Colors.Transparent : Colors.DeepPink,
                        Tooltip = dataValue == 0 ? "Not Elephant" : "Elephant",
                    };

                    data[y, x] = dataValue;
                    metaDatas[y, x] = metadata;
                }
            return new UniformHeatmapDataSeries<int, int, double>(data, 0, 1, 0, 1, metaDatas);
        }

        private double[] FillZValues()
        {
            var rnd = new Random();
            var lenght = _height * _width;
            var zValues = new double[lenght];

            // block1
            zValues[GetValueableIndex(0, 13)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(1, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(1, 14)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(2, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(2, 14)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(3, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(3, 14)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(4, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(4, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(4, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(4, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(4, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(4, 14)] = rnd.Next(20, 100);

            // block2
            zValues[GetValueableIndex(5, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(5, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(5, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(5, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(5, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(5, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(5, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(5, 15)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(6, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(6, 16)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(7, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(7, 17)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(8, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(8, 18)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(9, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 18)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(9, 19)] = rnd.Next(20, 100);

            // block3
            zValues[GetValueableIndex(10, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(10, 14)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(11, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(11, 14)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(12, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(12, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(12, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(12, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(12, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(12, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(12, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(12, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(12, 13)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(13, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(13, 14)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(14, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 20)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(14, 21)] = rnd.Next(20, 100);

            // block4
            zValues[GetValueableIndex(15, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 18)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 19)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 20)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(15, 21)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(16, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 18)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 19)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 20)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(16, 21)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(17, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 18)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 19)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 20)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(17, 21)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(18, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(18, 17)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(19, 0)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(19, 16)] = rnd.Next(20, 100);

            // block5
            zValues[GetValueableIndex(20, 0)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(20, 16)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(21, 0)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 18)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 19)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 20)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(21, 21)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(22, 0)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 18)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 19)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 20)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(22, 21)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(23, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 18)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 19)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 20)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(23, 21)] = rnd.Next(20, 100);
            
            zValues[GetValueableIndex(24, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 16)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 17)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 18)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 19)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 20)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(24, 21)] = rnd.Next(20, 100);

            // block5
            zValues[GetValueableIndex(25, 1)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(25, 15)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(26, 2)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(26, 14)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(27, 3)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 4)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 5)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(27, 13)] = rnd.Next(20, 100);
            
            zValues[GetValueableIndex(28, 6)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(28, 7)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(28, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(28, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(28, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(28, 11)] = rnd.Next(20, 100);
            
            zValues[GetValueableIndex(29, 8)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(29, 9)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(29, 10)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(29, 11)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(29, 12)] = rnd.Next(20, 100);

            zValues[GetValueableIndex(30, 12)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(30, 13)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(30, 14)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(30, 15)] = rnd.Next(20, 100);
            zValues[GetValueableIndex(30, 16)] = rnd.Next(20, 100);

            return zValues;
        }

        private int GetValueableIndex(int x, int y)
        {
            return (y *_width) + x;
        }

        private void EnableMetaDatas(object sender, RoutedEventArgs e)
        {
            heatmapSeries.PaletteProvider = new HeatmapMetaDataPaletteProvider();

            var template = Resources["TooltipItemTemplate"] as DataTemplate;
            TooltipModifier.SetTooltipTemplate(heatmapSeries, template);
        }

        private void DisableMetaDatas(object sender, RoutedEventArgs e)
        {
            heatmapSeries.PaletteProvider = null;
            TooltipModifier.SetTooltipTemplate(heatmapSeries, null);
        }
    }

    public class CustomUniformHeatmapRenderableSeries : FastUniformHeatmapRenderableSeries
    {
        protected override string FormatDataValue(double dataValue, int xIndex, int yIndex)
        {
            IPointMetadata[,] metaDatas = ((IHeatmapDataSeries)DataSeries).Metadata;
            if (metaDatas != null)
            {
                var metaData = (HeatmapMetaData)metaDatas[yIndex, xIndex];
                return metaData.IsBody ? base.FormatDataValue(dataValue, xIndex, yIndex) : string.Empty;
            }
            return base.FormatDataValue(dataValue, xIndex, yIndex); ;
        }
    }

    public class HeatmapMetaData : IPointMetadata
    {
        // Implementing IPointMetadata
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsSelected { get; set; }

        // Specific properties
        public Color CellColor { get; set; }
        public bool IsBody { get; set; }
        public string Tooltip { get; set; }
    }

    public class HeatmapMetaDataPaletteProvider : IHeatmapPaletteProvider
    {
        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }

        public Color? OverrideCellColor(IRenderableSeries rSeries, int xIndex, int yIndex, IComparable zValue, Color cellColor, IPointMetadata metadata)
        {
           if (metadata != null)
            {
                cellColor = ((HeatmapMetaData)metadata).CellColor;
            }

            return cellColor;
        }
    }
}
