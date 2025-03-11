// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ManipulateSeriesMvvmViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.ManipulateSeriesMVVM
{
    public class ManipulateSeriesMvvmViewModel : BaseViewModel
    {
        private SeriesType _selectedSeriesType;

        public ManipulateSeriesMvvmViewModel()
        {
            ViewportManager = new DefaultViewportManager();
            RenderableSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>();

            AddCommand = new ActionCommand(() =>
            {
                RenderableSeriesViewModels.Add(ViewModelsFactory.New(SelectedSeriesType.Type, 0));
                ZoomExtents();
                ClearCommand.RaiseCanExecuteChanged();
            }, () => SelectedSeriesType != null);

            RemoveCommand = new ActionCommand(() =>
            {
                RenderableSeriesViewModels.RemoveWhere(s => s.IsSelected);
                ClearCommand.RaiseCanExecuteChanged();
            }, () => RenderableSeriesViewModels.Any(s => s.IsSelected));

            ClearCommand = new ActionCommand(() =>
            {
                RenderableSeriesViewModels.Clear();
                ClearCommand.RaiseCanExecuteChanged();
            }, () => RenderableSeriesViewModels.Count > 0);

            SelectionChangedCommand = new ActionCommand(() =>
            {
                var rSeriesVm = RenderableSeriesViewModels.FirstOrDefault(s => s.IsSelected);
                if (rSeriesVm != null)
                {
                    SelectedSeriesType = SeriesTypes.FirstOrDefault(x => x.Type == rSeriesVm.GetType());
                }
                RemoveCommand.RaiseCanExecuteChanged();
            });

            var data = DataManager.Instance.GetSinewave(1.0, 0.5, 100, 5);
            var lineDataSeries = new XyDataSeries<double, double>();
            lineDataSeries.Append(data.XData.Select(d => d * 5d), data.YData);

            SeriesTypes = new List<SeriesType>
            {
                new SeriesType(typeof(ColumnRenderableSeriesViewModel), "IconColumn"),
                new SeriesType(typeof(ImpulseRenderableSeriesViewModel), "IconImpulse"),
                new SeriesType(typeof(LineRenderableSeriesViewModel), "IconLine"),
                new SeriesType(typeof(MountainRenderableSeriesViewModel), "IconMountain"),
                new SeriesType(typeof(XyScatterRenderableSeriesViewModel), "IconScatter"),
            };

            SelectedSeriesType = SeriesTypes[3];

            AddCommand.Execute(null);
            AddCommand.RaiseCanExecuteChanged();
            RemoveCommand.RaiseCanExecuteChanged();
        }

        public IViewportManager ViewportManager { get; }
        public List<SeriesType> SeriesTypes { get; }
        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; }

        public ActionCommand AddCommand { get; }
        public ActionCommand RemoveCommand { get; }
        public ActionCommand ClearCommand { get; }
        public ActionCommand SelectionChangedCommand { get; }

        public SeriesType SelectedSeriesType
        {
            get => _selectedSeriesType;
            set
            {
                if (_selectedSeriesType != value)
                {
                    _selectedSeriesType = value;
                    ChangeRenderableSeriesType();
                    OnPropertyChanged("SelectedSeriesType");
                }
            }
        }

        private void ChangeRenderableSeriesType()
        {
            var rSeriesVm = RenderableSeriesViewModels.FirstOrDefault(s => s.IsSelected);
            if (rSeriesVm != null)
            {
                var index = RenderableSeriesViewModels.IndexOf(rSeriesVm);

                RenderableSeriesViewModels[index] = ViewModelsFactory.New(SelectedSeriesType.Type, 0, RenderableSeriesViewModels[index].DataSeries);
                RenderableSeriesViewModels[index].IsSelected = true;

                ZoomExtents();
            }
        }

        private void ZoomExtents()
        {
            ViewportManager.BeginInvoke(() =>
            {
                ViewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
            });
        }
    }
}