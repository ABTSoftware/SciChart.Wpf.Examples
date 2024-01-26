// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ExampleHelpers.cs is part of SCICHART®, High Performance Scientific Charts
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
