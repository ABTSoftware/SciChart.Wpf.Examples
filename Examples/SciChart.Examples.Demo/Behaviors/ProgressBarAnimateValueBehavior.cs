using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Demo.Behaviors
{
    public class ProgressValues
    {
        public bool HasValues { get; private set; }
        public double OldValue { get; private set; }
        public double NewValue { get; private set; }

        public void Update(double oldValue, double newValue)
        {
            if (double.IsNaN(oldValue) || double.IsNaN(newValue))
                throw new ArgumentException("Invalid progress bar values");

            if (HasValues == false)
                OldValue = oldValue;
                             
            NewValue = newValue;
            HasValues = true;
        }

        public void Clear()
        {
            HasValues = false;
            OldValue = double.NaN;
            NewValue = double.NaN;
        }
    }

    public class ProgressBarAnimateValueBehavior : Behavior<ProgressBar>
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
            (nameof(Value), typeof(double), typeof(ProgressBarAnimateValueBehavior), new PropertyMetadata(0d, OnValuePropertyChanged));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register
            (nameof(Duration), typeof(TimeSpan), typeof(ProgressBarAnimateValueBehavior), new PropertyMetadata(TimeSpan.FromSeconds(1d)));

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

        private bool _isAnimating;
        private readonly DoubleAnimation _valueAnimation;

        private readonly object _progressLock = new();
        private readonly ProgressValues _progressValues = new();

        public ProgressBarAnimateValueBehavior()
        {
            _valueAnimation = new DoubleAnimation();
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

        private void AnimateProgressBarValue(double oldValue, double newValue)
        {
            if (AssociatedObject != null)
            {
                _isAnimating = true;

                _valueAnimation.From = oldValue;
                _valueAnimation.To = newValue;
                _valueAnimation.Duration = Duration;

                AssociatedObject.BeginAnimation(RangeBase.ValueProperty, _valueAnimation);
            }
        }

        private void TryAnimateProgressBarValue(double oldValue, double newValue)
        {
            lock (_progressLock)
            {
                if (_isAnimating)
                {
                    _progressValues.Update(oldValue, newValue);
                }
                else
                {
                    AnimateProgressBarValue(oldValue, newValue);
                }
            }
        }

        private void OnValueAnimationCompleted(object sender, EventArgs e)
        {
            lock (_progressLock)
            {
                _isAnimating = false;

                if (_progressValues.HasValues)
                {
                    var oldValue = _progressValues.OldValue;
                    var newValue = _progressValues.NewValue;

                    _progressValues.Clear();

                    AnimateProgressBarValue(oldValue, newValue);
                }
            }
        }
    }
}