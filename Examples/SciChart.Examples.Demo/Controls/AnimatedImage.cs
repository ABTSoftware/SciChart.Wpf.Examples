using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SciChart.Examples.Demo.Controls
{
    [TemplatePart(Name = "PART_Image", Type = typeof(Image))]
    public class AnimatedImage : Control
    {
        public static DependencyProperty SourceProperty = DependencyProperty.Register
            (nameof(Source), typeof(ImageSource), typeof(AnimatedImage), new PropertyMetadata(null, OnSourcePropertyChanged));

        public static DependencyProperty FadeDurationProperty = DependencyProperty.Register
            (nameof(FadeDuration), typeof(TimeSpan), typeof(AnimatedImage), new PropertyMetadata(TimeSpan.FromMilliseconds(500)));

        public static DependencyProperty PreserveAspectRatioProperty = DependencyProperty.Register
            (nameof(PreserveAspectRatio), typeof(bool), typeof(AnimatedImage), new PropertyMetadata(true, OnPreserveAspectRatioPropertyChanged));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public TimeSpan FadeDuration
        {
            get => (TimeSpan)GetValue(FadeDurationProperty);
            set => SetValue(FadeDurationProperty, value);
        }

        public bool PreserveAspectRatio
        {
            get => (bool)GetValue(PreserveAspectRatioProperty);
            set => SetValue(PreserveAspectRatioProperty, value);
        }

        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedImage animatedImage && animatedImage.IsLoaded)
            {
                animatedImage.AnimateSourceChange((ImageSource)e.NewValue);
            }
        }

        private static void OnPreserveAspectRatioPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedImage animatedImage && animatedImage.IsLoaded)
            {
                animatedImage.ValidateImageStretch((bool)e.NewValue);
            }
        }

        private Image _image;
   
        public AnimatedImage()
        {
            DefaultStyleKey = typeof(AnimatedImage);

            Loaded += (s, e) =>
            {
                ValidateImageStretch(PreserveAspectRatio);
                AnimateSourceChange(Source);
            };
        }

        private void OnFadeOutAnimationCompleted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = GetAndAssertTemplateChild<Image>("PART_Image");
        }

        private T GetAndAssertTemplateChild<T>(string childName) where T : class
        {
            if (GetTemplateChild(childName) is T templateChild)
            {
                return templateChild;
            }
            throw new InvalidOperationException($"Unable to load '{childName}' template part");
        }

        private void ValidateImageStretch(bool preserveAspectRatio)
        {
            _image.Stretch = preserveAspectRatio ? Stretch.Uniform : Stretch.Fill; 
        }

        private void AnimateSourceChange(ImageSource imageSource)
        {
            var animationDuration = TimeSpan.FromMilliseconds(FadeDuration.Milliseconds / 2);

            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1d,
                To = 0d,
                Duration = animationDuration
            };

            fadeOutAnimation.Completed += (s, e) =>
            {
                _image.Source = imageSource;

                var fadeInAnimation = new DoubleAnimation
                {
                    From = 0d,
                    To = 1d,
                    Duration = animationDuration
                };

                _image.BeginAnimation(OpacityProperty, fadeInAnimation);
            };

            _image.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }
    }
}