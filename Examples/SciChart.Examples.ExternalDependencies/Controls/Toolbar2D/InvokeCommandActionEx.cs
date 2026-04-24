// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// InvokeCommandActionEx.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D
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
                var seriesName = (eventArgs.AddedItems.Count > 0 ? eventArgs.AddedItems[0] : null) as string;

                if (seriesName != null && SnapToSelectedSeriesCommand != null)
                {
                    SnapToSelectedSeriesCommand.Execute(seriesName);
                }
            }
        }
    }
}
