// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VitalSignsIndicatorsProvider.cs is part of the SCICHART® Examples. Permission is hereby
// granted to modify, create derivative works, distribute and publish any part of this
// source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.VitalSignsMonitor
{
    public class VitalSignsIndicatorsProvider
    {
        private readonly Random _random = new Random();

        private static readonly string[] BpmValues = { "67", "69", "72", "74" };
        private static readonly string[] BpValues = { "120/70", "115/70", "115/75", "120/80" };
        private static readonly string[] BvValues = { "13.1", "13.2", "13.3", "13.0" };

        private static readonly int[] BpbValues = { 50, 75, 100 };
        private static readonly int[] BvbValues = { 75, 90, 100 };
        private static readonly string[] BoValues = { "93", "95", "96", "97" };

        public string BpmValue { get; private set; } = BpmValues[0];
        public string BpValue { get; private set; } = BpValues[0];
        public string BvValue { get; private set; } = BvValues[0];

        public int BpbValue { get; private set; } = BpbValues[0];
        public int BvBar1Value { get; private set; } = BvbValues[0];
        public int BvBar2Value { get; private set; } = BvbValues[0];

        public string SpoValue { get; private set; } = BoValues[0];
        public string SpoClockValue { get; private set; } = GetTimeString();

        public void Update()
        {
            BpmValue = RandomString(BpmValues);

            BpValue = RandomString(BpValues);
            BpbValue = RandomInt(BpbValues);

            BvValue = RandomString(BvValues);
            BvBar1Value = RandomInt(BvbValues);
            BvBar2Value = RandomInt(BvbValues);

            SpoValue = RandomString(BoValues);
            SpoClockValue = GetTimeString();
        }

        private string RandomString(IReadOnlyList<string> values)
        {
            return values[_random.Next(values.Count)];
        }

        private int RandomInt(IReadOnlyList<int> values)
        {
            return values[_random.Next(values.Count)];
        }

        private static string GetTimeString()
        {
            return DateTime.Now.ToString("HH:mm");
        }
    }
}