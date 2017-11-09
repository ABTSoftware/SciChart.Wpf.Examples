using System;
using System.Reactive.Linq;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Reactive.Traits;

namespace SciChart.Examples.Demo.ViewModels
{
    public class AllowFeedbackSettingBehaviour : ViewModelTrait<SettingsViewModel>
    {
        private readonly ISyncUsageHelper _usageHelper;

        public AllowFeedbackSettingBehaviour(SettingsViewModel target, ISyncUsageHelper usageHelper) : base(target)
        {
            _usageHelper = usageHelper;

            target.WhenPropertyChanged(x => x.AllowFeedback)
                .Skip(1)
                .Subscribe(allowFeedback =>
                {
                    usageHelper.Enabled = allowFeedback;
                }).DisposeWith(this);

            _usageHelper.EnabledChanged += (s, e) =>
            {
                target.AllowFeedback = _usageHelper.Enabled;
            };
        }
    }
}