// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ZoomHistoryMvvmViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.HistoryManagers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.ZoomHistory.ZoomHistoryMVVM
{
    public class ZoomHistoryMvvmViewModel : BaseViewModel
    {
        private static readonly AxisKey _xAxisKey = new AxisKey("XAxisId", true);
        private static readonly AxisKey _y0AxisKey = new AxisKey("Y0AxisId", false);
        private static readonly AxisKey _y1AxisKey = new AxisKey("Y1AxisId", false);
        private RangeHistorySameCompare _rangeHistoryCompare;
        private const int AxisAmount = 3;

        public class ChartRangeHistory : BaseViewModel
        {
            public ChartRangeHistory()
            {
            }

            public ChartRangeHistory(IDictionary<AxisKey, IRange> ranges)
            {
                foreach (var pair in ranges)
                {
                    if (pair.Key.Id == _xAxisKey.Id && pair.Key.IsXAxis == _xAxisKey.IsXAxis)
                    {
                        XAxisRange = pair.Value;
                    }
                    else if (pair.Key.Id == _y0AxisKey.Id && pair.Key.IsXAxis == _y0AxisKey.IsXAxis)
                    {
                        Y0AxisRange = pair.Value;
                    }
                    else if (pair.Key.Id == _y1AxisKey.Id && pair.Key.IsXAxis == _y1AxisKey.IsXAxis)
                    {
                        Y1AxisRange = pair.Value;
                    }
                }
            }

            public IRange XAxisRange { get; set; }
            public IRange Y0AxisRange { get; set; }
            public IRange Y1AxisRange { get; set; }
            public string ItemId { get; set; }

            public override bool Equals(object obj)
            {
                var other = obj as ChartRangeHistory;

                return other != null && Equals((ChartRangeHistory)obj);
            }

            public bool Equals(ChartRangeHistory other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;

                return other.XAxisRange.Equals(XAxisRange) &&
                       other.Y0AxisRange.Equals(Y0AxisRange) &&
                       other.Y1AxisRange.Equals(Y1AxisRange);
            }
        }

        public class RangeHistorySameCompare : EqualityComparer<ChartRangeHistory>
        {
            public override bool Equals(ChartRangeHistory x, ChartRangeHistory y)
            {
                if (x.ItemId == y.ItemId)
                {
                    return true;
                }

                return false;
            }

            public override int GetHashCode(ChartRangeHistory obj)
            {
                throw new NotImplementedException();
            }
        }

        private readonly Random _random = new Random(251916);
        private const int Count = 2000;

        private IZoomHistoryManager _zoomHistoryManager;
        private readonly ActionCommand _undoCommand;
        private readonly ActionCommand _redoCommand;
        private ObservableCollection<ChartRangeHistory> _rangesHistory = new ObservableCollection<ChartRangeHistory>();
        private ChartRangeHistory _selectedRange;
        private ObservableCollection<IRenderableSeriesViewModel> _renderableSeriesViewModels;

        public ZoomHistoryMvvmViewModel()
        {
            _rangeHistoryCompare = new RangeHistorySameCompare();

            var vm1 = new LineRenderableSeriesViewModel
            {
                DataSeries = FillData(new XyDataSeries<double, double>(), "firstDataSeries"),
                StyleKey = "FirstLineRenderableSeriesStyle",
                StrokeThickness = 2,
                Stroke = Color.FromArgb(0xff, 0x64, 0x95, 0xed),
            };

            var vm2 = new LineRenderableSeriesViewModel
            {
                DataSeries = FillData(new XyDataSeries<double, double>(), "secondDataSeries"),
                StyleKey = "SecondLineRenderableSeriesStyle",
                StrokeThickness = 2,
                Stroke = Color.FromArgb(0xff, 0xff, 0x45, 0x00),
            };

            RenderableSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel> { vm1, vm2 };

            _zoomHistoryManager = new ZoomHistoryManager();
            _zoomHistoryManager.RangeHistoryChanged += OnRangeHistoryChanged;

            _undoCommand = new ActionCommand(() =>
            {
                if (_zoomHistoryManager.CanUndo())
                {
                    _zoomHistoryManager.Undo();

                    var index = RangesHistory.IndexOf(_selectedRange, _rangeHistoryCompare);

                    _selectedRange = RangesHistory[index - 1];

                    OnPropertyChanged("SelectedRange");
                    RaiseUndoRedoCanExecute();
                }
            }, () => _zoomHistoryManager.CanUndo());

            _redoCommand = new ActionCommand(() =>
            {
                if (_zoomHistoryManager.CanRedo())
                {
                    _zoomHistoryManager.Redo();

                    var index = RangesHistory.IndexOf(_selectedRange, _rangeHistoryCompare);

                    _selectedRange = RangesHistory[index + 1];

                    OnPropertyChanged("SelectedRange");
                    RaiseUndoRedoCanExecute();
                }
            }, () => _zoomHistoryManager.CanRedo());
        }

        public IZoomHistoryManager ZoomHistoryManager
        {
            get { return _zoomHistoryManager; }
            set
            {
                _zoomHistoryManager = value;
                OnPropertyChanged("ZoomHistoryManager");
            }
        }

        public ObservableCollection<ChartRangeHistory> RangesHistory
        {
            get { return _rangesHistory; }
            set
            {
                _rangesHistory = value;
                OnPropertyChanged("RangesHistory");
            }
        }

        public ChartRangeHistory SelectedRange
        {
            get { return _selectedRange; }
            set
            {
                var currentIndex = _rangesHistory.IndexOf(_selectedRange, _rangeHistoryCompare);
                var newIndex = _rangesHistory.IndexOf(value, _rangeHistoryCompare);

                var times = currentIndex - newIndex;
                if (times > 0)
                {
                    for (int i = 0; i < times; i++)
                    {
                        _zoomHistoryManager.Undo();
                    }
                }
                else if (times < 0)
                {
                    for (int i = 0; i < Math.Abs(times); i++)
                    {
                        _zoomHistoryManager.Redo();
                    }
                }

                _selectedRange = value;
                OnPropertyChanged("SelectedRange");

                RaiseUndoRedoCanExecute();
            }
        }

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeriesViewModels
        {
            get { return _renderableSeriesViewModels; }
            set
            {
                _renderableSeriesViewModels = value;
                OnPropertyChanged("RenderableSeriesViewModels");
            }
        }

        public ICommand UndoCommand
        {
            get { return _undoCommand; }
        }

        public ICommand RedoCommand
        {
            get { return _redoCommand; }
        }

        private void OnRangeHistoryChanged(object sender, HistoryChangedEventArgs args)
        {
            if (args.OldRanges != null)
            {
                for (int i = 0; i < args.DisposedAmount; i++)
                {
                    RangesHistory.RemoveAt(RangesHistory.Count - 1);
                }
            }

            if (args.NewRanges != null && args.NewRanges.Count > 0)
            {
                var newRanges = args.NewRanges;

                if (newRanges.Count < AxisAmount)
                {
                    newRanges = FillRanges(newRanges);
                }

                var item = new ChartRangeHistory(newRanges) { ItemId = Guid.NewGuid().ToString() };
                RangesHistory.Add(item);
                while (RangesHistory.Count > ZoomHistoryManager.HistoryDepth)
                {
                    RangesHistory.RemoveAt(0);
                }
                OnPropertyChanged("RangesHistory");

                _selectedRange = item;
                OnPropertyChanged("SelectedRange");
            }

            RaiseUndoRedoCanExecute();
        }

        private IDictionary<AxisKey, IRange> FillRanges(IDictionary<AxisKey, IRange> ranges)
        {
            var dict = new Dictionary<AxisKey, IRange>(ranges);

            if (!ranges.ContainsKey(_xAxisKey))
            {
                dict.Add(_xAxisKey, RangesHistory[RangesHistory.Count - 1].XAxisRange);
            }
            if (!ranges.ContainsKey(_y0AxisKey))
            {
                dict.Add(_y0AxisKey, RangesHistory[RangesHistory.Count - 1].Y0AxisRange);
            }
            if (!ranges.ContainsKey(_y1AxisKey))
            {
                dict.Add(_y1AxisKey, RangesHistory[RangesHistory.Count - 1].Y1AxisRange);
            }

            return dict;
        }

        private void RaiseUndoRedoCanExecute()
        {
            _undoCommand.RaiseCanExecuteChanged();
            _redoCommand.RaiseCanExecuteChanged();
        }

        private IDataSeries FillData(IXyDataSeries<double, double> dataSeries, string name)
        {
            double randomWalk = 10.0;

            // Generate the X,Y data with sequential dates on the X-Axis and slightly positively biased random walk on the Y-Axis
            var xBuffer = new double[Count];
            var yBuffer = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                randomWalk += (_random.NextDouble() - 0.498);
                yBuffer[i] = randomWalk;
                xBuffer[i] = i;
            }

            // Buffer above and append all in one go to avoid multiple recalculations of series range
            dataSeries.Append(xBuffer, yBuffer);
            dataSeries.SeriesName = name;

            return dataSeries;
        }
    }
}
