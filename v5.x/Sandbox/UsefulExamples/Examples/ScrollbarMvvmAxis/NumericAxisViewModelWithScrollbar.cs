using SciChart.Charting.Model.ChartSeries;

namespace SciChart.Sandbox.Examples.ScrollbarMvvmAxis
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