using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting.Visuals.RenderableSeries.Animations;
using SciChart.Core.Extensions;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers
{
    public class SeriesAnimationCustomModifier : ChartModifierBase
    {
        public SeriesAnimationCustomModifier()
        {
            InitCommands();
        }

        public static readonly DependencyProperty FadeAnimationCommandProperty = DependencyProperty
            .Register(nameof(FadeAnimationCommand), typeof(ICommand), typeof(SeriesAnimationCustomModifier), new PropertyMetadata(null));

        public static readonly DependencyProperty ScaleAnimationCommandProperty = DependencyProperty
            .Register(nameof(ScaleAnimationCommand), typeof(ICommand), typeof(SeriesAnimationCustomModifier), new PropertyMetadata(null));

        public static readonly DependencyProperty SweepAnimationCommandProperty = DependencyProperty
            .Register(nameof(SweepAnimationCommand), typeof(ICommand), typeof(SeriesAnimationCustomModifier), new PropertyMetadata(null));

        public static readonly DependencyProperty WaveAnimationCommandProperty = DependencyProperty
            .Register(nameof(WaveAnimationCommand), typeof(ICommand), typeof(SeriesAnimationCustomModifier), new PropertyMetadata(null));

        public static readonly DependencyProperty StartAnimationCommandProperty =
            DependencyProperty.Register(nameof(StartAnimationCommand), typeof(ICommand), typeof(SeriesAnimationCustomModifier), new PropertyMetadata(null));

        public static readonly DependencyProperty StopAnimationCommandProperty =
            DependencyProperty.Register(nameof(StopAnimationCommand), typeof(ICommand), typeof(SeriesAnimationCustomModifier), new PropertyMetadata(null));

        public ICommand StartAnimationCommand
        {
            get { return (ICommand)GetValue(StartAnimationCommandProperty); }
            set { SetValue(StartAnimationCommandProperty, value); }
        }

        public ICommand StopAnimationCommand
        {
            get { return (ICommand)GetValue(StopAnimationCommandProperty); }
            set { SetValue(StopAnimationCommandProperty, value); }
        }

        public ICommand FadeAnimationCommand
        {
            get { return (ICommand)GetValue(FadeAnimationCommandProperty); }
            set { SetValue(FadeAnimationCommandProperty, value); }
        }

        public ICommand ScaleAnimationCommand
        {
            get { return (ICommand)GetValue(ScaleAnimationCommandProperty); }
            set { SetValue(ScaleAnimationCommandProperty, value); }
        }

        public ICommand SweepAnimationCommand
        {
            get { return (ICommand)GetValue(SweepAnimationCommandProperty); }
            set { SetValue(SweepAnimationCommandProperty, value); }
        }

        public ICommand WaveAnimationCommand
        {
            get { return (ICommand)GetValue(WaveAnimationCommandProperty); }
            set { SetValue(WaveAnimationCommandProperty, value); }
        }

        private void InitCommands()
        {
            FadeAnimationCommand = new ActionCommand(() => { FadeAnimation(); });
            ScaleAnimationCommand = new ActionCommand(() => { ScaleAnimation(); });
            SweepAnimationCommand = new ActionCommand(() => { SweepAnimation(); });
            WaveAnimationCommand = new ActionCommand(() => { WaveAnimation(); });

            StartAnimationCommand = new ActionCommand(() => { ProcessAnimation(a => a?.StartAnimation()); });
            StopAnimationCommand = new ActionCommand(() => { ProcessAnimation(a => a?.StopAnimation()); });
        }

        private void ProcessAnimation(Action<ISeriesAnimation> doAction)
        {
            ParentSurface
                .RenderableSeries
                .OfType<BaseRenderableSeries>()
                .ForEachDo(rs => doAction(rs.SeriesAnimation));
        }

        private void FadeAnimation()
        {
            foreach (var s in ParentSurface.RenderableSeries)
            {
                if (s is BaseRenderableSeries series)
                {
                    var trans = new FadeAnimation();
                    trans.Duration = TimeSpan.FromSeconds(5);
                    trans.AnimationDelay = TimeSpan.FromSeconds(1);
                    trans.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };

                    series.SeriesAnimation = trans;
                }
            }
        }

        private void ScaleAnimation()
        {
            foreach (var s in ParentSurface.RenderableSeries)
            {
                if (s is BaseRenderableSeries series)
                {
                    var trans = new ScaleAnimation();
                    trans.Duration = TimeSpan.FromSeconds(5);
                    trans.AnimationDelay = TimeSpan.FromSeconds(1);
                    trans.EasingFunction = new BounceEase { Bounces = 2, EasingMode = EasingMode.EaseInOut, Bounciness = 3 };
                    if (s.DataSeries != null)
                    {
                        trans.ZeroLine = (Convert.ToDouble(series.DataSeries.YMax) +
                                          Convert.ToDouble(series.DataSeries.YMin)) / 2.0;
                    }

                    series.SeriesAnimation = trans;
                }
            }
        }

        private void SweepAnimation()
        {
            foreach (var s in ParentSurface.RenderableSeries)
            {
                if (s is BaseRenderableSeries series)
                {
                    var trans = new SweepAnimation();
                    trans.Duration = TimeSpan.FromSeconds(5);
                    trans.AnimationDelay = TimeSpan.FromSeconds(1);
                    trans.EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn };

                    series.SeriesAnimation = trans;
                }
            }
        }

        private void WaveAnimation()
        {
            foreach (var s in ParentSurface.RenderableSeries)
            {
                if (s is BaseRenderableSeries series)
                {
                    var trans = new WaveAnimation();
                    trans.Duration = TimeSpan.FromSeconds(2);
                    trans.AnimationDelay = TimeSpan.FromSeconds(2);
                    if (s.DataSeries != null)
                    {
                        trans.ZeroLine = (double) series.DataSeries.YMin;
                    }

                    series.SeriesAnimation = trans;
                }
            }
        }
    }
}
