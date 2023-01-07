using SciChart.Charting.Model.ChartSeries;

namespace ScrollbarMvvmAxisExample
{
    public class NumericAxisViewModelWithScrollbar : NumericAxisViewModel
    {
        private bool _hasScrollbar;

        public bool HasScrollbar
        {
            get { return _hasScrollbar; }
            set
            {
                _hasScrollbar = value;
                OnPropertyChanged("HasScrollbar");
            }
        }
    }
}