// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ExampleHelpers.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Input;
using SciChart.Core.Utility;

namespace SciChart.Examples.ExternalDependencies.Common
{
    /// <summary>
    /// A helper class which is used to invoke commands on the Loaded and Unloaded events of a FrameworkElement
    /// </summary>
    public static class ExampleHelpers
    {
        private static bool IsLoaded(FrameworkElement fe)
        {
#if SILVERLIGHT
            var parent = System.Windows.Media.VisualTreeHelper.GetParent(fe);
            return parent != null;
#else
            return fe.IsLoaded;
#endif
        }

        public static readonly DependencyProperty LoadedEventCommandProperty = DependencyProperty.RegisterAttached(
            "LoadedEventCommand", typeof(ICommand), typeof(ExampleHelpers), new PropertyMetadata(default(ICommand), OnLoadedEventCommandChanged));

        public static void SetLoadedEventCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(LoadedEventCommandProperty, value);
        }

        public static ICommand GetLoadedEventCommand(DependencyObject element)
        {
            return (ICommand) element.GetValue(LoadedEventCommandProperty);
        }

        public static readonly DependencyProperty UnloadedEventCommandProperty = DependencyProperty.RegisterAttached(
            "UnloadedEventCommand", typeof (ICommand), typeof (ExampleHelpers), new PropertyMetadata(default(ICommand), OnUnloadedEventCommandChanged));

        public static void SetUnloadedEventCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(UnloadedEventCommandProperty, value);
        }

        public static ICommand GetUnloadedEventCommand(DependencyObject element)
        { 
            return (ICommand) element.GetValue(UnloadedEventCommandProperty);
        }

        private static void OnLoadedEventCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe != null)
            {
                fe.Loaded += DoLoaded;
                if (IsLoaded(fe))
                {
                    DoLoaded(fe, EventArgs.Empty);
                }
            }
        }

        private static void DoLoaded(object sender, EventArgs e)
        {
            TimedMethod.Invoke(() =>
            {
                var command = GetLoadedEventCommand((DependencyObject)sender);
                if (command != null)
                {
                    command.Execute(null);
                }
            }).After(200).Go();
        }

        private static void OnUnloadedEventCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe != null)
            {
                fe.Unloaded += (s, arg) =>
                {
                    var command = GetUnloadedEventCommand(fe);
                    if (command != null)
                    {
                        command.Execute(null);
                    }
                };
            }
        }        
    }
}
