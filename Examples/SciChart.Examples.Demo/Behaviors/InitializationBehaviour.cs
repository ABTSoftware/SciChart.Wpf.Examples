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

            Target.EverythingViewModel = new EverythingViewModel(_module);

            Target.WhenPropertyChanged(x => x.IsBusy)
                .Skip(1)
                .Subscribe(b => Target.BlurBackgroundParams = b ? _blurredParams : _defaultParams)
                .DisposeWith(this);
        }
    }
}