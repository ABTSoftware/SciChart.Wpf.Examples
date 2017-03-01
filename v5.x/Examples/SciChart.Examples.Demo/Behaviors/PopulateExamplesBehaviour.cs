using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.ViewModels;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Async;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Reactive.Traits;

namespace SciChart.Examples.Demo.Behaviors
{
    public class PopulateExamplesBehaviour : ViewModelTrait<EverythingViewModel>
    {
        public PopulateExamplesBehaviour(EverythingViewModel target, IModule module, ISchedulerContext schedulerContext) : base(target)
        {
            var categoryObs = Target.WhenPropertyChanged(x => x.SelectedCategory)
                .Do(c => 
                {
                    var allExamples = module.Examples;
                    var newDict = new Dictionary<Guid, Example>();
                    foreach (var example in allExamples)
                    {
                        if (c == null || example.Value.TopLevelCategory == c)
                        {
                            newDict.Add(example.Key, example.Value);
                        }
                    }
                    Target.Examples = newDict;
                });

            Observable.CombineLatest(Target.WhenPropertyChanged(x => x.SelectedGroupingMode), categoryObs, Tuple.Create)
                .Where(tpl => tpl.Item1 != null && tpl.Item2 != null)
                .ObserveOn(schedulerContext.Dispatcher)
                .Subscribe(tpl => Target.UpdateEverythingSource(tpl.Item1.GroupingPredicate))
                .DisposeWith(this);
        }
    }
}