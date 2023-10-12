using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using SciChart.Examples.Demo.Common;

namespace SciChart.Examples.Demo.Controls
{
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_ItemsPresenter", Type = typeof(ItemsPresenter))]
    [TemplatePart(Name = "PART_LeftScrollButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_RightScrollButton", Type = typeof(Button))]
    public class ShowcaseControl : ListBox
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register
            (nameof(Content), typeof(object), typeof(ShowcaseControl), new PropertyMetadata(null));

        public static DependencyProperty ScrollButtonStyleProperty = DependencyProperty.RegisterAttached
            (nameof(ScrollButtonStyle), typeof(Style), typeof(ShowcaseControl), new PropertyMetadata(null));

        public static readonly DependencyProperty IsAutoSelectEnabledProperty = DependencyProperty.Register
            (nameof(IsAutoSelectEnabled), typeof(bool), typeof(ShowcaseControl), new PropertyMetadata(true, OnIsAutoSelectEnabledChanged));

        public static readonly DependencyProperty AutoSelectIntervalProperty = DependencyProperty.Register
            (nameof(AutoSelectInterval), typeof(TimeSpan), typeof(ShowcaseControl), new PropertyMetadata(TimeSpan.FromSeconds(5d), OnAutoSelectIntervalChanged));

        private static readonly DependencyPropertyKey AutoSelectСountdownPropertyKey = DependencyProperty.RegisterReadOnly
            (nameof(AutoSelectСountdown), typeof(TimeSpan), typeof(ShowcaseControl), new PropertyMetadata(TimeSpan.FromSeconds(5d)));

        public static readonly DependencyProperty AutoSelectСountdownProperty = AutoSelectСountdownPropertyKey.DependencyProperty;

        public static DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached
            ("HorizontalOffset", typeof(double), typeof(ShowcaseControl), new PropertyMetadata(0d, OnHorizontalOffsetChanged));

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public Style ScrollButtonStyle
        {
            get => (Style)GetValue(ScrollButtonStyleProperty);
            set => SetValue(ScrollButtonStyleProperty, value);
        }

        public bool IsAutoSelectEnabled
        {
            get => (bool)GetValue(IsAutoSelectEnabledProperty);
            set => SetValue(IsAutoSelectEnabledProperty, value);
        }

        public TimeSpan AutoSelectInterval
        {
            get => (TimeSpan)GetValue(AutoSelectIntervalProperty);
            set => SetValue(AutoSelectIntervalProperty, value);
        }

        public TimeSpan AutoSelectСountdown
        {
            get => (TimeSpan)GetValue(AutoSelectСountdownProperty);
            private set => SetValue(AutoSelectСountdownPropertyKey, value);
        }

        public static void SetHorizontalOffset(FrameworkElement target, double value)
        {
            target.SetValue(HorizontalOffsetProperty, value);
        }

        public static double GetHorizontalOffset(FrameworkElement target)
        {
            return (double)target.GetValue(HorizontalOffsetProperty);
        }

        private ScrollViewer _scrollViewer;
        private ItemsPresenter _itemsPresenter;

        private Button _leftScrollButton;
        private Button _rightScrollButton;

        private readonly TimeSpan TimerInterval = TimeSpan.FromSeconds(1);
        private readonly PausableDispatcherTimer _timer;

        public ShowcaseControl()
        {
            DefaultStyleKey = typeof(ShowcaseControl);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;

            PreviewMouseWheel += OnPreviewMouseWheel;

            _timer = new PausableDispatcherTimer(TimerInterval, OnTimerTick);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is ListBoxItem container)
            {
                container.RequestBringIntoView += Container_OnRequestBringIntoView;
                container.PreviewMouseLeftButtonDown += Container_OnPrevMouseLeftButtonDown;
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            if (element is ListBoxItem container)
            {
                container.RequestBringIntoView -= Container_OnRequestBringIntoView;
                container.PreviewMouseLeftButtonDown -= Container_OnPrevMouseLeftButtonDown;
            }
        }

