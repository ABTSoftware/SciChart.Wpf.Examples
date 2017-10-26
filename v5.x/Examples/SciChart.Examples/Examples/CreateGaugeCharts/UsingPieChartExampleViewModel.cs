// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UsingPieChartExampleViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace SciChart.Examples.Examples.CreateGaugeCharts
{
    public class UsingPieChartExampleViewModel : BaseViewModel
    {
        private IPieSegmentViewModel _oneSelectedSegment;

        public UsingPieChartExampleViewModel()
        {
            _oneSelectedSegment = new PieSegmentViewModel {Value = 60, Name = "Fruit", IsSelected = true};

            SegmentsDataCollection = new ObservableCollection<IPieSegmentViewModel>
            {
                _oneSelectedSegment,
                new PieSegmentViewModel {Value = 46, Name = "Protein"},
                new PieSegmentViewModel {Value = 36, Name = "Vegetables"},
                new PieSegmentViewModel {Value = 30, Name = "Diary" },
                new PieSegmentViewModel {Value = 18, Name = "Grains" },
                new PieSegmentViewModel {Value = 10, Name = "Other" },
            };

            SelectedSegment = _oneSelectedSegment;

            AddNewItem = new ActionCommand(() =>
            {
                SegmentsDataCollection.Add(new PieSegmentViewModel
                {
                    Value = NewSegmentValue.ToDouble(),
                    Name = NewSegmentText,
                    Fill = ToGradient(((SolidColorBrush)NewSegmentBrush.Brush).Color),
                    Stroke = ToShade(((SolidColorBrush)NewSegmentBrush.Brush).Color, 0.8), 
                    IsSelected = true,
                });
            }, () =>
            {
                return !NewSegmentText.IsNullOrEmpty() && (!NewSegmentValue.IsNullOrEmpty() && NewSegmentValue.ToDouble() > 0) && NewSegmentBrush != null;
            });

            DeleteSegment = new ActionCommand(() => { SegmentsDataCollection.RemoveAt(0);});
            SegmentSelectionCommand = new ActionCommand<NotifyCollectionChangedEventArgs>(OnSegmentSelectionExecute);
        }

        private void OnSegmentSelectionExecute(NotifyCollectionChangedEventArgs e)
        {
            if (!e.NewItems.IsNullOrEmptyList() && e.NewItems[0] != null)
            {
                SelectedSegment = (IPieSegmentViewModel) e.NewItems[0];
            }
        }

        public ObservableCollection<IPieSegmentViewModel> SegmentsDataCollection { get; set; }

        // Populates combo box for choosing color of new item to add
        public List<PieBrushesModel> AllBrushes
        {
            get { return typeof (Brushes).GetProperties().Select(x => new PieBrushesModel {BrushName = x.Name, Brush = (Brush) x.GetValue(null, null)}).ToList(); }
        }

        // For managing 'Add New Segment'
        private PieBrushesModel _newSegmentBrush;
        private string _newSegmentText;
        private IPieSegmentViewModel _selectedSegment;
        private string _newSegmentValue;

        public PieBrushesModel NewSegmentBrush
        {
            get { return _newSegmentBrush; }
            set
            {
                _newSegmentBrush = value;
                AddNewItem.RaiseCanExecuteChanged();
            }
        }

        public IPieSegmentViewModel SelectedSegment
        {
            get { return _selectedSegment; }
            set
            {
                _selectedSegment = value;
                OnPropertyChanged("SelectedSegment");
            }
        }

        public string NewSegmentText
        {
            get { return _newSegmentText; }
            set
            {
                _newSegmentText = value;
                AddNewItem.RaiseCanExecuteChanged();
            }
        }

        public string NewSegmentValue
        {
            get { return _newSegmentValue; }
            set
            {
                _newSegmentValue = value;
                AddNewItem.RaiseCanExecuteChanged();
            }
        }
        public ActionCommand AddNewItem { get; set; }

        public ActionCommand DeleteSegment { get; set; }
        public ActionCommand<NotifyCollectionChangedEventArgs> SegmentSelectionCommand { get; set; }

        // Helper functions to create nice brushes out of colors
        private Brush ToGradient(Color baseColor)
        {
            return new LinearGradientBrush(new GradientStopCollection()
            {
                new GradientStop(baseColor, 0.0),
                new GradientStop(ToShade(baseColor, 0.7).Color, 1.0),
            });
        }

        private SolidColorBrush ToShade(Color baseColor, double shade)
        {
            return new SolidColorBrush(Color.FromArgb(baseColor.A, (byte)(baseColor.R * shade), (byte)(baseColor.G * shade), (byte)(baseColor.B * shade)));
        }  
    }

    public class PieBrushesModel
    {
        public Brush Brush { get; set; }
        public string BrushName { get; set; }
    }

    public class PieSegmentViewModel : BaseViewModel, IPieSegmentViewModel
    {

        public double _Value;
        public double _Percentage;
        public bool _IsSelected;
        public string _Name;
        public Brush _Fill;
        public Brush _Stroke;

        public double Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                OnPropertyChanged("Value");
            }
        }

        public double Percentage
        {
            get
            {
                return _Percentage;
            }
            set
            {
                _Percentage = value;
                OnPropertyChanged("Percentage");
            }
        }
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }
        public Brush Fill
        {
            get
            {
                return _Fill;
            }
            set
            {
                _Fill = value;
                OnPropertyChanged("Fill");
            }
        }
        public Brush Stroke
        {
            get
            {
                return _Stroke;
            }
            set
            {
                _Stroke = value;
                OnPropertyChanged("Stroke");
            }
        }
    }
}
