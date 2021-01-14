// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DeveloperModManager.cs is part of SCICHART®, High Performance Scientific Charts
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
