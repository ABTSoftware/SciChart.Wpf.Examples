using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace State_Series_Example
{
    public enum State
    {
        Normal = 1,
        Warning = 2,
        Error = 3,
    }

    public class MainWindowViewModel : BindableObject
    {
        private XyDataSeries<double> _lineSeriesData;
        private XyDataSeries<double> _scatterSeriesData;
        private readonly Random _random = new Random();

        public MainWindowViewModel()
        {
            _lineSeriesData = new XyDataSeries<double>();
            _scatterSeriesData = new XyDataSeries<double>();

            for (int i = 0; i < 100; i++)
            {
                // put a sinewave into the line series data
                _lineSeriesData.Append(i, Math.Sin(i*0.1));

                // put random state values (converted to int) into the scatter data
                _scatterSeriesData.Append(i, (int)GetRandomState());
            }
        }

        private State GetRandomState()
        {
            // return a random state for the state scatter series
            State randomState = (State)_random.Next(1, 4);
            return randomState;
        }

        public XyDataSeries<double> LineSeriesData => _lineSeriesData;

        public XyDataSeries<double> ScatterSeriesData => _scatterSeriesData;
    }
}
