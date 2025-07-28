using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Controls
{
    public class ItemsControlWithUIAutomation : ItemsControl
    {
        public static readonly DependencyProperty NavigatedItemProperty = DependencyProperty.Register
            (nameof(NavigatedItem), typeof(TileViewModel), typeof(ItemsControlWithUIAutomation),
            new PropertyMetadata(null, OnNavigatedItemChanged));

        private ScrollViewer _scrollViewer;

        private static bool _isInternalNavigation = false;

        public ItemsControlWithUIAutomation()
        {
            Loaded += (s, e) =>
            {
                _scrollViewer = FindParentScrollViewer(this);

                if (_scrollViewer != null)
                {
                    _scrollViewer.ScrollChanged += (s, e) => UpdateScrollItem();

                    UpdateScrollItem();
                }
            };
        }

        public TileViewModel NavigatedItem
        {
            get => (TileViewModel)GetValue(NavigatedItemProperty);
            set => SetValue(NavigatedItemProperty, value);
        }

        private void SetNavigatedItemInternal(TileViewModel item)
        {
            try
            {
                _isInternalNavigation = true;

                SetValue(NavigatedItemProperty, item);
            }
            finally
            {
                _isInternalNavigation = false;
            }
        }

        public static ScrollViewer FindParentScrollViewer(DependencyObject child)
        {
            DependencyObject current = child;

            while (current != null)
            {
                if (current is ScrollViewer sv)
                {
                    return sv;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new GenericAutomationPeer(this);
        }

        private void UpdateScrollItem()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is FrameworkElement container)
                {
                    var point = container.TranslatePoint(new Point(0, 0), _scrollViewer);

                    if ((point.Y >= 0 && point.Y <= 200) || point.Y > 200)
                    {
                        if (ItemContainerGenerator.ItemFromContainer(container) is TileViewModel item)
                        {
                            SetNavigatedItemInternal(item);
                            break;
                        }
                    }
                }
            }
        }

        private void ScrollToContainer(FrameworkElement container)
        {
            if (_scrollViewer != null)
            {
                var point = container.TranslatePoint(new Point(0, 0), this);

                _scrollViewer.ScrollToVerticalOffset(Math.Max(0, point.Y));
            }
        }

        private static void OnNavigatedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!_isInternalNavigation && d is ItemsControlWithUIAutomation itemsControl)
            {
                if (itemsControl.ItemContainerGenerator.ContainerFromItem(e.NewValue) is FrameworkElement container)
                {
                    itemsControl.ScrollToContainer(container);
                }
            }
        }
    }
}