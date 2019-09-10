// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
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
    public class ManipulateSeriesMvvmViewModel :BaseViewModel
    {
        private readonly IViewportManager _viewportManager = new DefaultViewportManager();

        private SeriesType _selectedSeriesType;
        private List<SeriesType> _seriesTypes;
        private int _valueShift;

        private readonly ObservableCollection<IRenderableSeriesViewModel> _renderableSeriesViewModels;

        private readonly ActionCommand _addCommand;
        private readonly ActionCommand _removeCommand;
        private readonly ActionCommand _clearCommand;
        private readonly ActionCommand _selectionChangedCommand;

        public ManipulateSeriesMvvmViewModel()
        {
            _renderableSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>();

            _addCommand = new ActionCommand(() =>
            {
                RenderableSeriesViewModels.Add(ViewModelsFactory.New(SelectedSeriesType.Type, 0));
                ZoomExtents();
                ClearCommand.RaiseCanExecuteChanged();
            }, () => SelectedSeriesType != null);

            _removeCommand = new ActionCommand(() =>
            {
                RenderableSeriesViewModels.RemoveWhere(s => s.IsSelected);
                ClearCommand.RaiseCanExecuteChanged();
            }, () => RenderableSeriesViewModels.Any(s => s.IsSelected));

            _clearCommand = new ActionCommand(() =>
            {
                RenderableSeriesViewModels.Clear();
                ClearCommand.RaiseCanExecuteChanged();
            }, () => RenderableSeriesViewModels.Count > 0);

            _selectionChangedCommand = new ActionCommand(() =>
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
            lineDataSeries.Append(data.XData.Select(d => d*5), data.YData);

            FillSeriesTypes();
            SelectedSeriesType = SeriesTypes[3];

            _addCommand.Execute(null);
            
            AddCommand.RaiseCanExecuteChanged();
            RemoveCommand.RaiseCanExecuteChanged();
        }

        public IViewportManager ViewportManager
        {
            get { return _viewportManager; }
        }

        public List<SeriesType> SeriesTypes
        {
            get { return _seriesTypes; }
        }

        public SeriesType SelectedSeriesType
        {
            get { return _selectedSeriesType; }
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

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeriesViewModels
        {
            get { return _renderableSeriesViewModels; }
        }

        public ActionCommand AddCommand
        {
            get { return _addCommand; }
        }

        public ActionCommand RemoveCommand
        {
            get { return _removeCommand; }
        }

        public ActionCommand ClearCommand
        {
            get { return _clearCommand; }
        }

        public ActionCommand SelectionChangedCommand
        {
            get { return _selectionChangedCommand; }
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
            _viewportManager.BeginInvoke(() =>
            {
                ViewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500));
            });
        }

        private void FillSeriesTypes()
        {
            _seriesTypes = new List<SeriesType>
            {
                new SeriesType(typeof (ColumnRenderableSeriesViewModel), "IconColumn"),
                new SeriesType(typeof (ImpulseRenderableSeriesViewModel), "IconImpulse"),
                new SeriesType(typeof (LineRenderableSeriesViewModel), "IconLine"),
                new SeriesType(typeof (MountainRenderableSeriesViewModel), "IconMountain"),
                new SeriesType(typeof (XyScatterRenderableSeriesViewModel), "IconScatter"),
            };
        }
    }
}