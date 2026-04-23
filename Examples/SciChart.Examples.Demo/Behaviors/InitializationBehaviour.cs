// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// InitializationBehaviour.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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