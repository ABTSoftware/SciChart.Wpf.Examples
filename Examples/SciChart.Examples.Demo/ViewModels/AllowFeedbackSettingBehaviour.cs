// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// AllowFeedbackSettingBehaviour.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Reactive.Linq;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using SciChart.UI.Reactive;
using SciChart.UI.Reactive.Observability;
using SciChart.UI.Reactive.Traits;

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