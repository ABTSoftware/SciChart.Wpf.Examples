// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ToolbarIcon.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using MahApps.Metro.IconPacks;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class ToolbarIcon : PackIconMaterial
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register
            (nameof(Path), typeof(string), typeof(ToolbarIcon), new PropertyMetadata(null, OnPathPropertyChanged));

        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        protected override void UpdateData()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                Data = Path;
            }
            else
            {
                base.UpdateData();
            }
        }

        private static void OnPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ToolbarIcon toolbarIcon)
            {
                toolbarIcon.UpdateData();
            }
        }
    }
}