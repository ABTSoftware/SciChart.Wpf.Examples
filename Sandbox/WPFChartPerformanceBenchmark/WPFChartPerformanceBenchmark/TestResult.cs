using System.Collections.Generic;
using System.ComponentModel;

namespace WPFChartPerformanceBenchmark
{
    /// <summary>
    /// A simple value-object for storing test results
    /// </summary>
    public class TestResult : INotifyPropertyChanged
    {
        private Dictionary<string, double> _results = new Dictionary<string, double>();

        public string TestName { get; set; }

        public double this[string index]
        {
            get
            {
                return _results.ContainsKey(index) ? _results[index] : 0.0;
            }
            set
            {
                _results[index] = value;
                OnPropertyChanged(null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}