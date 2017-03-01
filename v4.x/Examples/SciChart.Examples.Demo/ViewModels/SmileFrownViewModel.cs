using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Services;

namespace SciChart.Examples.Demo.ViewModels
{
    public class SmileFrownViewModel : BaseViewModel
    {
        private readonly ExampleViewModel _parent;
        private bool _isSmile;
        private bool _isFrown;
        private bool _firstLoad;
        private string _feedbackEmail = "";
        private string _feedbackSubject = "";
        private string _feedbackContent = "";
        private bool _isFrownVisible;
        private bool _isSmileVisible;
        private bool _isVisible;

        public SmileFrownViewModel(ExampleViewModel parent)
        {
            _parent = parent;
            _firstLoad = true;
            ExampleChanged();
        }

        public void ExampleChanged()
        {
            IsSmile = _parent.Usage.FeedbackType.GetValueOrDefault(ExampleFeedbackType.Frown) == ExampleFeedbackType.Smile;
            IsFrown = _parent.Usage.FeedbackType.GetValueOrDefault(ExampleFeedbackType.Smile) == ExampleFeedbackType.Frown;
            FeedbackEmail = _parent.Usage.Email;
            if (!string.IsNullOrEmpty(_parent.Usage.FeedbackText))
            {
                FeedbackSubject = _parent.Usage.FeedbackText.Contains(":") ? _parent.Usage.FeedbackText.Substring(0, _parent.Usage.FeedbackText.IndexOf(":")) : "";
                FeedbackContent = _parent.Usage.FeedbackText.Contains(":") ? _parent.Usage.FeedbackText.Substring(_parent.Usage.FeedbackText.IndexOf(":") + 1) : _parent.Usage.FeedbackText;
            }
            else
            {
                FeedbackSubject = null;
                FeedbackContent = null;
            }


            FrownVisible = false;
            SmileVisible = false;
        }

        public ActionCommand CloseCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    SmileVisible = false;
                    FrownVisible = false;
                    // Revert to original values
                    IsSmile = _parent.Usage.FeedbackType.GetValueOrDefault(ExampleFeedbackType.Frown) == ExampleFeedbackType.Smile;
                    IsFrown = _parent.Usage.FeedbackType.GetValueOrDefault(ExampleFeedbackType.Smile) == ExampleFeedbackType.Frown;
                    FeedbackEmail = _parent.Usage.Email;
                    if (!string.IsNullOrEmpty(_parent.Usage.FeedbackText))
                    {
                        FeedbackSubject = _parent.Usage.FeedbackText.Contains(":") ? _parent.Usage.FeedbackText.Substring(0, _parent.Usage.FeedbackText.IndexOf(":")) : "";
                        FeedbackContent = _parent.Usage.FeedbackText.Contains(":") ? _parent.Usage.FeedbackText.Substring(_parent.Usage.FeedbackText.IndexOf(":") + 1) : _parent.Usage.FeedbackText;
                    }
                    else
                    {
                        FeedbackSubject = null;
                        FeedbackContent = null;
                    }

                });
            }
        }

        public ActionCommand SubmitCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    _parent.Usage.FeedbackType = (ExampleFeedbackType?)(_isSmile ? ExampleFeedbackType.Smile : (_isFrown ? ExampleFeedbackType.Frown : (ExampleFeedbackType?)null));
                    _parent.Usage.FeedbackText = _feedbackSubject + ((_feedbackSubject + _feedbackContent).Length > 0 ? ": " : "") + _feedbackContent;
                    _parent.Usage.Email = _feedbackEmail;

                    SmileVisible = false;
                    FrownVisible = false;
                });
            }
        }

        public bool IsSmile
        {
            get { return _isSmile; }
            set
            {
                if (_isSmile != value)
                {
                    _isSmile = value;

                    if (IsSmile)
                    {
                        IsFrown = false;
                    }
                }
            }
        }

        public bool IsFrown
        {
            get { return _isFrown; }
            set
            {
                if (_isFrown != value)
                {
                    _isFrown = value;

                    if (IsFrown)
                    {
                        IsSmile = false;
                    }
                }
            }
        }

        public string FeedbackEmail
        {
            get { return _feedbackEmail; }
            set
            {
                if (_feedbackEmail != value)
                {
                    _feedbackEmail = value;
                    OnPropertyChanged("FeedbackEmail");
                }
            }
        }

        public string FeedbackSubject
        {
            get { return _feedbackSubject; }
            set
            {
                if (_feedbackSubject != value)
                {
                    _feedbackSubject = value;
                    OnPropertyChanged("FeedbackSubject");
                }
            }
        }

        public string FeedbackContent
        {
            get { return _feedbackContent; }
            set
            {
                if (_feedbackContent != value)
                {
                    _feedbackContent = value;
                    OnPropertyChanged("FeedbackContent");
                }
            }
        }

        public bool SmileVisible
        {
            get { return _isSmileVisible; }
            set
            {
                if (_isSmileVisible != value)
                {
                    _isSmileVisible = value;
                    OnPropertyChanged("SmileVisible");
                }

                if (SmileVisible)
                {
                    _parent.ExportExampleViewModel.IsExportVisible = false;
                    _parent.BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
                    FrownVisible = false;
                }
                else
                {
                    FeedbackSubject = null;
                    FeedbackContent = null;
                }

                _parent.InvalidateDialogProperties();
            }
        }

        public bool FrownVisible
        {
            get { return _isFrownVisible; }
            set
            {
                if (_isFrownVisible != value)
                {
                    _isFrownVisible = value;
                    OnPropertyChanged("FrownVisible");
                }

                if (FrownVisible)
                {
                    _parent.ExportExampleViewModel.IsExportVisible = false;
                    _parent.BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
                    SmileVisible = false;
                }
                else
                {
                    FeedbackSubject = null;
                    FeedbackContent = null;
                }

                _parent.InvalidateDialogProperties();
            }
        }
    }
}