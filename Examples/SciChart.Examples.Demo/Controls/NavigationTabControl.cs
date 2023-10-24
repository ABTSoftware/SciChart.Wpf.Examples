using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Demo.Controls
{
    [TemplatePart(Name = "PART_TabPanel", Type = typeof(TabPanel))]
    [TemplatePart(Name = "PART_SliderBorder", Type = typeof(Border))]
    public class NavigationTabControl : TabControl
    {
        public static DependencyProperty SliderWidthProperty = DependencyProperty.Register
            (nameof(SliderWidth), typeof(double), typeof(NavigationTabControl), new PropertyMetadata(double.NaN, OnSliderWidthChanged));

        public static DependencyProperty SliderStretchProperty = DependencyProperty.Register
            (nameof(SliderStretch), typeof(double), typeof(NavigationTabControl), new PropertyMetadata(10d));

        [TypeConverter(typeof(LengthConverter))]
        public double SliderWidth
        {
            get => (double)GetValue(SliderWidthProperty);
            set => SetValue(SliderWidthProperty, value);
        }

        public double SliderStretch
        {
            get => (double)GetValue(SliderStretchProperty);
            set => SetValue(SliderStretchProperty, value);
        }

        private static void OnSliderWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationTabControl navTabControl && navTabControl.IsLoaded)
            {
                navTabControl.ValidateSliderPosition();
            }
        }

        private TabPanel _tabPanel;
        private Border _sliderBorder;

        private int _selectedIndex;
        private double _sliderPosition;
        private double _sliderWidth;

        private readonly Storyboard _sliderRightStoryboard;
        private readonly Storyboard _sliderLeftStoryboard;

        public NavigationTabControl()
        {
            DefaultStyleKey = typeof(NavigationTabControl);

            _sliderLeftStoryboard = new Storyboard { Duration = TimeSpan.FromMilliseconds(425) };
            _sliderRightStoryboard = new Storyboard { Duration = TimeSpan.FromMilliseconds(425) };

            Loaded += (s, e) => ValidateSliderPosition();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (IsLoaded)
            {
                ValidateSliderPosition();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _tabPanel = GetAndAssertTemplateChild<TabPanel>("PART_TabPanel");
            _sliderBorder = GetAndAssertTemplateChild<Border>("PART_SliderBorder");
        }

        private T GetAndAssertTemplateChild<T>(string childName) where T : class
        {
            if (GetTemplateChild(childName) is T templateChild)
            {
                return templateChild;
            }
            throw new InvalidOperationException($"Unable to load '{childName}' template part");
        }

        private void ValidateSliderPosition()
        {
            if (SelectedIndex >= 0 && _tabPanel.Children[SelectedIndex] is TabItem tabItem && tabItem.ActualWidth > 0)
            {
                double newSliderWidth, newSliderPosition;

                if (SliderWidth.IsNaN())
                {
                    newSliderWidth = tabItem.ActualWidth - tabItem.Padding.Left - tabItem.Padding.Right;
                    newSliderPosition = tabItem.TranslatePoint(new Point(0, 0), this).X + tabItem.Padding.Left;
                }
                else
                {
                    newSliderWidth = SliderWidth;
                    newSliderPosition = tabItem.TranslatePoint(new Point(0, 0), this).X + tabItem.ActualWidth / 2 - SliderWidth / 2;
                }

                if (SelectedIndex < _selectedIndex)
                {
                    BeginSlideLeftStoryboard(newSliderWidth, newSliderPosition);
                }
                else
                {
                    BeginSlideRightStoryboard(newSliderWidth, newSliderPosition);
                }

                _selectedIndex = SelectedIndex;
                _sliderPosition = newSliderPosition;
                _sliderWidth = newSliderWidth;
            }
        }
        private void BeginSlideRightStoryboard(double newSliderWidth, double newSliderPosition)
        {
            _sliderRightStoryboard.Children.Clear();

            var stretchWidthAnimation = new DoubleAnimation
            {
                From = _sliderWidth,
                To = _sliderWidth + SliderStretch,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };

            Storyboard.SetTarget(stretchWidthAnimation, _sliderBorder);
            Storyboard.SetTargetProperty(stretchWidthAnimation, new PropertyPath(WidthProperty));
            _sliderRightStoryboard.Children.Add(stretchWidthAnimation);

            var slidePositionAnimation = new DoubleAnimation
            {
                From = _sliderPosition,
                To = newSliderPosition - SliderStretch,
                BeginTime = TimeSpan.FromMilliseconds(250),
                Duration = new Duration(TimeSpan.FromMilliseconds(25))
            };

            Storyboard.SetTarget(slidePositionAnimation, _sliderBorder);
            Storyboard.SetTargetProperty(slidePositionAnimation, new PropertyPath(Canvas.LeftProperty));
            _sliderRightStoryboard.Children.Add(slidePositionAnimation);

            var squeezePositionAnimation = new DoubleAnimation
            {
                From = newSliderPosition - SliderStretch,
                To = newSliderPosition,
                BeginTime = TimeSpan.FromMilliseconds(250),
                Duration = new Duration(TimeSpan.FromMilliseconds(150))
            };

            Storyboard.SetTarget(squeezePositionAnimation, _sliderBorder);
            Storyboard.SetTargetProperty(squeezePositionAnimation, new PropertyPath(Canvas.LeftProperty));
            _sliderRightStoryboard.Children.Add(squeezePositionAnimation);

            var squeezeWidthAnimation = new DoubleAnimation
            {
                From = _sliderWidth + SliderStretch,
                To = newSliderWidth,
                BeginTime = TimeSpan.FromMilliseconds(250),
                Duration = new Duration(TimeSpan.FromMilliseconds(150))
            };

            Storyboard.SetTarget(squeezeWidthAnimation, _sliderBorder);
            Storyboard.SetTargetProperty(squeezeWidthAnimation, new PropertyPath(WidthProperty));
            _sliderRightStoryboard.Children.Add(squeezeWidthAnimation);

            _sliderBorder.BeginStoryboard(_sliderRightStoryboard);
        }

        private void BeginSlideLeftStoryboard(double newSliderWidth, double newSliderPosition)
        {
            _sliderLeftStoryboard.Children.Clear();

            var stretchWidthAnimation = new DoubleAnimation
            {
                From = _sliderWidth,
                To = _sliderWidth + SliderStretch,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };

            Storyboard.SetTarget(stretchWidthAnimation, _sliderBorder);
            Storyboard.SetTargetProperty(stretchWidthAnimation, new PropertyPath(WidthProperty));
            _sliderLeftStoryboard.Children.Add(stretchWidthAnimation);

            var stretchPositionAnimation = new DoubleAnimation
            {
                From = _sliderPosition,
                To = _sliderPosition - SliderStretch,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };

            Storyboard.SetTarget(stretchPositionAnimation, _sliderBorder);
            Storyboard.SetTargetProperty(stretchPositionAnimation, new PropertyPath(Canvas.LeftProperty));
            _sliderLeftStoryboard.Children.Add(stretchPositionAnimation);

            var slidePositionAnimation = new DoubleAnimation
            {
                From = _sliderPosition - SliderStretch,
                To = newSliderPosition,
                BeginTime = TimeSpan.FromMilliseconds(250),
                Duration = new Duration(TimeSpan.FromMilliseconds(25))
            };

            Storyboard.SetTarget(slidePositionAnimation, _sliderBorder);
            Storyboard.SetTargetProperty(slidePositionAnimation, new PropertyPath(Canvas.LeftProperty));
            _sliderLeftStoryboard.Children.Add(slidePositionAnimation);

            var squeezeWidthAnimation = new DoubleAnimation
            {
                From = _sliderWidth + SliderStretch,
                To = newSliderWidth,
                BeginTime = TimeSpan.FromMilliseconds(250),
                Duration = new Duration(TimeSpan.FromMilliseconds(150))
            };

            Storyboard.SetTarget(squeezeWidthAnimation, _sliderBorder);
            Storyboard.SetTargetProperty(squeezeWidthAnimation, new PropertyPath(WidthProperty));
            _sliderLeftStoryboard.Children.Add(squeezeWidthAnimation);

            _sliderBorder.BeginStoryboard(_sliderLeftStoryboard);
        }
    }
}