        private void Container_OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = sender is ListBoxItem;
        }

        private void Container_OnPrevMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem container && container.IsSelected)
            {
                UpdateScrollPosition();
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            UpdateScrollPosition();

            AutoSelectСountdown = AutoSelectInterval;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _leftScrollButton = GetAndAssertTemplateChild<Button>("PART_LeftScrollButton");

            _leftScrollButton.Click -= OnLeftScrollButtonClick;
            _leftScrollButton.Click += OnLeftScrollButtonClick;

            _rightScrollButton = GetAndAssertTemplateChild<Button>("PART_RightScrollButton");

            _rightScrollButton.Click -= OnRightScrollButtonClick;
            _rightScrollButton.Click += OnRightScrollButtonClick;

            _itemsPresenter = GetAndAssertTemplateChild<ItemsPresenter>("PART_ItemsPresenter");

            _scrollViewer = GetAndAssertTemplateChild<ScrollViewer>("PART_ScrollViewer");
            _scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;

            ScrollViewer.SetCanContentScroll(this, false);
        }

        private T GetAndAssertTemplateChild<T>(string childName) where T : class
        {
            if (GetTemplateChild(childName) is T templateChild)
            {
                return templateChild;
            }
            throw new InvalidOperationException($"Unable to load '{childName}' template part");
        }

        private void OnTimerTick()
        {
            if (_timer.IsPaused || !IsAutoSelectEnabled) return;

            AutoSelectСountdown = AutoSelectСountdown.Subtract(TimerInterval);

            if (AutoSelectСountdown <= TimeSpan.Zero)
            {
                var nextItemIndex = SelectedIndex + 1;

                if (nextItemIndex < 0 || nextItemIndex >= Items.Count)
                {
                    nextItemIndex = 0;
                }

                SelectedItem = Items[nextItemIndex];
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateScrollPosition();

            if (IsAutoSelectEnabled)
            {
                AutoSelectСountdown = AutoSelectInterval;

                _timer.Start();

                if (IsMouseOver)
                {
                    _timer.Pause();
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (IsAutoSelectEnabled)
            {
                _timer.Stop();
            }
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (IsAutoSelectEnabled)
            {
                _timer.Pause();
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (IsAutoSelectEnabled)
            {
                _timer.Resume();
            }
        }

        private void OnLeftScrollButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollLeftOnViewport();
        }

        private void OnRightScrollButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollRightOnViewport();
        }

        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ViewportWidth >= e.ExtentWidth)
            {
                _leftScrollButton.IsEnabled = false;
                _rightScrollButton.IsEnabled = false;
            }
            else
            {
                _leftScrollButton.IsEnabled = e.HorizontalOffset > 0d;
                _rightScrollButton.IsEnabled = e.HorizontalOffset < _scrollViewer.ScrollableWidth;
            }
        }
        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            RaiseEvent(new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = e.Source
            });
        }

        public void ScrollLeftOnViewport()
        {
            if (_scrollViewer != null)
            {
                var offset = _scrollViewer.HorizontalOffset - _scrollViewer.ViewportWidth;
                AnimateScroll(_scrollViewer, Math.Max(offset, 0d));
            }
        }

        public void ScrollRightOnViewport()
        {
            if (_scrollViewer != null)
            {
                var offset = _scrollViewer.HorizontalOffset + _scrollViewer.ViewportWidth;
                AnimateScroll(_scrollViewer, Math.Min(offset, _scrollViewer.ScrollableWidth));
            }
        }

        private void AnimateScroll(ScrollViewer scrollViewer, double horizontalOffset)
        {
            var scrollAnimation = new DoubleAnimation
            {
                From = scrollViewer.HorizontalOffset,
                To = horizontalOffset,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };

            scrollViewer.BeginAnimation(HorizontalOffsetProperty, scrollAnimation);
        }

        private void UpdateScrollPosition()
        {
            if (_scrollViewer == null || _scrollViewer.ScrollableWidth <= 0d) return;

            var selectedItemLeft = 0d;
            var selectedItemWidth = 0d;

            for (int i = 0; i <= SelectedIndex; i++)
            {
                if (ItemContainerGenerator.ContainerFromItem(Items[i]) is ListBoxItem itemContainer)
                {
                    if (i < SelectedIndex)
                    {
                        selectedItemLeft += itemContainer.ActualWidth;
                    }
                    else
                    {
                        selectedItemLeft += _itemsPresenter.Margin.Left;
                        selectedItemWidth = itemContainer.ActualWidth;
                    }
                }
            }

            var selectedItemRight = selectedItemLeft + selectedItemWidth;
            var scrollViewportOffset = _scrollViewer.HorizontalOffset + _scrollViewer.ViewportWidth;

            if (selectedItemLeft < _scrollViewer.HorizontalOffset)
            {
                var offset = _scrollViewer.HorizontalOffset - selectedItemWidth + (selectedItemRight - _scrollViewer.HorizontalOffset);
                AnimateScroll(_scrollViewer, Math.Max(offset - selectedItemWidth / 4d, 0d));
            }
            else if (selectedItemRight > scrollViewportOffset)
            {
                var offset = _scrollViewer.HorizontalOffset + selectedItemWidth + (selectedItemLeft - scrollViewportOffset);
                AnimateScroll(_scrollViewer, Math.Min(offset + selectedItemWidth / 4d, _scrollViewer.ScrollableWidth));
            }
        }

        private static void OnIsAutoSelectEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ShowcaseControl showcaseControl && showcaseControl.IsLoaded)
            {
                showcaseControl.AutoSelectСountdown = showcaseControl.AutoSelectInterval;

                if ((bool)e.NewValue)
                {
                    showcaseControl._timer.Start();

                    if (showcaseControl.IsMouseOver)
                    {
                        showcaseControl._timer.Pause();
                    }
                }
                else
                {
                    showcaseControl._timer.Stop();
                }
            }
        }

        private static void OnAutoSelectIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ShowcaseControl showcaseControl)
            {
                showcaseControl.AutoSelectСountdown = (TimeSpan)e.NewValue;
            }
        }

        private static void OnHorizontalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }
    }
}
