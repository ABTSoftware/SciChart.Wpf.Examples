// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
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

        public UsingDonutChartExampleViewModel()
        {            
            _donutModels = new ObservableCollection<IPieSegmentViewModel>
            {
                new DonutSegmentViewModel {Value = 48, Name = "Rent", Stroke = ToShade(Colors.Orange, 0.8), Fill = ToGradient(Colors.Orange), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 19, Name = "Food", Stroke = ToShade(Colors.Green, 0.8), Fill = ToGradient(Colors.Green), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 9, Name = "Utilities", Stroke = ToShade(Colors.DodgerBlue, 0.8), Fill = ToGradient(Colors.DodgerBlue), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 9, Name = "Fun", Stroke = ToShade(Colors.Gray, 0.8), Fill = ToGradient(Colors.Gray), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 10, Name = "Clothes", Stroke = ToShade(Colors.Firebrick, 0.8), Fill = ToGradient(Colors.Firebrick), StrokeThickness = 2},
                new DonutSegmentViewModel {Value = 5, Name = "Phone", Stroke = ToShade(Colors.DarkSalmon, 0.8), Fill = ToGradient(Colors.DarkSalmon), StrokeThickness = 2}
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
            }, () =>
            {
                return !NewSegmentText.IsNullOrEmpty() && (!NewSegmentValue.IsNullOrEmpty() && NewSegmentValue.ToDouble() > 0) && NewSegmentBrush != null ;
            });

            SegmentSelectionCommand = new ActionCommand<NotifyCollectionChangedEventArgs>(OnSegmentSelectionExecute);
        }

        private void OnSegmentSelectionExecute(NotifyCollectionChangedEventArgs e)
        {
            if (!e.NewItems.IsNullOrEmptyList())
            {
                SelectedSegment = (IPieSegmentViewModel)e.NewItems[0];
            }
        }

        private IPieSegmentViewModel _selectedSegment;

        public IPieSegmentViewModel SelectedSegment
        {
            get { return _selectedSegment; }
            set
            {
                _selectedSegment = value;
                OnPropertyChanged("SelectedSegment");
            }
        }
        // Binds to ItemsSource of Donut Chart
        public ObservableCollection<IPieSegmentViewModel> DonutModels { get { return _donutModels; } }

        // For managing Addition of new segments
        public ActionCommand AddNewItemCommand { get; set; }

        public ActionCommand<NotifyCollectionChangedEventArgs> SegmentSelectionCommand { get; set; }
        
        // Populates combo box for choosing color of new item to add
        public List<DonutBrushesModel> AllBrushes
        {
            get { return typeof(Brushes).GetProperties().Select(x => new DonutBrushesModel { BrushName = x.Name, Brush = (Brush)x.GetValue(null, null) }).ToList(); }
        }

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
                AddNewItemCommand.RaiseCanExecuteChanged();
            }
        }

        public string NewSegmentText
        {
            get { return _newSegmentText; }
            set 
            {
                _newSegmentText = value;
                AddNewItemCommand.RaiseCanExecuteChanged();
            }
        }

        public string NewSegmentValue
        {
            get { return _newSegmentValue; }
            set
            {
                _newSegmentValue = value;
                AddNewItemCommand.RaiseCanExecuteChanged();
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
