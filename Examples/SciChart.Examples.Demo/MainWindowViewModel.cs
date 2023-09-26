using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SciChart.Charting.Visuals;
using Unity;
using SciChart.Charting.Common.Helpers;
using SciChart.Core.Utility;
using SciChart.Examples.Demo.Behaviors;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Helpers.HtmlExport;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using SciChart.Examples.Demo.ViewModels;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.UI.Reactive;
using SciChart.UI.Bootstrap;
using SciChart.UI.Reactive.Observability;
using SciChart.Wpf.UI.Transitionz;
using Unity.Attributes;
using ActionCommand = SciChart.Charting.Common.Helpers.ActionCommand;

namespace SciChart.Examples.Demo
{
    public interface IMainWindowViewModel
    {
        bool InitReady { get; set; }
        string SearchText { get; set; }
        ActionCommand HideSearchCommand { get; }
        Example SelectedExample { get; set; }
    }

    [ExportType(typeof(IMainWindowViewModel), CreateAs.Singleton)]
    public class MainWindowViewModel : ViewModelWithTraitsBase, IMainWindowViewModel
    {
        private readonly ActionCommand _hideSearchCommand;
        private readonly ActionCommand _exportCommand;
        private readonly ActionCommand _exportAllHtmlCommand;
        private readonly ActionCommand _showSettingsCommand;
        private readonly ActionCommand _hideSettingsCommand;
        private readonly IBlurParams _defaultParams = new BlurParams() { Duration = 200, From = 10, To = 0 };
        private readonly IBlurParams _blurredParams = new BlurParams() { Duration = 200, From = 0, To = 10 };

        private List<string> _autoCompleteDataSource;
        private ActionCommand _exportAllSolutionsCommand;
        private ActionCommand _gcCollectCommand;

        /// <summary>
        /// Design time constructor
        /// </summary>
        public MainWindowViewModel()
        {
            Categories = new[]
            {
                new ExampleCategoryViewModel {Category = "3D Charts"},
                new ExampleCategoryViewModel {Category = "2D Charts"},
                new ExampleCategoryViewModel {Category = "Featured Apps"},
            };

            var stubExamples = new Dictionary<Guid, Example>
            {
                {Guid.NewGuid(), new Example(null, new ExampleDefinition {Title = "An Example"})},
                {Guid.NewGuid(), new Example(null, new ExampleDefinition {Title = "Another Example"})},
                {Guid.NewGuid(), new Example(null, new ExampleDefinition {Title = "SuperCool Example"})},
            };
            EverythingViewModel = new EverythingViewModel(stubExamples);
        }

        [InjectionConstructor]
        public MainWindowViewModel(IModule module, IUsageCalculator usageCalculator)
        {
            SearchText = "";
            SearchResults = new ObservableCollection<ISelectable>();
            _autoCompleteDataSource = module.Examples.Select(ex => ex.Value.Title).ToList();

            WithTrait<AutoCompleteSearchBehaviour>();
            WithTrait<InitializationBehaviour>();

            _hideSearchCommand = new ActionCommand(() =>
            {
                SearchText = null;
            });

            _showSettingsCommand = new ActionCommand(() =>
            {
                IsSettingsShow = true;
                BlurOnSearchParams = _blurredParams;
            });

            _hideSettingsCommand= new ActionCommand(() =>
            {
                IsSettingsShow = false;
                BlurOnSearchParams = _defaultParams;
            });

            _exportCommand = new ActionCommand(() =>
            {
                _hideSettingsCommand.Execute(null);
                DeveloperModManager.Manage.IsDeveloperMode = false;
                TimedMethod.Invoke(() => HtmlExportHelper.ExportExampleToHtml(SelectedExample)).After(1000).Go();
            }, () => SelectedExample != null);

            _exportAllHtmlCommand = new ActionCommand(() =>
            {
                _hideSettingsCommand.Execute(null);
                DeveloperModManager.Manage.IsDeveloperMode = false;
                TimedMethod.Invoke(() => HtmlExportHelper.ExportExamplesToHtml(module)).After(1000).Go();
            }, () => SelectedExample != null);

            _exportAllSolutionsCommand = new ActionCommand(() =>
            {
                _hideSettingsCommand.Execute(null);
                ExportExampleHelper.ExportExamplesToSolutions(module);
            }, () => SelectedExample != null);

            _gcCollectCommand = new ActionCommand(() =>
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForFullGCApproach();
            });
        }

        public ICommand GoBackCommand { get { return Navigator.Instance.GoBackCommand; } }
        public ICommand GoForwardCommand { get { return Navigator.Instance.GoForwardCommand; } }

        public bool IsSettingsShow
        {
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public ActionCommand GCCollectCommand
        {
            get { return _gcCollectCommand; }
        }
        public ActionCommand HideSettingsCommand
        {
            get { return _hideSettingsCommand; }
        }

        public List<string> AutoCompleteDataSource
        {
            get { return _autoCompleteDataSource; }
        }

        public bool InitReady
        {
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public Guid HomePage
        {
            get { return AppPage.HomePageId; }
        }

        public string VersionAndLicenseInfo
        {
            get { return SciChartSurface.VersionAndLicenseInfo; }
        }

        public bool SearchBoxEnabled
        {
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public EverythingViewModel EverythingViewModel
        {
            get => this.GetDynamicValue<EverythingViewModel>(); 
            set => this.SetDynamicValue(value);
        }

        public ObservableCollection<ISelectable> SearchResults
        {
            get { return GetDynamicValue<ObservableCollection<ISelectable>>("SearchResults"); }
            set => this.SetDynamicValue(value);
        }

        public bool HasSearchResults
        {
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public IEnumerable<ExampleCategoryViewModel> Categories { get; set; }

        public bool IsBusy
        {
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public ExampleCategoryViewModel SelectedCategory
        {
            get => this.GetDynamicValue<ExampleCategoryViewModel>(); 
            set => this.SetDynamicValue(value);
        }

        public string SearchText
        {
            get => this.GetDynamicValue<string>(); 
            set => this.SetDynamicValue(value);
        }

        public IBlurParams BlurOnSearchParams
        {
            get => this.GetDynamicValue<IBlurParams>(); 
            set => this.SetDynamicValue(value);
        }

        public bool IsMainPage
        {
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public IBlurParams BlurBackgroundParams
        {
            get => this.GetDynamicValue<IBlurParams>(); 
            set => this.SetDynamicValue(value);
        }

        public Example SelectedExample
        {
            get => this.GetDynamicValue<Example>(); 
            set
            {
                this.SetDynamicValue(value);
                ExportToHtmlCommand.RaiseCanExecuteChanged();
                ExportAllHtmlCommand.RaiseCanExecuteChanged();
                ExportAllSolutionsCommand.RaiseCanExecuteChanged();
            }
        }

        public ActionCommand NavigateToHomeCommand
        {
            get { return Navigator.Instance.NavigateToHomeCommand; }
        }

        public ActionCommand HideSearchCommand
        {
            get { return _hideSearchCommand; }
        }

        public ActionCommand ExportToHtmlCommand
        {
            get { return _exportCommand; }
        }

        public ActionCommand ShowSettingsCommand
        {
            get { return _showSettingsCommand; }
        }
        public ActionCommand ExportAllHtmlCommand
        {
            get { return _exportAllHtmlCommand; }
        }
        public ActionCommand ExportAllSolutionsCommand
        {
            get { return _exportAllSolutionsCommand; }
        }
    }
}