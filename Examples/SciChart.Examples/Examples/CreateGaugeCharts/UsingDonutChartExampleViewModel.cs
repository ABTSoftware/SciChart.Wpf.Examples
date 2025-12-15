// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
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
        // For managing 'Add New Segment'
        private BrushViewModel _newSegmentBrush;
        private string _newSegmentText = "New";
        private string _newSegmentValue = "10";

        private readonly ObservableCollection<IPieSegmentViewModel> _donutSegmentViewModels;
        private readonly List<IPieSegmentViewModel> _selectedViewModels = new List<IPieSegmentViewModel>();

        private readonly Color _colorForSegment1 = Color.FromArgb(0xFF, 0x83, 0x98, 0xBA);
        private readonly Color _colorForSegment2 = Color.FromArgb(0xFF, 0x27, 0x4B, 0x92);
        private readonly Color _colorForSegment3 = Color.FromArgb(0xFF, 0x27, 0x4B, 0x92);
        private readonly Color _colorForSegment4 = Color.FromArgb(0xFF, 0x47, 0x81, 0xED);
        private readonly Color _colorForSegment5 = Color.FromArgb(0xFF, 0xE2, 0x69, 0xAE);
        private readonly Color _colorForSegment6 = Color.FromArgb(0xFF, 0xE8, 0xC6, 0x67);

        public UsingDonutChartExampleViewModel()
        {
            AllBrushes = typeof(Brushes).GetProperties().Select(x => new BrushViewModel { BrushName = x.Name, Brush = (Brush)x.GetValue(null, null) }).ToList();                  
            NewSegmentBrush = AllBrushes.First(x => x.BrushName == "Aquamarine");

            _donutSegmentViewModels = new ObservableCollection<IPieSegmentViewModel>
            {
                new PieSegmentViewModel {Value = 75, Name = "Rent", Stroke = ToShade(_colorForSegment1, 0.8), Fill = ToGradient(_colorForSegment1), StrokeThickness = 2},
                new PieSegmentViewModel {Value = 19, Name = "Food", Stroke = ToShade(_colorForSegment2, 0.8), Fill = ToGradient(_colorForSegment2), StrokeThickness = 2},
                new PieSegmentViewModel {Value = 9, Name = "Utilities", Stroke = ToShade(_colorForSegment3, 0.8), Fill = ToGradient(_colorForSegment3), StrokeThickness = 2},
                new PieSegmentViewModel {Value = 9, Name = "Fun", Stroke = ToShade(_colorForSegment4, 0.8), Fill = ToGradient(_colorForSegment4), StrokeThickness = 2},
                new PieSegmentViewModel {Value = 10, Name = "Clothes", Stroke = ToShade(_colorForSegment5, 0.8), Fill = ToGradient(_colorForSegment5), StrokeThickness = 2},
                new PieSegmentViewModel {Value = 5, Name = "Phone", Stroke = ToShade(_colorForSegment6, 0.8), Fill = ToGradient(_colorForSegment6), StrokeThickness = 2},
            };

            AddNewItemCommand = new ActionCommand(() =>
            {
                _donutSegmentViewModels.Add(new PieSegmentViewModel
                {
                    Value = NewSegmentValue.ToDouble(),
                    Name = NewSegmentText,
                    Fill = ToGradient(((SolidColorBrush) NewSegmentBrush.Brush).Color),
                    Stroke = ToShade(((SolidColorBrush) NewSegmentBrush.Brush).Color, 0.8)
                });

            }, () =>!NewSegmentText.IsNullOrEmpty() && !NewSegmentValue.IsNullOrEmpty() && NewSegmentValue.ToDouble() > 0 && NewSegmentBrush != null);

            SegmentSelectionCommand = new ActionCommand<NotifyCollectionChangedEventArgs>(OnSegmentSelectionExecute);
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
        public ObservableCollection<IPieSegmentViewModel> DonutSegmentViewModels => _donutSegmentViewModels;

        // For managing Addition of new segments
        public ActionCommand AddNewItemCommand { get; set; }

        public ActionCommand<NotifyCollectionChangedEventArgs> SegmentSelectionCommand { get; set; }
        
        // Populates combo box for choosing color of new item to add
        public List<BrushViewModel> AllBrushes { get; }

        public BrushViewModel NewSegmentBrush
        {
            get => _newSegmentBrush;
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
            get => _newSegmentValue;
            set
            {
                _newSegmentValue = value;
                OnPropertyChanged(nameof(NewSegmentValue));
                AddNewItemCommand?.RaiseCanExecuteChanged();
            }
        }

        private void OnSegmentSelectionExecute(NotifyCollectionChangedEventArgs e)
        {
            if (!e.NewItems.IsNullOrEmptyList() && e.NewItems[0] != null)
            {
                _selectedViewModels.Add((IPieSegmentViewModel)e.NewItems[0]);
            }

            if (!e.OldItems.IsNullOrEmptyList() && e.OldItems[0] != null)
            {
                _selectedViewModels.Remove((IPieSegmentViewModel)e.OldItems[0]);
            }

            SelectedSegment = _selectedViewModels?.LastOrDefault();
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
}
