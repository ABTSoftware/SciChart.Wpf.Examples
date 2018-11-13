using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SciChart.Examples.Demo.Controls
{
    public class IndeterminateProgressBar : Control
    {
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof (bool), typeof (IndeterminateProgressBar), new PropertyMetadata(false, IsBusyPropertyChanged));

        public bool IsBusy
        {
            get { return (bool) GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        private double _knownWidth;
        private double _knownHeight;
        private bool _templateApplied;

        private readonly Storyboard _storyboard = new Storyboard {Duration = TimeSpan.FromSeconds(4.4)};

        private readonly List<DoubleAnimationUsingKeyFrames> _keyFramesAnimations =
            new List<DoubleAnimationUsingKeyFrames>();

        private readonly List<Rectangle> _rects = new List<Rectangle>();

        private FrameworkElement _root;

        public IndeterminateProgressBar()
        {
            DefaultStyleKey = typeof (IndeterminateProgressBar);

            SizeChanged += OnSizeChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _root = (FrameworkElement) GetTemplateChild("Root");
            for (int i = 0; i < 5; i++)
            {
                var rect = (Rectangle) GetTemplateChild(string.Format("R{0}", i + 1));
                _rects.Add(rect);
            }

            if (_storyboard != null)
            {
                SetUpAnimations();
            }

            _templateApplied = true;
            UpdateBusyState(IsBusy);
        }

        private void SetUpAnimations()
        {
            for (int i = 0; i < 5; i++)
            {
                var doubleAnimation = new DoubleAnimationUsingKeyFrames {BeginTime = TimeSpan.FromSeconds(0.2*i),};
                CreateAnimations(doubleAnimation);

                Storyboard.SetTarget(doubleAnimation, _rects[i]);
                Storyboard.SetTargetProperty(doubleAnimation,
                    new PropertyPath("(Rectangle.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));

                var opacityAnimation = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromSeconds(0.2*i),
                };
                opacityAnimation.KeyFrames.Clear();
                opacityAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame {KeyTime = TimeSpan.FromSeconds(0), Value = 1});
                opacityAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame {KeyTime = TimeSpan.FromSeconds(2.5), Value = 0});

                Storyboard.SetTarget(opacityAnimation, _rects[i]);
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

                _storyboard.Children.Add(doubleAnimation);
                _keyFramesAnimations.Add(doubleAnimation);

                _storyboard.Children.Add(opacityAnimation);
            }

            _storyboard.Completed += (_, __) => _storyboard.Begin();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            {
                _knownWidth = args.NewSize.Width;
                _knownHeight = args.NewSize.Height;

                Clip = new RectangleGeometry {Rect = new Rect(0, 0, _knownWidth, _knownHeight),};
                UpdateAnimations();
            }
        }

        private void CreateAnimations(DoubleAnimationUsingKeyFrames animation)
        {
            animation.KeyFrames.Clear();

            animation.KeyFrames.Add(new LinearDoubleKeyFrame {KeyTime = TimeSpan.FromSeconds(0), Value = 0.001*_knownWidth});
            animation.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                EasingFunction = new ExponentialEase {Exponent = 1, EasingMode = EasingMode.EaseOut},
                KeyTime = TimeSpan.FromSeconds(0.5),
                Value = 0.331*_knownWidth
            });
            animation.KeyFrames.Add(new LinearDoubleKeyFrame {KeyTime = TimeSpan.FromSeconds(2), Value = 0.661*_knownWidth});
            animation.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                EasingFunction = new ExponentialEase {Exponent = 1, EasingMode = EasingMode.EaseIn},
                KeyTime = TimeSpan.FromSeconds(2.5),
                Value = 1.001*_knownWidth
            });
        }

        private void UpdateAnimations()
        {
            foreach (var animation in _keyFramesAnimations)
            {
                animation.KeyFrames[0].Value = 0.001*_knownWidth;
                animation.KeyFrames[1].Value = 0.331*_knownWidth;
                animation.KeyFrames[2].Value = 0.661*_knownWidth;
                animation.KeyFrames[3].Value = 1.001*_knownWidth;
            }
        }

        private void UpdateBusyState(bool isBusy)
        {
            if (_root != null)
            {
                _root.Visibility = isBusy ? Visibility.Visible : Visibility.Collapsed;
            }
            if (isBusy)
            {
                _storyboard.Begin();
            }
            else
            {
                _storyboard.Stop();
            }
        }

        private static void IsBusyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var bar = (IndeterminateProgressBar) d;
            if (bar != null && bar._templateApplied)
            {
                bar.UpdateBusyState((bool) args.NewValue);
            }
        }
    }

}