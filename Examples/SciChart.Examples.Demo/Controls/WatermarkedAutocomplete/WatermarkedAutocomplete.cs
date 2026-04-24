// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// WatermarkedAutocomplete.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SciChart.Examples.Demo.Common;

namespace SciChart.Examples.Demo.Controls.WatermarkedAutocomplete
{
    [TemplatePart(Name = "PART_SearchIcon", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_Watermark", Type = typeof(TextBlock))]
    [TemplatePart(Name = "Text", Type = typeof(TextBox))]
    [TemplatePart(Name = "Popup", Type = typeof(Popup))]
    public class WatermarkedAutocomplete : AutoCompleteBoxCompatible
    {
        private Popup _contentListPopup;

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark",
            typeof(string), typeof(WatermarkedAutocomplete), new PropertyMetadata("Watermark"));

        public string Watermark
        {
            get => (string) GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public WatermarkedAutocomplete()
        {
            DefaultStyleKey = typeof(WatermarkedAutocomplete);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentListPopup = GetTemplateChild("Popup") as Popup;

            if (_contentListPopup != null && Window.GetWindow(this) is Window window)
            {
                window.LocationChanged += WindowOnLocationChanged;
            }
        }

        private void WindowOnLocationChanged(object sender, EventArgs e)
        {
            if (_contentListPopup?.IsOpen == true)
            {
                _contentListPopup.IsOpen = false;
            }
        }
    }
}