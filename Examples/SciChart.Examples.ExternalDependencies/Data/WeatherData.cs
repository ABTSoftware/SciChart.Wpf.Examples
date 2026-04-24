// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// WeatherData.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public enum WindDirection { N, NE, E, SE, S, SW, W, NW }

    public class WeatherData
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public double MinTemp { get; set; }

        public double MaxTemp { get; set; }

        public double Rainfall { get; set; }

        public double Sunshine { get; set; }

        public int UVIndex { get; set; }

        public int WindSpeed { get; set; }

        public WindDirection WindDirection { get; set; }

        public string Forecast { get; set; }

        public bool LocalStation { get; set; }
    }
}
