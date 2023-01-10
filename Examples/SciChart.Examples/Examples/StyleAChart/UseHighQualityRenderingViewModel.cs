// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UseHighQualityRenderingViewModel.cs is part of the SCICHART® Examples. Permission is
// hereby granted to modify, create derivative works, distribute and publish any part of
// this source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.StyleAChart
{
    public class UseHighQualityRenderingViewModel : BaseViewModel
    {
        private IRange _sharedXVisibleRange;
        private IRange _sharedYVisibleRange;

        public IRange SharedXVisibleRange
        {
            get => _sharedXVisibleRange;
            set
            {
                _sharedXVisibleRange = value;
                OnPropertyChanged(nameof(SharedXVisibleRange));
            }
        }

        public IRange SharedYVisibleRange
        {
            get => _sharedYVisibleRange;
            set
            {
                _sharedYVisibleRange = value;
                OnPropertyChanged(nameof(SharedYVisibleRange));
            }
        }

        public IXyDataSeries<double, double> DataSeries { get; }

        public IViewportManager ViewportManager { get; }

        public ICommand ZoomExtentsCommand { get;}

        public UseHighQualityRenderingViewModel()
        {
            DataSeries = CreateDataSeries();

            ViewportManager = new DefaultViewportManager();

            ZoomExtentsCommand = new ActionCommand(() => ViewportManager.AnimateZoomExtents(TimeSpan.FromMilliseconds(500)));
        }

        private IXyDataSeries<double, double> CreateDataSeries()
        {
            const int count = 1000;

            var dataSeries = new XyDataSeries<double>();

            for (int i = 0; i < count; i++)
            {
                dataSeries.Append(i, count * Math.Sin(i * Math.PI * 0.1) / i);
            }
            return dataSeries;
        }
    }
}