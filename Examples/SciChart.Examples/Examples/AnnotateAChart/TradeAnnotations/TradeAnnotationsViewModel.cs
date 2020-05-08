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
using SciChart.Charting.Visuals.Annotations;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.AnnotateAChart.TradeAnnotations
{
    public class TradeAnnotationsViewModel : BaseViewModel
    {
        private ObservableCollection<IRenderableSeriesViewModel> _series;
        private ObservableCollection<IAnnotationViewModel> _annotations;
        
        private Type _annotationType;
        private IAnnotationViewModel _selectedAnnotation;

        private bool _isEditPanelVisible;
        private bool _isAnnotationCreationEnable;

        private bool _isZoomPanEnabled;
        private bool _isRubberBandEnabled;

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
            get => _isEditPanelVisible;
            set
            {
                _isEditPanelVisible = value;
                OnPropertyChanged("IsEditPanelVisible");
            }
        }

        public IAnnotationViewModel SelectedAnnotation
        {
            get => _selectedAnnotation;
            set
            {
                _selectedAnnotation = value; 
                OnPropertyChanged("SelectedAnnotation");
            }
        }

        public bool IsRubberBandEnabled
        {
            get => _isRubberBandEnabled;
            set
            {
                _isRubberBandEnabled = value;
                OnPropertyChanged("IsRubberBandEnabled");
            }
        }

        public bool IsAnnotationDrawn { set; get; }

        public ObservableCollection<IRenderableSeriesViewModel> Series
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged("Series");
            }
        }

        public ObservableCollection<IAnnotationViewModel> Annotations
        {
            get => _annotations;
            set
            {
                _annotations = value;
                OnPropertyChanged("Annotations");
            }
        }

        public Type AnnotationType
        {
            get => _annotationType;
            set
            {
                _annotationType = value;
                OnPropertyChanged("AnnotationType");
            }
        }

        public bool IsAnnotationCreationEnable
        {
            get => _isAnnotationCreationEnable;
            set
            {
                _isAnnotationCreationEnable = value;
                OnPropertyChanged("IsAnnotationCreationEnable");
            }
        }

        public bool IsZoomPanEnabled
        {
            get => _isZoomPanEnabled;
            set
            {
                _isZoomPanEnabled = value;
                OnPropertyChanged("IsZoomPanEnabled");
            }
        }

        public ICommand SetAnnotationCreationTypeCommand => new ActionCommand<Type>(SetAnnotationTypeExecute);

        public ICommand AnnotationCreatedCommand => new ActionCommand<AnnotationCreationMVVMArgs>(e =>
        {
            var annotation = e.NewAnnotationViewModel;

            if (annotation != null)
            {
                if (annotation is ITradingAnnotationViewModel tradingAnnotation)
                {
                    ((AnnotationBase) tradingAnnotation.Annotation).Selected += OnAnnotationSelectionChanged;
                    ((AnnotationBase) tradingAnnotation.Annotation).Unselected += OnAnnotationSelectionChanged;
                }

                annotation.IsEditable = true;
                annotation.CanEditText = true;
                annotation.IsSelected = true;
            }

            IsAnnotationCreationEnable = false;
            IsAnnotationDrawn = false;

            OnPropertyChanged("IsAnnotationDrawn");
        });
        
        public List<Brush> AllBrushes => typeof(Brushes).GetProperties().Select(x => (Brush)x.GetValue(null, null)).ToList();

        public ICommand DeleteSelectedAnnotationCommand => new ActionCommand(DeleteSelectedAnnotationOnSelectedPane);

        public ICommand WorkSpaceKeyUpCommand => new ActionCommand<KeyEventArgs>(e =>
        {
            if (e.Key == Key.Delete)
            {
                DeleteSelectedAnnotationOnSelectedPane();
            }
        });

        private void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
            {
                foreach (IAnnotationViewModel annotation in args.OldItems)
                {
                    if (annotation is ITradingAnnotationViewModel tradingAnnotation)
                    {
                        ((AnnotationBase)tradingAnnotation.Annotation).Selected -= OnAnnotationSelectionChanged;
                        ((AnnotationBase)tradingAnnotation.Annotation).Unselected -= OnAnnotationSelectionChanged;
                    }
                }
            }
        }

        private void OnAnnotationSelectionChanged(object sender, EventArgs eventArgs)
        {
            SelectedAnnotation = Annotations.FirstOrDefault(x => x.IsSelected);
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
            SelectedAnnotation = null;
            IsEditPanelVisible = false;
        }
    }
}