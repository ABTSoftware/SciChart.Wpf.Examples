// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// UniformDataManager.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public static class UniformDataManager
    {
        public static void GenerateAnalogData(float[] buffer)
        {
            var count = buffer.Length;
            var freq = count / 100;

            var amp = 1d;
            var phase = 0d;

            for (int i = 0, j = 0; i < count; i++, j++)
            {
                var wn = 2 * Math.PI / (count / (double)freq);
                buffer[i] = (float)(amp * Math.Sin(j * wn + phase));
            }
        }
    }
}