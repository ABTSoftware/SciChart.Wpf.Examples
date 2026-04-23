// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// IPowerManager.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Collections.Generic;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public interface IPowerManager
    {
        /// <returns>
        /// All supported power plans.
        /// </returns>
        List<PowerPlan> GetPlans();

        PowerPlan GetCurrentPlan();

        void SetActive(PowerPlan plan);

        /// <summary>
        /// Opens Power Options section of the Control Panel.
        /// </summary>
        void OpenControlPanel();
    }
}