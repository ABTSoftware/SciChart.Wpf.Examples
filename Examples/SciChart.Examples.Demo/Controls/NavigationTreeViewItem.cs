using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Controls
{
    public class NavigationTreeViewItem : TreeViewItem
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var treeViewItemHeader = GetTemplateChild("PART_ItemHeader") as FrameworkElement;

            treeViewItemHeader.MouseLeftButtonDown += (s, e) => IsExpanded = !IsExpanded;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NavigationTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is NavigationTreeViewItem;
        }
    }
}