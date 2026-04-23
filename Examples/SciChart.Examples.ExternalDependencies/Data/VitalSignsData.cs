// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// VitalSignsData.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
namespace SciChart.Examples.ExternalDependencies.Data
{
    public class VitalSignsData
    {
        public bool IsATrace { get; set; }
        public double XValue { get; set; }
        public double ECGHeartRate { get; set; }
        public double BloodPressure { get; set; }
        public double BloodVolume { get; set; }
        public double BloodOxygenation { get; set; }
    }
}