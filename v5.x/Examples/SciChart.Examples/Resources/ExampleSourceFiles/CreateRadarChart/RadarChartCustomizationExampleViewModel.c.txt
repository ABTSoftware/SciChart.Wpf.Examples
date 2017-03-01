using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Utility.Mouse;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateRadarChart
{
    public class RadarChartCustomizationExampleViewModel : BaseViewModel
    {
        private bool _showTooltip;
        private bool _showLegend;
        private double _startAngle;
        private bool _isFilled;
        private bool _allowSelection;
        private bool _allowMultiSelection;
        private MouseModifier _selectionExecuteWhen;
        
        public RadarChartCustomizationExampleViewModel()
        {
            IsFilled = true;

            UkraineDataSeries = new ObservableCollection<IRadarPointViewModel>
            {
                new RadarPointViewModel {DataValue = 4.05, AxisId = "gracePeriodId"},
                new RadarPointViewModel {DataValue = 51.35, AxisId = "grantElementId"},
                new RadarPointViewModel {DataValue = 1.17, AxisId = "interestId"},
                new RadarPointViewModel {DataValue = 12.02, AxisId = "maturityId"},
                new RadarPointViewModel {DataValue = 7.9, AxisId = "netReservesId"},
                new RadarPointViewModel {DataValue = 2.05, AxisId =  "concessionalDebtId"},
                new RadarPointViewModel {DataValue = 1.4, AxisId =  "aidId"},
            };

            RomaniaDataSeries = new ObservableCollection<IRadarPointViewModel>
            {
                new RadarPointViewModel {DataValue = 14, AxisId = "gracePeriodId"},
                new RadarPointViewModel {DataValue = 45.99, AxisId = "grantElementId"},
                new RadarPointViewModel {DataValue = 3.34, AxisId = "interestId"},
                new RadarPointViewModel {DataValue = 14.33, AxisId = "maturityId"},
                new RadarPointViewModel {DataValue = 8.9, AxisId = "netReservesId"},
                new RadarPointViewModel {DataValue = 7.14, AxisId =  "concessionalDebtId"},
                new RadarPointViewModel {DataValue = 1.1, AxisId =  "aidId"},
            };

            GeorgiaDataSeries = new ObservableCollection<IRadarPointViewModel>
            {
                new RadarPointViewModel {DataValue = 7.5, AxisId = "gracePeriodId"},
                new RadarPointViewModel {DataValue = 66.91, AxisId = "grantElementId"},
                new RadarPointViewModel {DataValue = 0.9, AxisId = "interestId"},
                new RadarPointViewModel {DataValue = 23.6, AxisId = "maturityId"},
                new RadarPointViewModel {DataValue = 3.2, AxisId = "netReservesId"},
                new RadarPointViewModel {DataValue = 17.78, AxisId =  "concessionalDebtId"},
                new RadarPointViewModel {DataValue = 0.56, AxisId =  "aidId"},
            };

            BelarusDataSeries = new ObservableCollection<IRadarPointViewModel>
            {
                new RadarPointViewModel {DataValue = 3.82, AxisId = "gracePeriodId"},
                new RadarPointViewModel {DataValue = 25.09, AxisId = "grantElementId"},
                new RadarPointViewModel {DataValue = 4.83, AxisId = "interestId"},
                new RadarPointViewModel {DataValue = 11.35, AxisId = "maturityId"},
                new RadarPointViewModel {DataValue = 5.4, AxisId = "netReservesId"},
                new RadarPointViewModel {DataValue = 20.61, AxisId =  "concessionalDebtId"},
                new RadarPointViewModel {DataValue = 0.11, AxisId =  "aidId"},
            };                                                                 
        }

        public ObservableCollection<IRadarPointViewModel> UkraineDataSeries { get; set; }

        public ObservableCollection<IRadarPointViewModel> RomaniaDataSeries { get; set; }

        public ObservableCollection<IRadarPointViewModel> GeorgiaDataSeries { get; set; }

        public ObservableCollection<IRadarPointViewModel> BelarusDataSeries { get; set; }
        
        public MouseModifier SelectionExecuteWhen
        {
            get { return _selectionExecuteWhen; }
            set
            {
                _selectionExecuteWhen = value;
                OnPropertyChanged("SelectionExecuteWhen");
            }
        }

        public bool AllowSelection
        {
            get { return _allowSelection; }
            set
            {
                _allowSelection = value;
                OnPropertyChanged("AllowSelection");
            }
        }

        public bool AllowMultiSelection
        {
            get { return _allowMultiSelection; }
            set
            {
                _allowMultiSelection = value;
                OnPropertyChanged("AllowMultiSelection");
            }
        }

        public bool IsFilled
        {
            get { return _isFilled; }
            set
            {
                _isFilled = value;
                OnPropertyChanged("IsFilled");
            }
        }

        public bool ShowTooltip
        {
            get { return _showTooltip; }
            set
            {
                _showTooltip = value;
                OnPropertyChanged("ShowTooltip");
            }
        }

        public bool ShowLegend
        {
            get { return _showLegend; }
            set
            {
                _showLegend = value;
                OnPropertyChanged("ShowLegend");
            }
        }

        public double StartAngle
        {
            get { return _startAngle; }
            set
            {
                _startAngle = value;
                OnPropertyChanged("StartAngle");
            }
        }
    }
}
