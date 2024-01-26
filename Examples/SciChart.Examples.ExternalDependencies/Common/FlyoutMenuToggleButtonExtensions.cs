// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FlyoutMenuToggleButtonExtensions.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class FlyoutMenuToggleButtonExtensions : DependencyObject
    {
        private static readonly Dictionary<string, List<ToggleButton>> _elementToGroupNames = new Dictionary<string, List<ToggleButton>>();

        /// <summary>
        /// Defines the GroupName DependenccyProperty
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached
            ("GroupName", typeof(string), typeof(FlyoutMenuToggleButtonExtensions), new PropertyMetadata(OnGroupNameChanged));

        /// <summary>
        /// Sets the GroupName Attached Property
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetGroupName(ToggleButton element, string value)
        {
            element.SetValue(GroupNameProperty, value);
        }

        /// <summary>
        /// Gets the GroupName Attached Property
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetGroupName(ToggleButton element)
        {
            return element.GetValue(GroupNameProperty).ToString();
        }

        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ToggleButton toggleButton)
            {
                var newGroupName = e.NewValue?.ToString();
                var oldGroupName = e.OldValue?.ToString();

                if (!string.IsNullOrEmpty(oldGroupName))
                {
                    // Remove the toggle button from the group
                    RemoveFromGroup(oldGroupName, toggleButton);
                }

                if (!string.IsNullOrEmpty(newGroupName))
                {
                    // Add the toggle button to the group
                    AddToGroup(newGroupName, toggleButton);
                }
            }
        }

        private static void RemoveFromGroup(string groupName, ToggleButton toggleButton)
        {
            if (_elementToGroupNames.TryGetValue(groupName, out var groupButtons))
            {
                groupButtons.Remove(toggleButton);

                if (groupButtons.Count == 0)
                {
                    _elementToGroupNames.Remove(groupName);
                }
            }

            toggleButton.Click -= ToggleButtonClicked;
            toggleButton.Unloaded -= ToggleButtonUnloaded;
        }

        private static void AddToGroup(string groupName, ToggleButton toggleButton)
        {
            if (!_elementToGroupNames.ContainsKey(groupName))
            {
                _elementToGroupNames.Add(groupName, new List<ToggleButton>());
            }

            _elementToGroupNames[groupName].Add(toggleButton);

            toggleButton.Click += ToggleButtonClicked;
            toggleButton.Unloaded += ToggleButtonUnloaded;

            if (_elementToGroupNames[groupName].Count(x => x.IsChecked == true) > 1)
            {
                UpdateGroupState(toggleButton);
            }
        }

        private static void ToggleButtonUnloaded(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;

            RemoveFromGroup(GetGroupName(toggleButton), toggleButton);
        }

        private static void ToggleButtonClicked(object sender, RoutedEventArgs e)
        {
            if (e.Source is ToggleButton toggleButton)
            {
                var point = Mouse.GetPosition(toggleButton);

                if (point.X >= 0 && point.X <= toggleButton.ActualWidth &&
                    point.Y >= 0 && point.Y <= toggleButton.ActualHeight)
                {
                    UpdateGroupState(toggleButton);
                }
            }
        }

        private static void UpdateGroupState(ToggleButton toggleButton)
        {
            var allToggleButtons = _elementToGroupNames[GetGroupName(toggleButton)];

            foreach (var item in allToggleButtons.Where(x => !x.Equals(toggleButton)))
            {
                item.IsChecked = false;
            }
        }
    }
}
