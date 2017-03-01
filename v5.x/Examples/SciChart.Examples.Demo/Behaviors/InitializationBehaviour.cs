using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Search;
using SciChart.Examples.Demo.ViewModels;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Async;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Reactive.Traits;
using SciChart.Wpf.UI.Transitionz;

namespace SciChart.Examples.Demo.Behaviors
{
    public class InitializationBehaviour : ViewModelTrait<MainWindowViewModel>
    {
        private readonly IModule _module;

        private readonly IBlurParams _defaultParams = new BlurParams() { Duration = 120, From = 8, To = 0 };
        private readonly IBlurParams _blurredParams = new BlurParams() { Duration = 200, From = 0, To = 8 };

        public InitializationBehaviour(MainWindowViewModel target, IModule module, ISchedulerContext schedulerContext)
            : base(target)
        {
            _module = module;

            Task.Factory.StartNew(() =>
            {
                CreateInvertedIndex.CreateIndex(_module.Examples);
                CreateInvertedIndex.CreateIndexForCode(_module.Examples);
            })
            .Then(() => Target.SearchBoxEnabled = true)
            .Catch(ex => target.Exception = new ExceptionViewModel("Initialization Error", ex.Flatten()));        

            var groupsByCategory = _module.GroupsByCategory;            

            Target.Categories = groupsByCategory.Select(y => new ExampleCategoryViewModel() { Category = y.Key, Groups = y.Value}).ToList();            
            int midCount = (Target.Categories.Count() - 1) / 2;
            if (midCount >= 0 && midCount < Target.Categories.Count())
                Target.SelectedCategory = Target.Categories.ElementAt(midCount);

            Target.EverythingViewModel = new EverythingViewModel();

            Target.WhenPropertyChanged(x => x.SelectedCategory)
                .Do(_ => Target.IsBusy = true)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Do(c => Target.EverythingViewModel.SelectedCategory = c != null ? c.Category : null)
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