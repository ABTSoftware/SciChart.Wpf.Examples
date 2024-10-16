using System.Collections.Generic;
using System.ComponentModel;

namespace ChartProviders.Common.Models
{
    public class TestResult : INotifyPropertyChanged
    {
        private readonly Dictionary<string, double> _results = new Dictionary<string, double>();

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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}