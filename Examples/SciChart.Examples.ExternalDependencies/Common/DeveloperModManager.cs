// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// DeveloperModManager.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class DeveloperModManager : BaseViewModel
    {
        private bool _isDevMode;

        public bool IsDeveloperMode
        {
            get { return _isDevMode; }
            set
            {
                _isDevMode = value;
                OnPropertyChanged("IsDeveloperMode");
            }
        }

        public static DeveloperModManager Manage { get; private set; }

        static DeveloperModManager()
        {
            Manage = new DeveloperModManager();
        }

        private DeveloperModManager()
        {
        }
    }
}
