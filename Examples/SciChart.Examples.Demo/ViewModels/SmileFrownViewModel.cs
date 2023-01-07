using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading.Tasks;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.ExternalDependencies.Common;
using ActionCommand = SciChart.Charting.Common.Helpers.ActionCommand;

namespace SciChart.Examples.Demo.ViewModels
{
    public class SmileFrownViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly ExampleViewModel _parent;
        private bool _isSmile;
        private bool _isFrown;
        private string _feedbackEmail = "";
        private string _feedbackSubject = "";
        private string _feedbackContent = "";
        private bool _isFrownVisible;
        private bool _isSmileVisible;
        private bool _onSubmitted;

        public SmileFrownViewModel(ExampleViewModel parent)
        {
            _parent = parent;
            ExampleChanged();
        }

        public void ExampleChanged()
        {
            var usageService = _parent.Usage;
            if (usageService != null)
            {
                IsSmile = usageService.FeedbackType.GetValueOrDefault(ExampleFeedbackType.Frown) == ExampleFeedbackType.Smile;
                IsFrown = usageService.FeedbackType.GetValueOrDefault(ExampleFeedbackType.Smile) == ExampleFeedbackType.Frown;
                
                FeedbackEmail = usageService.Email;
                
                if (!string.IsNullOrEmpty(usageService.FeedbackText))
                {
                    FeedbackSubject = usageService.FeedbackText.Contains(":")
                        ? usageService.FeedbackText.Substring(0, usageService.FeedbackText.IndexOf(":", StringComparison.InvariantCulture))
                        : string.Empty;

                    FeedbackContent = usageService.FeedbackText.Contains(":")
                        ? usageService.FeedbackText.Substring(usageService.FeedbackText.IndexOf(":", StringComparison.InvariantCulture) + 1)
                        : usageService.FeedbackText;
                }
                else
                {
                    FeedbackSubject = null;
                    FeedbackContent = null;
                }
            }
   
            FrownVisible = false;
            SmileVisible = false;
        }

        public ActionCommand CloseCommand => new ActionCommand(() =>
        {
            SmileVisible = false;
            FrownVisible = false;

            var usageService = _parent.Usage;
            if (usageService != null)
            {
                // Revert to original values
                IsSmile = usageService.FeedbackType.GetValueOrDefault(ExampleFeedbackType.Frown) == ExampleFeedbackType.Smile;
                IsFrown = usageService.FeedbackType.GetValueOrDefault(ExampleFeedbackType.Smile) == ExampleFeedbackType.Frown;

                FeedbackEmail = usageService.Email;

                if (!string.IsNullOrEmpty(usageService.FeedbackText))
                {
                    FeedbackSubject = usageService.FeedbackText.Contains(":")
                        ? usageService.FeedbackText.Substring(0, usageService.FeedbackText.IndexOf(":", StringComparison.InvariantCulture))
                        : string.Empty;

                    FeedbackContent = usageService.FeedbackText.Contains(":")
                        ? usageService.FeedbackText.Substring(usageService.FeedbackText.IndexOf(":", StringComparison.InvariantCulture) + 1)
                        : usageService.FeedbackText;
                }
                else
                {
                    FeedbackSubject = null;
                    FeedbackContent = null;
                }
            }
        });

        public ActionCommand SubmitCommand => new ActionCommand(async () =>
        {
            OnSubmitted = true;

            var usageService = _parent.Usage;
            if (usageService != null)
            {
                usageService.FeedbackType = _isSmile
                    ? ExampleFeedbackType.Smile
                    : (_isFrown ? ExampleFeedbackType.Frown : (ExampleFeedbackType?)null);

                usageService.FeedbackText = _feedbackSubject +
                                            ((_feedbackSubject + _feedbackContent).Length > 0 ? ": " : "") +
                                            _feedbackContent;

                usageService.Email = _feedbackEmail;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(1500));

            SmileVisible = false;
            FrownVisible = false;

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            OnSubmitted = false;
        });

        public bool IsSmile
        {
            get => _isSmile;
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
            get => _isFrown;
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

        public bool OnSubmitted
        {
            get => _onSubmitted;
            set
            {
                if (_onSubmitted != value)
                {
                    _onSubmitted = value;
                    OnPropertyChanged(nameof(OnSubmitted));
                }
            }
        }

        public string FeedbackEmail
        {
            get => _feedbackEmail;
            set
            {
                if (_feedbackEmail != value)
                {
                    _feedbackEmail = value;
                    OnPropertyChanged(nameof(FeedbackEmail));
                }
            }
        }

        public string FeedbackSubject
        {
            get => _feedbackSubject;
            set
            {
                if (_feedbackSubject != value)
                {
                    _feedbackSubject = value;
                    OnPropertyChanged(nameof(FeedbackSubject));
                }
            }
        }

        public string FeedbackContent
        {
            get => _feedbackContent;
            set
            {
                if (_feedbackContent != value)
                {
                    _feedbackContent = value;
                    OnPropertyChanged(nameof(FeedbackContent));
                }
            }
        }

        public bool SmileVisible
        {
            get => _isSmileVisible;
            set
            {
                if (_isSmileVisible != value)
                {
                    _isSmileVisible = value;
                    OnPropertyChanged(nameof(SmileVisible));
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
            get => _isFrownVisible;
            set
            {
                if (_isFrownVisible != value)
                {
                    _isFrownVisible = value;
                    OnPropertyChanged(nameof(FrownVisible));
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

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(FeedbackEmail) && !IsValidEmail(FeedbackEmail))
                {
                    return "Please enter a valid email so we can reply";
                }

                if (columnName == nameof(FeedbackSubject) && string.IsNullOrWhiteSpace(FeedbackSubject))
                {
                    return "Please enter a subject line";
                }

                if (columnName == nameof(FeedbackContent) && string.IsNullOrWhiteSpace(FeedbackContent))
                {
                    return "Please tell us some feedback";
                }

                return null;
            }
        }

        private bool IsValidEmail(string feedbackEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(feedbackEmail))
                {
                    return false;
                }

                var mailAddress = new MailAddress(feedbackEmail);

                return mailAddress != null;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public string Error { get; }
    }
}