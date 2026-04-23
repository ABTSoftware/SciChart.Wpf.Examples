// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomListBoxItem.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Controls
{
    [TemplateVisualState(GroupName = "GroupStates", Name = "GroupState")]
    [TemplateVisualState(GroupName = "GroupStates", Name = "ExampleState")]
    public class CustomListBoxItem : ListBoxItem
    {
        public static readonly DependencyProperty IsGroupProperty = DependencyProperty.Register("IsGroup", typeof (bool), typeof (CustomListBoxItem), new PropertyMetadata(false, IsGroupPropertyChanged));

        public bool IsGroup
        {
            get { return (bool) GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var isGroup = (bool)this.GetValue(IsGroupProperty);
            UpdateGroupState(isGroup);
        }

        private void UpdateGroupState(bool isGroup)
        {
            VisualStateManager.GoToState(this, isGroup ? "GroupState" : "ExampleState", true);
        }

        private static void IsGroupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var customListBoxItem = d as CustomListBoxItem;
            if (customListBoxItem != null)
            {
                customListBoxItem.UpdateGroupState((bool) args.NewValue);
            }
        }
    }
}