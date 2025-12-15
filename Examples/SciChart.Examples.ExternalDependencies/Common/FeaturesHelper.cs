// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
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
using System.Windows.Input;
using System.Windows.Threading;
using SciChart.Charting;
using SciChart.Charting.Common.Helpers;
using SciChart.Data.Model;

namespace SciChart.Examples.ExternalDependencies.Common
{
    /// <summary>
    /// Provides power plan and hardware acceleration status tracking.
    /// </summary>
    public class FeaturesHelper : BindableObject
    {
        private readonly PowerManager _powerManager = new PowerManager();

        private PowerPlan _lastPlan;

        private FeaturesHelper()
        {
            OpenControlPanelCommand = new ActionCommand(_powerManager.OpenControlPanel);

            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};

            timer.Tick += (s, e) =>
            {
                var currentPlan = _powerManager.GetCurrentPlan();

                if (_lastPlan == null || !_lastPlan.Equals(currentPlan))
                {
                    OnPropertyChanged(nameof(CurrentPowerPlanName));
                    OnPropertyChanged(nameof(PowerSavingHighPerformance));
                    OnPropertyChanged(nameof(PowerSavingBalanced));
                    OnPropertyChanged(nameof(PowerSavingUnknown));
                    OnPropertyChanged(nameof(PowerSavingPowerSaver));

                    _lastPlan = currentPlan;
                }                
            };

            timer.Start();
        }

        /// <summary>
        /// Gets the singleton instance of the FeaturesHelper class.
        /// </summary>
        public static FeaturesHelper Instance { get; } = new FeaturesHelper();

        /// <summary>
        /// Gets the name of the currently active power plan.
        /// </summary>
        public string CurrentPowerPlanName => _powerManager.GetCurrentPlan().Name;

        /// <summary>
        /// Indicates whether the GPU supports hardware acceleration.
        /// </summary>
        public bool SupportsHardwareAcceleration => VisualXcceleratorEngine.SupportsHardwareAcceleration;

        /// <summary>
        /// Indicates whether the system is using the 'High Performance' power plan.
        /// </summary>
        public bool PowerSavingHighPerformance => _powerManager.GetCurrentPlan().Guid == _powerManager.MaximumPerformance.Guid;

        /// <summary>
        /// Indicates whether the system is using the 'Balanced' power plan.
        /// </summary>
        public bool PowerSavingBalanced => _powerManager.GetCurrentPlan().Guid == _powerManager.Balanced.Guid;

        /// <summary>
        /// Indicates whether the system is using the 'Power Saver' power plan.
        /// </summary>
        public bool PowerSavingPowerSaver => _powerManager.GetCurrentPlan().Guid == _powerManager.PowerSourceOptimized.Guid;

        /// <summary>
        /// Indicates whether the current power plan does not match any known predefined plans.
        /// </summary>
        public bool PowerSavingUnknown => !PowerSavingHighPerformance && !PowerSavingBalanced && !PowerSavingPowerSaver;

        /// <summary>
        /// Gets the command to open the power options in control panel.
        /// </summary>
        public ICommand OpenControlPanelCommand { get; }
    }
}