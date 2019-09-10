using System.Collections.Generic;
using System.Linq;
using Unity;
using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.UI.Bootstrap;
using SciChart.UI.Reactive.Services;
using SciChart.UI.Reactive;
using SciChart.Wpf.UI.Transitionz;
using Unity.Attributes;
using ActionCommand = SciChart.Charting.Common.Helpers.ActionCommand;

namespace SciChart.Examples.Demo.ViewModels
{
    public interface IExampleViewModel
    {
        Example SelectedExample { get; set; }
    }

    [ExportType(typeof(IExampleViewModel), CreateAs.Singleton)]
    public class ExampleViewModel : BaseViewModel, IExampleViewModel
    {
        private Example _selectedExample;

        private bool _showSourceCode;
        private KeyValuePair<string, string> _selectedFile;

        private readonly ActionCommand _exportExampleCommand;
        private readonly ActionCommand _sendSmileCommand;
        private readonly ActionCommand _sendFrownCommand;

        public ExampleUsage Usage { get { return _selectedExample.Usage; } }

        private bool _showExample;

        private readonly IBlurParams _defaultParams = new BlurParams() { Duration = 200, From = 10, To = 0 };
        private readonly IBlurParams _blurredParams = new BlurParams() { Duration = 200, From = 0, To = 10 };
        private bool _firstLoad = true;
        private bool _isInfoVisible;
        private bool _showBreadcrumbNavigation;
        private ActionCommand _closeHelpCommand;
        private bool _isShowingHelp;

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

            _exportExampleCommand = new ActionCommand(() =>
            {
                ExportExampleViewModel.IsExportVisible = true;
            });

            _sendSmileCommand = new ActionCommand(() =>
            {
                SmileFrownViewModel.SmileVisible = !SmileFrownViewModel.SmileVisible;
                SmileFrownViewModel.IsSmile = SmileFrownViewModel.SmileVisible;

            });

            _sendFrownCommand = new ActionCommand(() =>
            {
                SmileFrownViewModel.FrownVisible = !SmileFrownViewModel.FrownVisible;
                SmileFrownViewModel.IsFrown = SmileFrownViewModel.FrownVisible;
            });

            _closeHelpCommand = new ActionCommand(() =>
            {
                IsShowingHelp = false;
            });

            SmileFrownViewModel = new SmileFrownViewModel(this);
            ExportExampleViewModel = new ExportExampleViewModel(module, this);
            BreadCrumbViewModel = new BreadCrumbViewModel(module, this);
            this.ShowExample = true;
            this.IsShowingHelp = true;
            this.IsInfoVisible = true;
        }

        public string Group { get; set; }

        public SmileFrownViewModel SmileFrownViewModel { get; private set; }

        public ExportExampleViewModel ExportExampleViewModel { get; private set; }

        public BreadCrumbViewModel BreadCrumbViewModel { get; private set; }

        public Example SelectedExample
        {
            get { return _selectedExample; }
            set
            {
                _selectedExample = value;
                ServiceLocator.Container.Resolve<IMainWindowViewModel>().SelectedExample = value;

                SelectedFile = _selectedExample.SourceFiles.FirstOrDefault();
                OnPropertyChanged("SelectedExample");
                if (SmileFrownViewModel != null)
                    SmileFrownViewModel.ExampleChanged();
                if (BreadCrumbViewModel != null)
                    BreadCrumbViewModel.UpdateSelectedExample();
                ShowExample = true;
            }
        }

        public bool ShowExample
        {
            get { return _showExample; }
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
            get { return _isShowingHelp; }
            set
            {
                if (_isShowingHelp != value)
                {
                    _isShowingHelp = value;
                    OnPropertyChanged("IsShowingHelp");
                }
            }
        }

        public ActionCommand CloseHelpCommand
        {
            get
            {
                return _closeHelpCommand;
            }
        }

        public ActionCommand HideDescriptionCommand
        {
            get
            {
                return new ActionCommand(() => IsInfoVisible = false);
            }
        }

        public bool ShowSourceCode
        {
            get { return _showSourceCode; }
            set
            {
                if (_showSourceCode != value)
                {
                    _showSourceCode = value;
                    OnPropertyChanged("ShowSourceCode");

                    ShowExample = !ShowSourceCode;
                    if (_showSourceCode)
                    {
                        Usage.ViewedSource = true;
                    }
                }
            }
        }

        public bool IsInfoVisible
        {
            get { return _isInfoVisible; }
            set
            {
                _isInfoVisible = value;
                OnPropertyChanged("IsInfoVisible");
            }
        }

        public KeyValuePair<string, string> SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OnPropertyChanged("SelectedFile");
                OnPropertyChanged("SourceOpacityParams");
            }
        }

        public IOpacityParams SourceOpacityParams
        {
            get { return new OpacityParams() { From = 0, To = 1, Duration = 100, TransitionOn = TransitionOn.Once }; }
        }

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

                return (SmileFrownViewModel.SmileVisible | SmileFrownViewModel.FrownVisible | ExportExampleViewModel.IsExportVisible | BreadCrumbViewModel.IsShowingBreadcrumbNavigation) ?
                    _blurredParams : _defaultParams;
            }
        }

        public ActionCommand ExportExampleCommand
        {
            get { return _exportExampleCommand; }
        }

        public ActionCommand SendSmileCommand
        {
            get { return _sendSmileCommand; }
        }

        public ActionCommand SendFrownCommand
        {
            get { return _sendFrownCommand; }
        }

        public void InvalidateDialogProperties()
        {
            OnPropertyChanged("BackgroundBlurParams");
        }

    }
}