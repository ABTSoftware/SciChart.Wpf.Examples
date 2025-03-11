using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.UI.Bootstrap;
using SciChart.Wpf.UI.Transitionz;
using Unity;
using ActionCommand = SciChart.Charting.Common.Helpers.ActionCommand;

namespace SciChart.Examples.Demo.ViewModels
{
    public interface IExampleViewModel
    {
        bool InitReady { get; set; }

        Example SelectedExample { get; set; }

        ExportExampleViewModel ExportExampleViewModel { get; }

        BreadCrumbViewModel BreadCrumbViewModel { get; }
    }

    [ExportType(typeof(IExampleViewModel), CreateAs.Singleton)]
    public class ExampleViewModel : BaseViewModel, IExampleViewModel
    {
        private Example _selectedExample;
        private KeyValuePair<string, string> _selectedFile;

        private bool _showExample;
        private bool _showSourceCode;

        private readonly IBlurParams _defaultParams = new BlurParams { Duration = 200, From = 10, To = 0 };
        private readonly IBlurParams _blurredParams = new BlurParams { Duration = 200, From = 0, To = 10 };

        private bool _firstLoad = true;

        private bool _isInfoVisible;
        private bool _isShowingHelp;
        private bool _isShowingSourceCodeHelp;

        /// <summary>
        /// Designer Constructor
        /// </summary>
        public ExampleViewModel()
        {
        }

        [InjectionConstructor]
        public ExampleViewModel(IModule module)
        {
            SelectedExample = module.CurrentExample;

            ExportExampleCommand = new ActionCommand(() =>
            {
                ExportExampleViewModel.IsExportVisible = true;
            });

            SendFeedbackCommand = new ActionCommand(() =>
            {
                FeedbackViewModel.IsFeedbackVisible = true;
            });

            CloseHelpCommand = new ActionCommand(() =>
            {
                IsShowingHelp = false;
            });

            CloseSourceCodeHelpCommand = new ActionCommand(() =>
            {
                IsShowingSourceCodeHelp = false;
            });

            ShowGithubCommand = new ActionCommand(() =>
            {
                string githubUrl = SelectedExample == null
                    ? Urls.GithubRootUrl
                    : SelectedExample.GithubExampleUrl;

                var procStartInfo = new ProcessStartInfo(githubUrl) { UseShellExecute = true };
                Process.Start(procStartInfo);
            });

            GoToDocumentationCommand = new ActionCommand(() =>
            {
                var procStartInfo = new ProcessStartInfo(Urls.DocumentationRootUrl) { UseShellExecute = true };
                Process.Start(procStartInfo);
            });

            FeedbackViewModel = new FeedbackViewModel(this);
            ExportExampleViewModel = new ExportExampleViewModel(module, this);
            BreadCrumbViewModel = new BreadCrumbViewModel(module, this);

            ShowExample = true;
            IsInfoVisible = true;
#if !DEBUG
            IsShowingHelp = true;
            IsShowingSourceCodeHelp = true;
#endif
        }

        public bool InitReady { get; set; }

        public string Group { get; set; }

        public FeedbackViewModel FeedbackViewModel { get; }

        public ExportExampleViewModel ExportExampleViewModel { get; }

        public BreadCrumbViewModel BreadCrumbViewModel { get; }

        public Example SelectedExample
        {
            get => _selectedExample;
            set
            {
                _selectedExample = value;
                ServiceLocator.Container.Resolve<IMainWindowViewModel>().SelectedExample = value;

                SelectedFile = _selectedExample?.SourceFiles.FirstOrDefault() ?? default;
                OnPropertyChanged("SelectedExample");

                FeedbackViewModel?.ExampleChanged();
                BreadCrumbViewModel?.UpdateSelectedExample();

                ShowExample = true;
            }
        }

        public bool ShowExample
        {
            get => _showExample;
            set
            {
                if (_showExample != value)
                {
                    _showExample = value;
                    OnPropertyChanged("ShowExample");

                    ShowSourceCode = !ShowExample;
                }
            }
        }

        public bool IsShowingHelp
        {
            get => _isShowingHelp;
            set
            {
                if (_isShowingHelp != value)
                {
                    _isShowingHelp = value;
                    OnPropertyChanged("IsShowingHelp");
                }
            }
        }

        public bool IsShowingSourceCodeHelp
        {
            get => _isShowingSourceCodeHelp;
            set
            {
                if (_isShowingSourceCodeHelp != value)
                {
                    _isShowingSourceCodeHelp = value;
                    OnPropertyChanged("IsShowingSourceCodeHelp");
                }
            }
        }

        public ActionCommand CloseHelpCommand { get; }

        public ActionCommand CloseSourceCodeHelpCommand { get; }

        public ActionCommand ShowGithubCommand { get; }

        public ActionCommand GoToDocumentationCommand { get; }

        public ActionCommand HideDescriptionCommand
        {
            get
            {
                return new ActionCommand(() => IsInfoVisible = false);
            }
        }

        public ExampleUsage Usage => _selectedExample?.Usage;

        public bool ShowSourceCode
        {
            get => _showSourceCode;
            set
            {
                if (_showSourceCode != value)
                {
                    _showSourceCode = value;
                    OnPropertyChanged("ShowSourceCode");

                    ShowExample = !ShowSourceCode;
                    if (_showSourceCode && Usage != null)
                    {
                        Usage.ViewedSource = true;
                    }
                }
            }
        }

        public bool IsInfoVisible
        {
            get => _isInfoVisible;
            set
            {
                _isInfoVisible = value;
                OnPropertyChanged("IsInfoVisible");
            }
        }

        public KeyValuePair<string, string> SelectedFile
        {
            get => _selectedFile;
            set
            {
                _selectedFile = value;
                OnPropertyChanged("SelectedFile");
                OnPropertyChanged("SourceOpacityParams");
            }
        }

        public IOpacityParams SourceOpacityParams => new OpacityParams { From = 0, To = 1, Duration = 100, TransitionOn = TransitionOn.Once };

        public IBlurParams BackgroundBlurParams
        {
            get
            {
                if (_firstLoad)
                {
                    // Prevent blurring from N to 0 when example is shown
                    _firstLoad = false;
                    return null;
                }

                return (FeedbackViewModel.IsFeedbackVisible | ExportExampleViewModel.IsExportVisible | BreadCrumbViewModel.IsShowingBreadcrumbNavigation)
                    ? _blurredParams
                    : _defaultParams;
            }
        }

        public ActionCommand ExportExampleCommand { get; }

        public ActionCommand SendFeedbackCommand { get; }

        public void InvalidateDialogProperties()
        {
            OnPropertyChanged("BackgroundBlurParams");
        }
    }
}