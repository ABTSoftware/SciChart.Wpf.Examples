using System;
using System.Linq;
using System.Reactive.Linq;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.ViewModels;
using SciChart.UI.Reactive;
using SciChart.UI.Reactive.Async;
using SciChart.UI.Reactive.Observability;
using SciChart.UI.Reactive.Traits;
using SciChart.Wpf.UI.Transitionz;

namespace SciChart.Examples.Demo.Behaviors
{
    public class InitializationBehaviour : ViewModelTrait<MainWindowViewModel>
    {
        private readonly IModule _module;

        private readonly IBlurParams _defaultParams = new BlurParams { Duration = 120, From = 8, To = 0 };
        private readonly IBlurParams _blurredParams = new BlurParams { Duration = 200, From = 0, To = 8 };

        public InitializationBehaviour(MainWindowViewModel target, IModule module, ISchedulerContext schedulerContext) : base(target)
        {
            _module = module;

            Target.ShowcaseExamples = _module.ShowcaseExamples
                .Select(x => x.Value)
                .Distinct(new ExampleEqualityComparer())
                .ShuffleToList();

            Target.Categories = _module.GroupsByCategory
                .Select(x => new ExampleCategoryViewModel { Category = x.Key, Groups = x.Value })
                .ToList();

            Target.SelectedShowcaseExample = Target.ShowcaseExamples.FirstOrDefault();

            int midCount = (Target.Categories.Count() - 1) / 2;
            if (midCount >= 0 && midCount < Target.Categories.Count())
            {
                Target.SelectedCategory = Target.Categories.ElementAt(midCount);
                Target.SelectedCategory.IsHomeCategory = true;
            }

            Target.EverythingViewModel = new EverythingViewModel();

            Target.WhenPropertyChanged(x => x.SelectedCategory)
                .Do(_ => Target.IsBusy = true)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Do(c => Target.EverythingViewModel.SelectedCategory = c?.Category)
                .Delay(TimeSpan.FromMilliseconds(200), schedulerContext.Default)
                .ObserveOn(schedulerContext.Dispatcher)
                .Subscribe(_ => Target.IsBusy = false)
                .DisposeWith(this);

            Target.WhenPropertyChanged(x => x.IsBusy)
                .Skip(1)
                .Subscribe(b => Target.BlurBackgroundParams = b ? _blurredParams : _defaultParams)
                .DisposeWith(this);
        }
    }
}