using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading.Tasks;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.ExternalDependencies.Common;
using ActionCommand = SciChart.Charting.Common.Helpers.ActionCommand;

namespace SciChart.Examples.Demo.ViewModels
{
    public class FeedbackViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly ExampleViewModel _parent;
        private bool _isSmile;
        private bool _isFrown;
        private string _feedbackEmail = "";
        private string _feedbackSubject = "";
        private string _feedbackContent = "";
        private bool _isFeedbackVisible;
        private bool _onSubmitted;

        public FeedbackViewModel(ExampleViewModel parent)
        {
            _parent = parent;
            IsFeedbackVisible = false;

            CloseCommand = new ActionCommand(CloseExecute);
            SubmitCommand = new ActionCommand(SubmitExecute, () => IsValid);

            ExampleChanged();
        }

        public void ExampleChanged()
        {
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
                    FeedbackSubject = string.Empty;
                    FeedbackContent = string.Empty;
                }
            }

            IsFeedbackVisible = false;
        }

        public void CloseExecute()
        {
            IsFeedbackVisible = false;

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
                    FeedbackSubject = string.Empty;
                    FeedbackContent = string.Empty;
                }
            }
        }

        public async void SubmitExecute()
        {
            OnSubmitted = true;

            var usageService = _parent.Usage;
            if (usageService != null)
            {
                usageService.FeedbackType = _isSmile ? ExampleFeedbackType.Smile
                    : (_isFrown ? ExampleFeedbackType.Frown : (ExampleFeedbackType?)null);

                usageService.FeedbackText = _feedbackSubject +
                                            ((_feedbackSubject + _feedbackContent).Length > 0 ? ": " : "") +
                                            _feedbackContent;

                usageService.Email = _feedbackEmail;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(1500));

            IsFeedbackVisible = false;

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            OnSubmitted = false;
        }

        public ActionCommand CloseCommand { get; }
        public ActionCommand SubmitCommand { get; }

        public bool IsSmile
        {
            get => _isSmile;
            set
            {
                if (_isSmile != value)
                {
                    _isSmile = value;
                    OnPropertyChanged(nameof(IsSmile));

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
                    OnPropertyChanged(nameof(IsFrown));

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
                    SubmitCommand?.RaiseCanExecuteChanged();
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
                    SubmitCommand?.RaiseCanExecuteChanged();
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
                    SubmitCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsFeedbackVisible
        {
            get => _isFeedbackVisible;
            set
            {
                if (_isFeedbackVisible != value)
                {
                    _isFeedbackVisible = value;
                    OnPropertyChanged(nameof(IsFeedbackVisible));
                }

                if (IsFeedbackVisible)
                {
                    _parent.ExportExampleViewModel.IsExportVisible = false;
                    _parent.BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
                }
                else
                {
                    FeedbackSubject = string.Empty;
                    FeedbackContent = string.Empty;
                }

                _parent.InvalidateDialogProperties();
            }
        }

        
        #region IDataErrorInfo

        private readonly string[] ValidatedProperties =
        {
            nameof(FeedbackEmail),
            nameof(FeedbackSubject),
            nameof(FeedbackContent)
        };

        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                {
                    if (GetValidationError(property) != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public string Error { get; private set; }

        public string this[string propertyName]
        {
            get { return GetValidationError(propertyName); }
        }

        private string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
            {
                return null;
            }

            if (propertyName == nameof(FeedbackEmail) && !IsValidEmail(FeedbackEmail))
            {
                return "Please enter a valid email so we can reply";
            }

            if (propertyName == nameof(FeedbackSubject) && string.IsNullOrWhiteSpace(FeedbackSubject))
            {
                return "Please enter a subject line";
            }

            if (propertyName == nameof(FeedbackContent) && string.IsNullOrWhiteSpace(FeedbackContent))
            {
                return "Please tell us some feedback";
            }

            return null;
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

        #endregion
    }
}