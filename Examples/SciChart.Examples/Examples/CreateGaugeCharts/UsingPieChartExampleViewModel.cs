// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
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
        private BrushViewModel _newSegmentBrush;
        private string _newSegmentText = "New";
        private string _newSegmentValue = "10";

        private IPieSegmentViewModel _selectedSegment;
        private readonly List<IPieSegmentViewModel> _selectedModels = new List<IPieSegmentViewModel>();

        public UsingPieChartExampleViewModel()
        {
            AllBrushes = typeof(Brushes).GetProperties().Select(x => new BrushViewModel { BrushName = x.Name, Brush = (Brush)x.GetValue(null, null) }).ToList();
            NewSegmentBrush = AllBrushes.First(x => x.BrushName == "Aquamarine");

            PieSegmentViewModels = new ObservableCollection<IPieSegmentViewModel>
            {
                new PieSegmentViewModel {Value = 60, Name = "Fruit"},
                new PieSegmentViewModel {Value = 46, Name = "Protein"},
                new PieSegmentViewModel {Value = 36, Name = "Vegetables"},
                new PieSegmentViewModel {Value = 30, Name = "Diary"},
                new PieSegmentViewModel {Value = 18, Name = "Grains"},
                new PieSegmentViewModel {Value = 10, Name = "Other"}
            };

            AddNewItemCommand = new ActionCommand(() =>
            {
                PieSegmentViewModels.Add(new PieSegmentViewModel
                {
                    Value = NewSegmentValue.ToDouble(),
                    Name = NewSegmentText,
                    Fill = ToGradient(((SolidColorBrush)NewSegmentBrush.Brush).Color),
                    Stroke = ToShade(((SolidColorBrush)NewSegmentBrush.Brush).Color, 0.8),
                    IsSelected = true,
                });

            }, () => !NewSegmentText.IsNullOrEmpty() && !NewSegmentValue.IsNullOrEmpty() && NewSegmentValue.ToDouble() > 0 && NewSegmentBrush != null);

            DeleteSegment = new ActionCommand(() => PieSegmentViewModels.RemoveAt(0));
            SegmentSelectionCommand = new ActionCommand<NotifyCollectionChangedEventArgs>(OnSegmentSelectionExecute);
        }

        public ObservableCollection<IPieSegmentViewModel> PieSegmentViewModels { get; set; }

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

        public IPieSegmentViewModel SelectedSegment
        {
            get => _selectedSegment;
            set
            {
                _selectedSegment = value;
                OnPropertyChanged(nameof(SelectedSegment));
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public bool IsSelected => SelectedSegment != null;

        public string NewSegmentText
        {
            get => _newSegmentText;
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
        public ActionCommand AddNewItemCommand { get; set; }

        public ActionCommand DeleteSegment { get; set; }

        public ActionCommand<NotifyCollectionChangedEventArgs> SegmentSelectionCommand { get; set; }

        private void OnSegmentSelectionExecute(NotifyCollectionChangedEventArgs e)
        {
            if (!e.NewItems.IsNullOrEmptyList() && e.NewItems[0] != null)
            {
                _selectedModels.Add((IPieSegmentViewModel)e.NewItems[0]);
            }

            if (!e.OldItems.IsNullOrEmptyList() && e.OldItems[0] != null)
            {
                _selectedModels.Remove((IPieSegmentViewModel)e.OldItems[0]);
            }

            SelectedSegment = _selectedModels?.LastOrDefault();
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
