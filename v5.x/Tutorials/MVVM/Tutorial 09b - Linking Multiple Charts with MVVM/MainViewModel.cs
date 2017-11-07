using System.Collections.ObjectModel;
using SciChart.Data.Model;

namespace SciChart.Mvvm.Tutorial
{
    public class MainViewModel : BindableObject
    {         
        private bool _enablePan;
        private bool _enableZoom = true;
        private readonly ObservableCollection<ChartViewModel> _chartViewModels = new ObservableCollection<ChartViewModel>();

        public MainViewModel()
        {
            ChartPanes.Add(new ChartViewModel(new DummyDataProvider(), "Primary Chart"));
            ChartPanes.Add(new ChartViewModel(new DummyDataProvider(), "Secondary Chart"));
        }   
        
        public ObservableCollection<ChartViewModel> ChartPanes {  get { return _chartViewModels; } }
        
        public bool EnableZoom
        {
            get { return _enableZoom; }
            set
            {
                if (_enableZoom != value)
                {
                    _enableZoom = value;
                    OnPropertyChanged("EnableZoom");
                    if (_enableZoom) EnablePan = false;
                }
            }
        }
        
        public bool EnablePan
        {
            get { return _enablePan; }
            set
            {
                if (_enablePan != value)
                {
                    _enablePan = value;
                    OnPropertyChanged("EnablePan");
                    if (_enablePan) EnableZoom = false;
                }
            }
        }

    }
}