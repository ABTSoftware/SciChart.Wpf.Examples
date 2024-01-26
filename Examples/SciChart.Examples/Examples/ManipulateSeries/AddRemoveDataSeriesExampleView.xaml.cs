// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AddRemoveDataSeriesExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ManipulateSeries
{
    /// <summary>
    /// Interaction logic for AddRemoveDataSeriesExampleView.xaml
    /// </summary>
    public partial class AddRemoveDataSeriesExampleView : UserControl
    {
        public AddRemoveDataSeriesExampleView()
        {
            InitializeComponent();
            Loaded += SciChartSurfaceLoaded;
        }

        private void SciChartSurfaceLoaded(object sender, RoutedEventArgs e)
        {
            OnSeriesSelectionChanged();
        }

        private void DeleteSeriesClick(object sender, RoutedEventArgs e)
        {
            var rSeries = sciChart.SelectedRenderableSeries.FirstOrDefault();
            if (rSeries?.DataSeries == null) return;

            sciChart.RenderableSeries.Remove(rSeries);
            sciChart.ZoomExtents();

            OnSeriesSelectionChanged();
        }

        private void OnSeriesSelectionChanged()
        {
            bool hasSelection = sciChart.SelectedRenderableSeries.Any();

            deleteSeriesButton.IsEnabled = hasSelection;
        }

        private void AddSeriesClick(object sender, RoutedEventArgs e)
        {
            // Create a DataSeries and append some data
            var dataSeries = new UniformXyDataSeries<double>();
            dataSeries.Append(DataManager.Instance.GetRandomDoubleData(250));

            // Create a RenderableSeries and ensure DataSeries is set
            var renderSeries = new FastLineRenderableSeries
            {
                Stroke = DataManager.Instance.GetRandomColor(),
                DataSeries = dataSeries,
                StrokeThickness = 2
            };

            // Add the new RenderableSeries
            sciChart.RenderableSeries.Add(renderSeries);
            sciChart.ZoomExtents();
        }

        private void SeriesSelectionModifier_SelectionChanged(object sender, EventArgs e)
        {
            OnSeriesSelectionChanged();
        }
    }
}