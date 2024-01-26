// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FeaturesHelper.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using SciChart.Charting;
using SciChart.Charting.Common.Helpers;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class FeaturesHelper : INotifyPropertyChanged
    {
        private readonly PowerManager _powerManager = new PowerManager();

        private PowerPlan _lastPlan;

        private FeaturesHelper()
        {
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};

            timer.Tick += (s, e) =>
            {
                var currentPlan = _powerManager.GetCurrentPlan();

                if (_lastPlan == null || !_lastPlan.Equals(currentPlan))
                {
                    OnPropertyChanged(nameof(PowerSavingHighPerformance));
                    OnPropertyChanged(nameof(PowerSavingBalanced));
                    OnPropertyChanged(nameof(PowerSavingUnknown));
                    OnPropertyChanged(nameof(PowerSavingPowerSaver));

                    _lastPlan = currentPlan;
                }                
            };

            timer.Start();
        }

        public static FeaturesHelper Instance { get; } = new FeaturesHelper();

        public string CurrentPowerPlanName => _powerManager.GetCurrentPlan().Name;

        public bool SupportsHardwareAcceleration => VisualXcceleratorEngine.SupportsHardwareAcceleration;

        public bool PowerSavingHighPerformance => _powerManager.GetCurrentPlan().Guid == _powerManager.MaximumPerformance.Guid;

        public bool PowerSavingBalanced => _powerManager.GetCurrentPlan().Guid == _powerManager.Balanced.Guid;

        public bool PowerSavingUnknown => !PowerSavingHighPerformance && !PowerSavingBalanced && !PowerSavingPowerSaver;

        public bool PowerSavingPowerSaver => _powerManager.GetCurrentPlan().Guid == _powerManager.PowerSourceOptimized.Guid;

        public ICommand OpenControlPanelCommand { get { return new ActionCommand(() => _powerManager.OpenControlPanel()); }}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}