// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VitalSignsBatch.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Linq;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.VitalSignsMonitor
{
    public class VitalSignsBatch
    {
        public VitalSignsBatch()
        {
            XValues = new List<double>();

            ECGHeartRateValuesA = new List<double>();
            BloodPressureValuesA = new List<double>();
            BloodVolumeValuesA = new List<double>();
            BloodOxygenationValuesA = new List<double>();

            ECGHeartRateValuesB = new List<double>();
            BloodPressureValuesB = new List<double>();
            BloodVolumeValuesB = new List<double>();
            BloodOxygenationValuesB = new List<double>();
        }

        public List<double> XValues { get; }

        public List<double> ECGHeartRateValuesA { get; }
        public List<double> BloodPressureValuesA { get; }
        public List<double> BloodVolumeValuesA { get; }
        public List<double> BloodOxygenationValuesA { get; }

        public List<double> ECGHeartRateValuesB { get; set; }
        public List<double> BloodPressureValuesB { get; set; }
        public List<double> BloodVolumeValuesB { get; set; }
        public List<double> BloodOxygenationValuesB { get; set; }

        public VitalSignsData LastVitalSignsData { get; private set; }

        public void UpdateData(IList<VitalSignsData> dataList)
        {
            XValues.Clear();

            ECGHeartRateValuesA.Clear();
            BloodPressureValuesA.Clear();
            BloodVolumeValuesA.Clear();
            BloodOxygenationValuesA.Clear();

            ECGHeartRateValuesB.Clear();
            BloodPressureValuesB.Clear();
            BloodVolumeValuesB.Clear();
            BloodOxygenationValuesB.Clear();

            foreach (VitalSignsData data in dataList)
            {
                XValues.Add(data.XValue);

                if (data.IsATrace)
                {
                    ECGHeartRateValuesA.Add(data.ECGHeartRate);
                    BloodPressureValuesA.Add(data.BloodPressure);
                    BloodVolumeValuesA.Add(data.BloodVolume);
                    BloodOxygenationValuesA.Add(data.BloodOxygenation);

                    ECGHeartRateValuesB.Add(double.NaN);
                    BloodPressureValuesB.Add(double.NaN);
                    BloodVolumeValuesB.Add(double.NaN);
                    BloodOxygenationValuesB.Add(double.NaN);
                }
                else
                {
                    ECGHeartRateValuesB.Add(data.ECGHeartRate);
                    BloodPressureValuesB.Add(data.BloodPressure);
                    BloodVolumeValuesB.Add(data.BloodVolume);
                    BloodOxygenationValuesB.Add(data.BloodOxygenation);

                    ECGHeartRateValuesA.Add(double.NaN);
                    BloodPressureValuesA.Add(double.NaN);
                    BloodVolumeValuesA.Add(double.NaN);
                    BloodOxygenationValuesA.Add(double.NaN);
                }
            }

            LastVitalSignsData = dataList.Last();
        }
    }
}