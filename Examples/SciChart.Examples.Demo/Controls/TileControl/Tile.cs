using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SciChart.Examples.Demo.Helpers;

namespace SciChart.Examples.Demo.Controls.TileControl
{
    [TemplatePart(Name = "PART_PaintArea", Type = typeof(Shape)),
     TemplatePart(Name = "PART_MainContent", Type = typeof(ContentPresenter))]
    public class Tile : Button
    {
        public static readonly DependencyProperty DetailedContentProperty = DependencyProperty.Register("DetailedContent", typeof(object), typeof(Tile), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TooltipContentProperty = DependencyProperty.Register("TooltipContent", typeof(object), typeof(Tile), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty TransitionPeriodProperty = DependencyProperty.Register("TransitionPeriod", typeof(int), typeof(Tile), new PropertyMetadata(10));

        public static readonly DependencyProperty TileStateProperty = DependencyProperty.Register("TileState", typeof(TileState), typeof(Tile), new PropertyMetadata(TileState.Main, TileStatePropertyChanged));

        public static readonly DependencyProperty TransitionTimeProperty = DependencyProperty.Register("TransitionTime", typeof(TimeSpan), typeof(Tile), new PropertyMetadata(TimeSpan.FromSeconds(1.2), UpdateAnimations));

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(Tile), new PropertyMetadata(new ExponentialEase { Exponent = 9, EasingMode = EasingMode.EaseOut }, UpdateAnimations));

        public object DetailedContent
        {
            get { return (object)GetValue(DetailedContentProperty); }
            set { SetValue(DetailedContentProperty, value); }
        }

        public object TooltipContent
        {
            get { return (object)GetValue(TooltipContentProperty); }
            set { SetValue(TooltipContentProperty, value); }
        }

        public int TransitionPeriod
        {
            get { return (int)GetValue(TransitionPeriodProperty); }
            set { SetValue(TransitionPeriodProperty, value); }
        }

        public TileState TileState
        {
            get { return (TileState)GetValue(TileStateProperty); }
            set { SetValue(TileStateProperty, value); }
        }

        public TimeSpan TransitionTime
        {
            get { return (TimeSpan)GetValue(TransitionTimeProperty); }
            set { SetValue(TransitionTimeProperty, value); }
        }

        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        private bool _isLoaded;
        
        private ContentPresenter _detailsContent;
        private ContentPresenter _mainContent;

        private Storyboard _toMainAnimation;
        private Storyboard _toDetailsAnimation;
        private Storyboard _tooltipIn;
        private Storyboard _tooltipOut;

        private DoubleAnimation _animateMainIn;
        private DoubleAnimation _animateDetailsOut;
        private DoubleAnimation _animateMainOut;
        private DoubleAnimation _animateDetailsIn;

        public Tile()
        {
            DefaultStyleKey = typeof(Tile);

            MouseEnter += (_, __) => _tooltipIn.Begin();
            MouseLeave += (_, __) => _tooltipOut.Begin();

            Loaded += (sender, args) =>
            {
                _isLoaded = true;

                var temp = TransitionTime;
                TransitionTime = TimeSpan.FromSeconds(0);
                AnimateToMain();
                TransitionTime = temp;
            };

            SizeChanged += (_, __) => CreateAnimations();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _mainContent = (ContentPresenter)GetTemplateChild("PART_MainContent");
            _detailsContent = (ContentPresenter)GetTemplateChild("PART_DetailsContent");

            _tooltipIn = (Storyboard)GetTemplateChild("TooltipIn");
            _tooltipOut = (Storyboard)GetTemplateChild("TooltipOut");
        }

        private void AnimateToMain()
        {
            if (_mainContent != null && _detailsContent != null && _toMainAnimation != null)
            {
                _mainContent.Visibility = Visibility.Visible;
                _detailsContent.IsHitTestVisible = false;

                _toMainAnimation.Begin();
            }
        }

        private void AnimateToDetails()
        {
            if (_mainContent != null && _detailsContent != null && _toDetailsAnimation != null)
            {
                _detailsContent.Visibility = Visibility.Visible;
                _mainContent.IsHitTestVisible = false;

                _toDetailsAnimation.Begin();
            }
        }

        private void CreateAnimations()
        {
            _toMainAnimation = new Storyboard();
            _toDetailsAnimation = new Storyboard();

            _animateMainIn = CreateAnimation(-ActualHeight, 0);
            SetStoryboardTargets(_animateMainIn, _toMainAnimation, _mainContent);

            _animateDetailsOut = CreateAnimation(0, ActualHeight, (s, e) =>
            {
                _detailsContent.Visibility = Visibility.Collapsed;
                _mainContent.IsHitTestVisible = true;
            });
            SetStoryboardTargets(_animateDetailsOut, _toMainAnimation, _detailsContent);

            _animateMainOut = CreateAnimation(0, -ActualHeight, (s, e) =>
            {
                _mainContent.Visibility = Visibility.Collapsed;
                _detailsContent.IsHitTestVisible = true;
            });
            SetStoryboardTargets(_animateMainOut, _toDetailsAnimation, _mainContent);

            _animateDetailsIn = CreateAnimation(ActualHeight, 0);
            SetStoryboardTargets(_animateDetailsIn, _toDetailsAnimation, _detailsContent);
        }

        private DoubleAnimation CreateAnimation(double from, double to, EventHandler whenDone = null)
        {
            var animation = new DoubleAnimation { From = from, To = to, Duration = new Duration(TransitionTime), EasingFunction = EasingFunction };

            if (whenDone != null)
            {
                animation.Completed += whenDone;
            }

            return animation;
        }

        private void SetStoryboardTargets(Timeline animation, Storyboard storyboard, ContentPresenter target)
        {
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
        }

        private static void TileStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var tile = d as Tile;
            var newState = (TileState)args.NewValue;

            if (tile != null)
            {
                if (newState == TileState.Main)
                {
                    tile.AnimateToMain();
                }
                else
                {
                    tile.AnimateToDetails();
                }
            }
        }

        private static void UpdateAnimations(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var tile = d as Tile;
            if (tile != null && tile._isLoaded)
            {
                tile.CreateAnimations();
            }
        }
    }
}
