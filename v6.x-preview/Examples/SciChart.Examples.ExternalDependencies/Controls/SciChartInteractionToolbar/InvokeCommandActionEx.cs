// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// InvokeCommandActionEx.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar
{
    public class InvokeSnapToSeriesCommandAction : TriggerAction<Control>
    {
        /// <summary>
        /// Defines the SnapToSelectedSeriesCommand property
        /// </summary>
        public static readonly DependencyProperty SnapToSelectedSeriesCommandProperty =
            DependencyProperty.Register("SnapToSelectedSeriesCommand", typeof(ICommand), typeof(InvokeSnapToSeriesCommandAction), new PropertyMetadata(null));

        public ICommand SnapToSelectedSeriesCommand
        {
            get { return (ICommand)GetValue(SnapToSelectedSeriesCommandProperty); }
            set { SetValue(SnapToSelectedSeriesCommandProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            var eventArgs = parameter as SelectionChangedEventArgs;

            if (eventArgs != null)
            {
                var selectedSeries = (eventArgs.AddedItems.Count > 0 ? eventArgs.AddedItems[0] : null) as BaseRenderableSeries;

                if (selectedSeries != null && SnapToSelectedSeriesCommand != null)
                {
                    SnapToSelectedSeriesCommand.Execute(selectedSeries);
                }
            }
        }
    }
}
