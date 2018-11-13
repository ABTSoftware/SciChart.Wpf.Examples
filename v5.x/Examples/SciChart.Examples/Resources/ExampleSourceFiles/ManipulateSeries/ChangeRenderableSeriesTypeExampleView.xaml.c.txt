// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ChangeRenderableSeriesTypeExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Linq;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ManipulateSeries
{
    /// <summary>
    /// Interaction logic for ChangeRenderableSeriesType.xaml
    /// </summary>
    public partial class ChangeRenderableSeriesType : UserControl
    {
        public ChangeRenderableSeriesType()
        {
            InitializeComponent();

            Loaded += ChangeRenderableSeriesTypeLoaded;

            seriesTypesCombo.Items.Add(typeof (FastLineRenderableSeries));
            seriesTypesCombo.Items.Add(typeof (FastColumnRenderableSeries));
            seriesTypesCombo.Items.Add(typeof (FastMountainRenderableSeries));
            seriesTypesCombo.Items.Add(typeof (FastImpulseRenderableSeries));
        }

        private void SeriesComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // When the selected series type combo changes, update the selected series type
            if (seriesTypesCombo.SelectedValue == null ||
                !sciChart.SelectedRenderableSeries.Any()) return;

            // Get the selected series
            var oldSelectedSeries = sciChart.SelectedRenderableSeries.First();
            oldSelectedSeries.IsSelected = false;

            // Replace it with a new series of the requested type
            int index = sciChart.RenderableSeries.IndexOf(oldSelectedSeries);

            var oldRenderableSeries = sciChart.RenderableSeries[index];
            var newSelectedSeries = (BaseRenderableSeries)Activator.CreateInstance((Type) seriesTypesCombo.SelectedValue);
            newSelectedSeries.Stroke = oldSelectedSeries.Stroke;
            sciChart.RenderableSeries[index] = newSelectedSeries;
            newSelectedSeries.DataSeries = oldRenderableSeries.DataSeries;
        }

        private void SeriesSelectionModifierSelectionChanged(object sender, EventArgs e)
        {
            seriesTypesCombo.SelectionChanged -= SeriesComboSelectionChanged;

            OnSeriesSelectionChanged();

            seriesTypesCombo.SelectionChanged += SeriesComboSelectionChanged;
        }

        private void OnSeriesSelectionChanged()
        {
            // When the user selects a new series, set the series type on the combo box
            var selectedSeries = sciChart.SelectedRenderableSeries;
            bool hasSelection = selectedSeries != null && selectedSeries.Count > 0;

            seriesTypesCombo.IsEnabled = hasSelection;
            if (hasSelection)
            {
                seriesTypesCombo.SelectedValue = selectedSeries[0].GetType();
            }
        }

        private void ChangeRenderableSeriesTypeLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // OnLoad, create three data and renderable series
            FillData(new XyDataSeries<double, double>());
            FillData(new XyDataSeries<double, double>());
            FillData(new XyDataSeries<double, double>());

            OnSeriesSelectionChanged();

            sciChart.ZoomExtents();
        }

        private void FillData(IXyDataSeries<double, double> dataSeries)
        {
            var data = DataManager.Instance.GetRandomDoubleSeries(50);
            dataSeries.Append(data.XData, data.YData);

            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()
                {
                    Stroke = DataManager.Instance.GetRandomColor(),
                    DataSeries = dataSeries,
                });
        }
    }
}
