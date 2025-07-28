// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ScatterChart3DFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.ThreeDCharts.ChartFactory
{
    public class ScatterChart3DFactory : IChart3DFactory
    {
        public string Title => "Scatter3D";

        public IAxis3DViewModel GetXAxis()
        {
            return new NumericAxis3DViewModel
            {
                StyleKey = "Scatter3DAxisStyle"
            };
        }

        public IAxis3DViewModel GetYAxis()
        {
            return new NumericAxis3DViewModel
            {
                StyleKey = "Scatter3DAxisStyle"
            };
        }

        public IAxis3DViewModel GetZAxis()
        {
            return new NumericAxis3DViewModel
            {
                StyleKey = "Scatter3DAxisStyle"
            };
        }

        public async Task<IEnumerable<IRenderableSeries3DViewModel>> GetSeriesAsync()
        {
            var renderSeries = new List<IRenderableSeries3DViewModel>(4);

            var xyzDataSeries1 = new XyzDataSeries3D<double>();
            var xyzDataSeries2 = new XyzDataSeries3D<double>();
            var xyzDataSeries3 = new XyzDataSeries3D<double>();

            await Task.Run(() =>
            {
                var colors = new[]
                {
                    Color.FromRgb(174, 064, 142),
                    Color.FromRgb(174, 064, 142),
                    Color.FromRgb(174, 064, 142),
                    Color.FromRgb(103, 190, 175),
                    Color.FromRgb(065, 112, 150),
                    Color.FromRgb(065, 112, 150)
                };

                Color getColor(double coord)
                {
                    var index = (int)Math.Ceiling(coord / 50) - 1;
                    return index < colors.Length ? colors[index] : colors[colors.Length - 1];
                }

                var data = DataManager.Instance.LoadOilGasScatterXyzData();

                foreach (var item in data)
                {
                    xyzDataSeries1.Append(100 * item.Scale, item.Y1, item.Z1, new PointMetadata3D(getColor(item.Y1), (float)item.Scale));
                    xyzDataSeries2.Append(item.X1, 100 * item.Scale, item.Z1, new PointMetadata3D(getColor(item.Z1), (float)item.Scale));
                    xyzDataSeries3.Append(item.X1, item.Y1, 100 * item.Scale, new PointMetadata3D(getColor(item.Y1), (float)item.Scale));
                }
            });

            renderSeries.Add(new ScatterRenderableSeries3DViewModel
            {
                DataSeries = xyzDataSeries1,
                StyleKey = "ScatterMetadataColorSeriesStyle"
            });

            renderSeries.Add(new ScatterRenderableSeries3DViewModel
            {
                DataSeries = xyzDataSeries2,
                StyleKey = "ScatterMetadataColorSeriesStyle"
            });

            renderSeries.Add(new ScatterRenderableSeries3DViewModel
            {
                DataSeries = xyzDataSeries3,
                StyleKey = "ScatterMetadataColorSeriesStyle"
            });

            return renderSeries;
        }
    }
}