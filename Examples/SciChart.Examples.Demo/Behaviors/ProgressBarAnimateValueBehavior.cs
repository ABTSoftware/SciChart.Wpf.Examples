using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Demo.Behaviors
{
    public class ProgressValue
    {
        public double OldValue { get; set; }
        public double NewValue { get; set; }
    }

    public class ProgressBarAnimateValueBehavior : Behavior<ProgressBar>
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
            (nameof(Value), typeof(double), typeof(ProgressBarAnimateValueBehavior),
            new PropertyMetadata(0d, OnValuePropertyChanged));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register
            (nameof(Duration), typeof(TimeSpan), typeof(ProgressBarAnimateValueBehavior),
            new PropertyMetadata(TimeSpan.FromSeconds(0.5), OnDurationPropertyChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public TimeSpan Duration
        {
            get => (TimeSpan)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressBarAnimateValueBehavior behavior)
            {
                behavior.TryAnimateProgressBarValue((double)e.OldValue, (double)e.NewValue); 
            }
        }

        private static void OnDurationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressBarAnimateValueBehavior behavior)
            {
                behavior._duration = new Duration((TimeSpan)e.NewValue);
            }
        }

        private bool _isAnimating;
        private Duration _duration;

        private ProgressValue _progressValue;
        private readonly DoubleAnimation _valueAnimation;

        public ProgressBarAnimateValueBehavior()
        {
            _valueAnimation = new DoubleAnimation();
            _duration = new Duration(TimeSpan.FromSeconds(0.5));
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            _valueAnimation.Completed -= OnValueAnimationCompleted;
            _valueAnimation.Completed += OnValueAnimationCompleted;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            _valueAnimation.Completed -= OnValueAnimationCompleted;
        }

        private void TryAnimateProgressBarValue(double oldValue, double newValue)
        {
            if (_isAnimating)
            {
                if (_progressValue == null)
                {
                    _progressValue = new ProgressValue
                    {
                        OldValue = oldValue,
                        NewValue = newValue
                    };
                }
                else
                {
                    _progressValue.NewValue = newValue;
                }

                return;
            }

            AnimateProgressBarValue(oldValue, newValue);
        }

        private void AnimateProgressBarValue(double oldValue, double newValue)
        {
            if (AssociatedObject != null)
            {
                _isAnimating = true;

                _valueAnimation.From = oldValue;
                _valueAnimation.To = newValue;
                _valueAnimation.Duration = _duration;

                AssociatedObject.BeginAnimation(RangeBase.ValueProperty, _valueAnimation);
            }
        }

        private void OnValueAnimationCompleted(object sender, EventArgs e)
        {
            _isAnimating = false;

            if (_progressValue != null)
            {
                var oldValue = _progressValue.OldValue;
                var newValue = _progressValue.NewValue;

                _progressValue = null;
                AnimateProgressBarValue(oldValue, newValue);
            }
        }
    }
}