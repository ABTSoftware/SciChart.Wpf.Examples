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