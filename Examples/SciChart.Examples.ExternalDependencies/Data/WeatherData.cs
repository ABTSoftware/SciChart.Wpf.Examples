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
