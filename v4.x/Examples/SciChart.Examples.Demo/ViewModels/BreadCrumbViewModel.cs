using System.Collections.Generic;
using System.Linq;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Wpf.UI.Reactive;

namespace SciChart.Examples.Demo.ViewModels
{
    public class BreadCrumbViewModel : BaseViewModel
    {
        private readonly IModule _module;
        private readonly ExampleViewModel _parent;
        private readonly List<BreadcrumbItemViewModel> _breadcrumbItems;
        private bool _isShowingNavigation;
        private IEnumerable<Example> _examples;
        private string _selectedTopLevelCategory;
        private string _selectedChartGroup;
        private Example _selectedExample;
        private IEnumerable<string> _allChartGroups;
        private IEnumerable<string> _allTopLevelCategories;

        public BreadCrumbViewModel(IModule module, ExampleViewModel parent)
        {
            _module = module;
            _parent = parent;
            _breadcrumbItems = new List<BreadcrumbItemViewModel>() 
            { 
                new BreadcrumbItemViewModel(parent.SelectedExample.TopLevelCategory, ShowNavigationCommand),
                new BreadcrumbItemViewModel(parent.SelectedExample.Group, ShowNavigationCommand),
                new BreadcrumbItemViewModel(parent.SelectedExample.Title, ShowNavigationCommand), 
            };

            UpdateSelectedExample();
        }

        public ActionCommand ShowNavigationCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    IsShowingBreadcrumbNavigation = true;
                });
            }
        }

        public ActionCommand HideNavigationCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    IsShowingBreadcrumbNavigation = false;
                });
            }
        }

        public IEnumerable<string> AllTopLevelCategories
        {
            get
            {
                return _allTopLevelCategories;
            }
            set
            {
                _allTopLevelCategories = value;
                OnPropertyChanged("AllTopLevelCategories");
            }
        }

        public string SelectedTopLevelCategory
        {
            get { return _selectedTopLevelCategory; }
            set
            {
                _selectedTopLevelCategory = value;
                OnPropertyChanged("SelectedTopLevelCategory");

                AllChartGroups = _module.GroupsByCategory[_selectedTopLevelCategory];
                SelectedChartGroup = AllChartGroups.Any(x => x.Equals(SelectedChartGroup)) ? SelectedChartGroup : AllChartGroups.FirstOrDefault();
            }
        }

        public IEnumerable<string> AllChartGroups
        {
            get
            {
                return _allChartGroups;
            }
            set
            {
                _allChartGroups = value;
                OnPropertyChanged("AllChartGroups");
            }
        }

        public string SelectedChartGroup
        {
            get { return _selectedChartGroup; }
            set
            {
                _selectedChartGroup = value ?? _module.CurrentExample.Group;
                Examples = _module.ExamplesByCategoryAndGroup(_selectedTopLevelCategory, _selectedChartGroup);
                OnPropertyChanged("SelectedChartGroup");
            }
        }

        public Example SelectedExample
        {
            get { return _selectedExample; }
            set
            {
                _selectedExample = value;

                if (_selectedExample != null && _parent.SelectedExample != _selectedExample)
                {
                    IsShowingBreadcrumbNavigation = false;
                    _selectedExample.SelectCommand.Execute(_selectedExample);
                    _parent.SelectedExample = _selectedExample;
                    OnPropertyChanged("SelectedExample");
                }
                else if (_selectedExample == null)
                {
                    _selectedExample = _parent.SelectedExample;
                    OnPropertyChanged("SelectedExample");
                }
            }
        }

        public IEnumerable<Example> Examples
        {
            get { return _examples; }
            set
            {
                _examples = value;
                OnPropertyChanged("Examples");
            }
        }

        public bool IsShowingBreadcrumbNavigation
        {
            get { return _isShowingNavigation; }
            set
            {
                if (_isShowingNavigation != value)
                {
                    _isShowingNavigation = value;

                    if (_isShowingNavigation)
                    {
                        _parent.SmileFrownViewModel.SmileVisible = false;
                        _parent.SmileFrownViewModel.FrownVisible = false;
                        _parent.ExportExampleViewModel.IsExportVisible = false;
                    }
                    _parent.InvalidateDialogProperties();
                    OnPropertyChanged("IsShowingBreadcrumbNavigation");
                }
            }
        }

        public IEnumerable<BreadcrumbItemViewModel> BreadCrumbItemViewModels { get { return _breadcrumbItems; } }

        public void UpdateSelectedExample()
        {
            _selectedExample = _module.CurrentExample;
            _selectedTopLevelCategory = _module.CurrentExample.TopLevelCategory;
            _selectedChartGroup = _module.CurrentExample.Group;

            _breadcrumbItems[0].Content = _selectedTopLevelCategory;
            _breadcrumbItems[1].Content = _selectedChartGroup;
            _breadcrumbItems[2].Content = _selectedExample.Title;

            AllTopLevelCategories = _module.AllCategories;
            AllChartGroups = _module.GroupsByCategory[_selectedTopLevelCategory];
            Examples = _module.ExamplesByCategoryAndGroup(_selectedTopLevelCategory, _selectedChartGroup);

            OnPropertyChanged("SelectedTopLevelCategory");
            OnPropertyChanged("SelectedChartGroup");
            OnPropertyChanged("SelectedExample");
        }
    }
}