using System.Collections.Generic;
using System.Linq;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.ExternalDependencies.Common;
using ActionCommand = SciChart.Charting.Common.Helpers.ActionCommand;

namespace SciChart.Examples.Demo.ViewModels
{
    public class BreadCrumbViewModel : BaseViewModel
    {
        private bool _isShowingNavigation;

        private readonly IModule _module;
        private readonly ExampleViewModel _parent;
        private readonly List<BreadcrumbItemViewModel> _breadcrumbItems;

        private IEnumerable<string> _allCategories;
        private IEnumerable<string> _allCategoryGroups;
        private IEnumerable<Example> _allGroupExamples;

        private string _selectedCategory;
        private string _selectedCategoryGroup;
        private Example _selectedGroupExample;

        public BreadCrumbViewModel(IModule module, ExampleViewModel parent)
        {
            ShowNavigationCommand = new ActionCommand(() => IsShowingBreadcrumbNavigation = true);
            HideNavigationCommand = new ActionCommand(() => IsShowingBreadcrumbNavigation = false);

            _module = module;
            _parent = parent;

            _breadcrumbItems = new List<BreadcrumbItemViewModel>()
            {
                new BreadcrumbItemViewModel(string.Empty, ShowNavigationCommand),
                new BreadcrumbItemViewModel(string.Empty, ShowNavigationCommand),
                new BreadcrumbItemViewModel(string.Empty, ShowNavigationCommand, true),
            };

            UpdateSelectedExample();
        }

        public ActionCommand ShowNavigationCommand { get; }
        public ActionCommand HideNavigationCommand { get; }

        public IEnumerable<string> AllCategories
        {
            get => _allCategories;         
            set
            {
                _allCategories = value;
                OnPropertyChanged(nameof(AllCategories));
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(Is2DCharts));
                OnPropertyChanged(nameof(Is3DCharts));

                AllCategoryGroups = _module.GroupsByCategory[_selectedCategory];
                SelectedCategoryGroup = AllCategoryGroups.FirstOrDefault(x => x == _parent.SelectedExample.Group) ?? AllCategoryGroups.First();
            }
        }

        public bool Is2DCharts => SelectedCategory?.Contains("2D") == true;
        public bool Is3DCharts => SelectedCategory?.Contains("3D") == true;

        public IEnumerable<string> AllCategoryGroups
        {
            get => _allCategoryGroups;
            set
            {
                _allCategoryGroups = value;
                OnPropertyChanged(nameof(AllCategoryGroups));
            }
        }

        public string SelectedCategoryGroup
        {
            get => _selectedCategoryGroup;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _selectedCategoryGroup = value;
                    OnPropertyChanged(nameof(SelectedCategoryGroup));

                    AllGroupExamples = _module.ExamplesByCategoryAndGroup(_selectedCategory, _selectedCategoryGroup);
                    SelectedGroupExample = AllGroupExamples.FirstOrDefault(x => x.Title == _parent.SelectedExample.Title);
                }
            }
        }

        public IEnumerable<Example> AllGroupExamples
        {
            get => _allGroupExamples;
            set
            {
                _allGroupExamples = value;
                OnPropertyChanged(nameof(AllGroupExamples));
            }
        }

        public Example SelectedGroupExample
        {
            get => _selectedGroupExample;
            set
            {
                _selectedGroupExample = value;
                OnPropertyChanged(nameof(SelectedGroupExample));

                if (_selectedGroupExample != null && _selectedGroupExample != _parent.SelectedExample)
                {
                    IsShowingBreadcrumbNavigation = false;
                    _selectedGroupExample.SelectCommand.Execute(_selectedGroupExample);
                    _parent.SelectedExample = _selectedGroupExample;
                }
            }
        }

        public bool IsShowingBreadcrumbNavigation
        {
            get => _isShowingNavigation;
            set
            {
                if (_isShowingNavigation != value)
                {
                    _isShowingNavigation = value;

                    if (_isShowingNavigation)
                    {
                        _parent.FeedbackViewModel.IsFeedbackVisible = false;
                        _parent.ExportExampleViewModel.IsExportVisible = false;
                    }

                    _parent.InvalidateDialogProperties();
                    OnPropertyChanged(nameof(IsShowingBreadcrumbNavigation));
                }
            }
        }

        public IEnumerable<BreadcrumbItemViewModel> BreadCrumbItemViewModels { get { return _breadcrumbItems; } }

        public void UpdateSelectedExample()
        {
            if (_module.CurrentExample == null) return;

            _selectedGroupExample = _module.CurrentExample;
            _selectedCategory = _module.CurrentExample.TopLevelCategory;
            _selectedCategoryGroup = _module.CurrentExample.Group;

            _breadcrumbItems[0].Content = _selectedCategory;
            _breadcrumbItems[1].Content = _selectedCategoryGroup;
            _breadcrumbItems[2].Content = _selectedGroupExample.Title;

            AllCategories = _module.AllCategories;
            AllCategoryGroups = _module.GroupsByCategory[_selectedCategory];
            AllGroupExamples = _module.ExamplesByCategoryAndGroup(_selectedCategory, _selectedCategoryGroup);

            OnPropertyChanged(nameof(SelectedCategory));
            OnPropertyChanged(nameof(SelectedCategoryGroup));
            OnPropertyChanged(nameof(SelectedGroupExample));
        }
    }
}