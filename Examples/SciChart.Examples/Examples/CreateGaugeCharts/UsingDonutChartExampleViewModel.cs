// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UsingDonutChartExampleViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateGaugeCharts
{
    public class UsingDonutChartExampleViewModel : BaseViewModel
    {
        private readonly ObservableCollection<IPieSegmentViewModel> _donutModels;
        private readonly List<IPieSegmentViewModel> _selectedModels;

        private readonly Color _colorForSegment1 = Color.FromArgb(0xFF, 0x83, 0x98, 0xBA);
        private readonly Color _colorForSegment2 = Color.FromArgb(0xFF, 0x27, 0x4B, 0x92);
        private readonly Color _colorForSegment3 = Color.FromArgb(0xFF, 0x27, 0x4B, 0x92);
        private readonly Color _colorForSegment4 = Color.FromArgb(0xFF, 0x47, 0x81, 0xED);
        private readonly Color _colorForSegment5 = Color.FromArgb(0xFF, 0xE2, 0x69, 0xAE);
        private readonly Color _colorForSegment6 = Color.FromArgb(0xFF, 0xE8, 0xC6, 0x67);

        public UsingDonutChartExampleViewModel()
        {           
            NewSegmentText = "New";
            NewSegmentValue = "10";

            AllBrushes = typeof(Brushes).GetProperties().Select(x => new DonutBrushesModel { BrushName = x.Name, Brush = (Brush)x.GetValue(null, null) }).ToList();                  
            NewSegmentBrush = AllBrushes.First(x => x.BrushName == "Aquamarine");

            _selectedModels = new List<IPieSegmentViewModel>();
            _donutModels = new ObservableCollection<IPieSegmentViewModel>
            {
                new DonutSegmentViewModel {Value = 75, Name = "Rent", Stroke = ToShade(_colorForSegment1, 0.8), Fill = ToGradient(_colorForSegment1), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 19, Name = "Food", Stroke = ToShade(_colorForSegment2, 0.8), Fill = ToGradient(_colorForSegment2), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 9, Name = "Utilities", Stroke = ToShade(_colorForSegment3, 0.8), Fill = ToGradient(_colorForSegment3), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 9, Name = "Fun", Stroke = ToShade(_colorForSegment4, 0.8), Fill = ToGradient(_colorForSegment4), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 10, Name = "Clothes", Stroke = ToShade(_colorForSegment5, 0.8), Fill = ToGradient(_colorForSegment5), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 5, Name = "Phone", Stroke = ToShade(_colorForSegment6, 0.8), Fill = ToGradient(_colorForSegment6), StrokeThickness = 2},
            };

            AddNewItemCommand = new ActionCommand(() =>
            {
                _donutModels.Add(new DonutSegmentViewModel
                {
                    Value = NewSegmentValue.ToDouble(),
                    Name = NewSegmentText,
                    Fill = ToGradient(((SolidColorBrush) NewSegmentBrush.Brush).Color),
                    Stroke = ToShade(((SolidColorBrush) NewSegmentBrush.Brush).Color, 0.8)
                });

            }, () =>!NewSegmentText.IsNullOrEmpty() && !NewSegmentValue.IsNullOrEmpty() && NewSegmentValue.ToDouble() > 0 && NewSegmentBrush != null);

            SegmentSelectionCommand = new ActionCommand<NotifyCollectionChangedEventArgs>(OnSegmentSelectionExecute);
        }

        private void OnSegmentSelectionExecute(NotifyCollectionChangedEventArgs e)
        {
            if (!e.NewItems.IsNullOrEmptyList() && e.NewItems[0] != null)
            {
                _selectedModels.Add((IPieSegmentViewModel) e.NewItems[0]);
            }

            if (!e.OldItems.IsNullOrEmptyList() && e.OldItems[0] != null)
            {
                _selectedModels.Remove((IPieSegmentViewModel) e.OldItems[0]);
            }
            
            SelectedSegment = _selectedModels?.LastOrDefault();     
        }

        private IPieSegmentViewModel _selectedSegment;

        public IPieSegmentViewModel SelectedSegment
        {
            get { return _selectedSegment; }
            set
            {
                _selectedSegment = value;
                OnPropertyChanged(nameof(SelectedSegment));
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        
        public bool IsSelected => SelectedSegment != null;

        // Binds to ItemsSource of Donut Chart
        public ObservableCollection<IPieSegmentViewModel> DonutModels { get { return _donutModels; } }

        // For managing Addition of new segments
        public ActionCommand AddNewItemCommand { get; set; }

        public ActionCommand<NotifyCollectionChangedEventArgs> SegmentSelectionCommand { get; set; }
        
        // Populates combo box for choosing color of new item to add
        public List<DonutBrushesModel> AllBrushes { get; }

        // For managing 'Add New Segment'
        private DonutBrushesModel _newSegmentBrush;
        private string _newSegmentText;
        private string _newSegmentValue;

        public DonutBrushesModel NewSegmentBrush
        {
            get { return _newSegmentBrush; }
            set
            {
                _newSegmentBrush = value;
                OnPropertyChanged(nameof(NewSegmentBrush));
                AddNewItemCommand?.RaiseCanExecuteChanged();
            }
        }

        public string NewSegmentText
        {
            get { return _newSegmentText; }
            set 
            {
                _newSegmentText = value;
                OnPropertyChanged(nameof(NewSegmentText));
                AddNewItemCommand?.RaiseCanExecuteChanged();
            }
        }

        public string NewSegmentValue
        {
            get { return _newSegmentValue; }
            set
            {
                _newSegmentValue = value;
                OnPropertyChanged(nameof(NewSegmentValue));
                AddNewItemCommand?.RaiseCanExecuteChanged();
            }
        }

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

    public class DonutBrushesModel
    {
        public Brush Brush { get; set; }
        public string BrushName { get; set; }
    }

    public class DonutSegmentViewModel : BaseViewModel, IPieSegmentViewModel
    {
        public double _Value;
        public double _Percentage;
        public bool _IsSelected;
        public string _Name;
        public Brush _Fill;
        public Brush _Stroke;
        public double _strokeThickness;

        public double StrokeThickness
        {
            get
            {
                return _strokeThickness;
            }
            set
            {
                _strokeThickness = value;
                OnPropertyChanged("StrokeThickness");
            }
        }

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
