// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
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
using SciChart.Charting.Common.Helpers;
using SciChart.Drawing.DirectX.Context.D3D11;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class FeaturesHelper : INotifyPropertyChanged
    {
        private readonly PowerManager _powerManager = new PowerManager();
        private static readonly FeaturesHelper _instance = new FeaturesHelper();
        private DispatcherTimer _timer;
        private PowerPlan _lastPlan;

        private FeaturesHelper()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                var currentPlan = _powerManager.GetCurrentPlan();
                if (_lastPlan != currentPlan)
                {
                    OnPropertyChanged("PowerSavingHighPerformance");
                    OnPropertyChanged("PowerSavingBalanced");
                    OnPropertyChanged("PowerSavingUnknown");
                    OnPropertyChanged("PowerSavingPowerSaver");

                    _lastPlan = currentPlan;
                }                
            };
            _timer.Start();
        }

        public static FeaturesHelper Instance { get { return _instance; } }

        public bool SupportsHardwareAcceleration
        {
            get
            {
#if SILVERLIGHT
                return false;
#else
                return Direct3D11CompatibilityHelper.SupportsDirectX10;
#endif

            }
        }

        public string CurrentPowerPlanName
        {
            get { return _powerManager.GetCurrentPlan().Name; }
        }

        public bool PowerSavingHighPerformance
        {
            get { return _powerManager.GetCurrentPlan().Guid == _powerManager.MaximumPerformance.Guid; }
        }

        public bool PowerSavingBalanced
        {
            get { return _powerManager.GetCurrentPlan().Guid == _powerManager.Balanced.Guid; }
        }

        public bool PowerSavingUnknown
        {
            get { return !PowerSavingHighPerformance && !PowerSavingBalanced && !PowerSavingPowerSaver; }
        }

        public bool PowerSavingPowerSaver
        {
            get { return _powerManager.GetCurrentPlan().Guid == _powerManager.PowerSourceOptimized.Guid; }
        }

        public ICommand OpenControlPanelCommand { get {  return new ActionCommand(() => _powerManager.OpenControlPanel());} }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
