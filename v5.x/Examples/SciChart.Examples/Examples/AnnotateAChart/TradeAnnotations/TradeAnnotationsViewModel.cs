using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.DrawingTools.TradingAnnotations.ViewModels;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.AnnotateAChart.TradeAnnotations
{
    public class TradeAnnotationsViewModel : BaseViewModel
    {
        private DefaultViewportManager _viewportManager;
        private bool _isRubberBandEnabled;
        private ObservableCollection<IRenderableSeriesViewModel> _series;
        private ObservableCollection<IAnnotationViewModel> _annotations;
        private Type _annotationType;
        private bool _isAnnotationCreationEnable;
        private bool _isZoomPanEnabled;
        private bool _isEditPanelVisible;

        public TradeAnnotationsViewModel()
        {
            Annotations = new ObservableCollection<IAnnotationViewModel>();
            Annotations.CollectionChanged += OnAnnotationsCollectionChanged;

            Series = new ObservableCollection<IRenderableSeriesViewModel>
            {
                new CandlestickRenderableSeriesViewModel {DataSeries = GetPriceDataSeries()}
            };
        }

        public bool IsEditPanelVisible
        {
            get { return _isEditPanelVisible; }
            set
            {
                _isEditPanelVisible = value;
                OnPropertyChanged(nameof(IsEditPanelVisible));
            }
        }

        public IAnnotationViewModel SelectedAnnotation
        {
            get
            {
                return Annotations.FirstOrDefault(x => x.IsSelected);
            }
            set
            {
                var val = value;
            }
        }

        public bool IsRubberBandEnabled
        {
            get { return _isRubberBandEnabled; }
            set
            {
                _isRubberBandEnabled = value;
                OnPropertyChanged(nameof(IsRubberBandEnabled));
            }
        }

        public bool IsAnnotationDrawn { set; get; }
        
        public ObservableCollection<IRenderableSeriesViewModel> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged(nameof(Series));
            }
        }

        public ObservableCollection<IAnnotationViewModel> Annotations
        {
            get { return _annotations; }
            set
            {
                _annotations = value;
                OnPropertyChanged(nameof(Annotations));
            }
        }

        public Type AnnotationType
        {
            get { return _annotationType; }
            set
            {
                _annotationType = value;
                OnPropertyChanged(nameof(AnnotationType));
            }
        }

        public bool IsAnnotationCreationEnable
        {
            get { return _isAnnotationCreationEnable; }
            set
            {
                _isAnnotationCreationEnable = value;
                OnPropertyChanged(nameof(IsAnnotationCreationEnable));
            }
        }

        public bool IsZoomPanEnabled
        {
            get { return _isZoomPanEnabled; }
            set
            {
                _isZoomPanEnabled = value;
                OnPropertyChanged(nameof(IsZoomPanEnabled));
            }
        }

        public ICommand SetAnnotationCreationTypeCommand
        {
            get { return new ActionCommand<Type>(SetAnnotationTypeExecute); }
        }

        public ICommand AnnotationCreatedCommand
        {
            get
            {
                return new SciChart.Charting.Common.Helpers.ActionCommand<AnnotationCreationMVVMArgs>((e) =>
                {
                    var annotation = (IAnnotationViewModel) e.NewAnnotationViewModel;

                    if (annotation != null)
                    {
                        var tradingAnnotation = annotation as ITradingAnnotationViewModel;
                        if (tradingAnnotation != null)
                        {
                            ((AnnotationBase) tradingAnnotation.Annotation).Selected += OnAnnotationSelected;
                            ((AnnotationBase) tradingAnnotation.Annotation).Unselected += OnAnnotationUnselected;
                        }

                        annotation.IsEditable = true;
                        annotation.CanEditText = true;
                        annotation.IsSelected = true;
                    }

                    IsAnnotationCreationEnable = annotation is BrushAnnotationViewModel;

                    if (!(annotation is BrushAnnotationViewModel))
                    {
                        IsAnnotationDrawn = false;
                        OnPropertyChanged(nameof(IsAnnotationDrawn));
                    }
                });
            }
        }
        public List<Brush> AllBrushes
        {
            get
            {
                return typeof(Brushes).GetProperties().Select(x => (Brush) x.GetValue(null, null)).ToList();
            }
        }


        public ICommand DeleteSelectedAnnotationCommand
        {
            get { return new SciChart.Charting.Common.Helpers.ActionCommand(DeleteSelectedAnnotationOnSelectedPane); }
        }

        public ICommand WorkSpaceKeyUpCommand
        {
            get
            {
                return new SciChart.Charting.Common.Helpers.ActionCommand<KeyEventArgs>((e) =>
                {
                    if (e.Key == Key.Delete)
                    {
                        DeleteSelectedAnnotationOnSelectedPane();
                    }
                });
            }
        }

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
            {
                foreach (IAnnotationViewModel annotation in args.OldItems)
                {
                    var tradingAnnotation = annotation as ITradingAnnotationViewModel;
                    if (tradingAnnotation != null)
                    {
                        ((AnnotationBase)tradingAnnotation.Annotation).Selected -= OnAnnotationSelected;
                        ((AnnotationBase)tradingAnnotation.Annotation).Unselected -= OnAnnotationUnselected;
                    }
                }
            }
        }

        private void OnAnnotationUnselected(object sender, EventArgs eventArgs)
        {
            OnPropertyChanged(nameof(SelectedAnnotation));
            IsEditPanelVisible = SelectedAnnotation != null;
        }

        private void OnAnnotationSelected(object sender, EventArgs eventArgs)
        {
            OnPropertyChanged(nameof(SelectedAnnotation));
            IsEditPanelVisible = SelectedAnnotation != null;
        }

        private void SetAnnotationTypeExecute(Type type)
        {
            if (IsAnnotationCreationEnable && type.IsEquivalentTo(AnnotationType))
            {
                IsAnnotationCreationEnable = false;
                AnnotationType = null;
            }
            else
            {
                IsAnnotationCreationEnable = true;
                AnnotationType = type;
            }
        }

        private IOhlcDataSeries GetPriceDataSeries()
        {
            var stockPrices = new OhlcDataSeries<DateTime, double>();

            var prices = DataManager.Instance.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);
            stockPrices.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);

            return stockPrices;
        }

        private void DeleteSelectedAnnotationOnSelectedPane()
        {
            Annotations.RemoveWhere(x => x.IsSelected);
        }
    }
}
