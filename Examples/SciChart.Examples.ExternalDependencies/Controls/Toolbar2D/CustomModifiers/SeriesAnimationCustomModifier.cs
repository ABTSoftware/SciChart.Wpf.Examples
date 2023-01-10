using System;
using System.Linq;
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
        public ICommand FadeAnimationCommand { get; }

        public ICommand ScaleAnimationCommand { get; }

        public ICommand SweepAnimationCommand { get; }

        public ICommand WaveAnimationCommand { get; }

        public SeriesAnimationCustomModifier()
        {
            FadeAnimationCommand = new ActionCommand(() => ProcessAnimation(FadeAnimation));
            ScaleAnimationCommand = new ActionCommand(() => ProcessAnimation(ScaleAnimation));
            SweepAnimationCommand = new ActionCommand(() => ProcessAnimation(SweepAnimation));
            WaveAnimationCommand = new ActionCommand(() => ProcessAnimation(WaveAnimation));
        }

        private void ProcessAnimation(Action<BaseRenderableSeries> setAnimationAction)
        {
            ParentSurface.RenderableSeries
                .OfType<BaseRenderableSeries>()
                .ForEachDo(rs =>
                {
                    setAnimationAction(rs);
                    rs.SeriesAnimation.StartAnimation();
                });
        }

        private void FadeAnimation(BaseRenderableSeries series)
        {
            if (series != null)
            {
                series.SeriesAnimation = new FadeAnimation
                {
                    Duration = TimeSpan.FromSeconds(5),
                    AnimationDelay = TimeSpan.FromSeconds(1),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
            }
        }

        private void ScaleAnimation(BaseRenderableSeries series)
        {
            if (series != null)
            {
                var animation = new ScaleAnimation
                {
                    Duration = TimeSpan.FromSeconds(5),
                    AnimationDelay = TimeSpan.FromSeconds(1),
                    EasingFunction = new BounceEase { Bounces = 2, EasingMode = EasingMode.EaseInOut, Bounciness = 3 }
                };

                if (series.DataSeries != null)
                {
                    animation.ZeroLine = (Convert.ToDouble(series.DataSeries.YMax) +
                                          Convert.ToDouble(series.DataSeries.YMin)) / 2.0;
                }

                series.SeriesAnimation = animation;
            }
        }

        private void SweepAnimation(BaseRenderableSeries series)
        {
            if (series != null)
            {
                series.SeriesAnimation = new SweepAnimation
                {
                    Duration = TimeSpan.FromSeconds(5),
                    AnimationDelay = TimeSpan.FromSeconds(1),
                    EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn }
                };
            }
        }

        private void WaveAnimation(BaseRenderableSeries series)
        {
            if (series != null)
            {
                var animation = new WaveAnimation
                {
                    Duration = TimeSpan.FromSeconds(2),
                    AnimationDelay = TimeSpan.FromSeconds(2)
                };

                if (series.DataSeries != null)
                {
                    animation.ZeroLine = series.DataSeries.YMin.ToDouble();
                }

                series.SeriesAnimation = animation;
            }
        }
    }
}
