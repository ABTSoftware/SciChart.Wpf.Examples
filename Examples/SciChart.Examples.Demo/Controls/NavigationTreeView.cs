// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// NavigationTreeView.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Controls
{
    public class NavigationTreeView : TreeView
    {
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register
            (nameof(SelectedNode), typeof(ExampleTreeNodeViewModel), typeof(NavigationTreeView),
            new PropertyMetadata(null, OnSelectedNodeChanged));

        public ExampleTreeNodeViewModel SelectedNode
        {
            get => (ExampleTreeNodeViewModel)GetValue(SelectedNodeProperty);
            set => SetValue(SelectedNodeProperty, value);
        }

        public NavigationTreeView()
        {
            // Ensures that SelectContainer is called when all containers are created
            Loaded += (s, e) => SelectContainer(SelectedNode);

            // Prevents node expander from toggling during quick mouse clicks
            PreviewMouseDoubleClick += (s, e) => e.Handled = true;
            
            // Prevents switching nodes using arrow keys
            PreviewKeyDown += (s, e) => e.Handled = true;
        }

        private static IEnumerable<NavigationTreeViewItem> GetAllContainersRecursive(ItemsControl parent)
        {
            for (int i = 0; i < parent.Items.Count; i++)
            {
                if (parent.ItemContainerGenerator.ContainerFromIndex(i) is NavigationTreeViewItem itemContainer)
                {
                    yield return itemContainer;

                    var childContainers = GetAllContainersRecursive(itemContainer);

                    foreach (NavigationTreeViewItem childContainer in childContainers)
                    {
                        yield return childContainer;
                    }
                }
            }
        }

        private void SelectContainer(ExampleTreeNodeViewModel item)
        {
            if (item != null)
            {
                foreach (var container in GetAllContainersRecursive(this))
                {
                    if (ReferenceEquals(container.DataContext, item))
                    {
                        container.SetValue(TreeViewItem.IsSelectedProperty, true);
                        container.BringIntoView();
                        break;
                    }
                }
            }
            else
            {
                foreach (var container in GetAllContainersRecursive(this))
                {
                    container.SetValue(TreeViewItem.IsSelectedProperty, false);
                }
            }
        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);

            SetValue(SelectedNodeProperty, e.NewValue);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NavigationTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is NavigationTreeViewItem;
        }

        private static void OnSelectedNodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationTreeView treeView && e.NewValue is ExampleTreeNodeViewModel item)
            {
                treeView.SelectContainer(item);
            }
        }
    }
}