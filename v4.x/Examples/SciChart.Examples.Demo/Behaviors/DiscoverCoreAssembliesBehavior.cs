using System;
using System.Reactive.Linq;
using SciChart.Examples.Demo.ViewModels;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Async;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Reactive.Traits;

namespace SciChart.Examples.Demo.Behaviors
{
    public class DiscoverCoreAssembliesBehavior : ViewModelTrait<ExportExampleViewModel>
    {
        

        public DiscoverCoreAssembliesBehavior(ExportExampleViewModel target, ISchedulerContext schedulerContext) 
            : base(target)
        {
            Target.LibrariesPath = ExportExampleHelper.TryAutomaticallyFindAssemblies();            

            Target.WhenPropertyChanged(x => x.LibrariesPath)
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(ExportExampleHelper.SearchForCoreAssemblies)
                .ObserveOn(schedulerContext.Dispatcher)
                .Subscribe(b => Target.IsAssemblyOk = b)
                .DisposeWith(this); 
        }    
    }
}