using SciChart.Examples.Demo.Helpers;
using SciChart.UI.Reactive.Observability;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using SciChart.Examples.Demo.Helpers.Grouping;
using Unity;

namespace SciChart.Examples.Demo.ViewModels
{
    public class EverythingViewModel : ViewModelWithTraitsBase
    {
        public EverythingViewModel(IDictionary<Guid, Example> stubExamples)
        {
            Examples = stubExamples;
        }

        [InjectionConstructor]
        public EverythingViewModel(IModule module)
        {
            SetItemSources(module);
        }

        public IDictionary<Guid, Example> Examples
        {
            get => GetDynamicValue<IDictionary<Guid, Example>>();
            set => SetDynamicValue(value);
        }

        public ExampleTreeNodeViewModel SelectedExampleNode
        {
            get => GetDynamicValue<ExampleTreeNodeViewModel>();
            set
            {
                if (value != null && SelectedExampleNode != value)
                {
                    if (value.Example == null)
                    {
                        var navigatedItem = EverythingSource.FirstOrDefault(t =>
                            t.TileDataContext is EverythingGroupViewModel g &&
                            g.SubcategoryName == value.Name &&
                            g.CategoryName == value.GroupName);

                        SetDynamicValue(navigatedItem, nameof(NavigatedTileItem));
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                            () => value.Example?.SelectCommand.Execute(value.Example));
                    }
                }

                SetDynamicValue(value);
            }
        }

        public List<ExampleTreeNodeViewModel> ExampleNodes
        {
            get => GetDynamicValue<List<ExampleTreeNodeViewModel>>();
            set => SetDynamicValue(value);
        }

        public ObservableCollection<TileViewModel> EverythingSource
        {
            get => GetDynamicValue<ObservableCollection<TileViewModel>>();
            set => SetDynamicValue(value);
        }

        public TileViewModel NavigatedTileItem
        {
            get => GetDynamicValue<TileViewModel>();
            set
            {
                if (value != null && NavigatedTileItem != value)
                {
                    if (value.TileDataContext is EverythingGroupViewModel groupToNavigate)
                    {
                        var node = FindNodeByName(ExampleNodes, groupToNavigate.SubcategoryName, groupToNavigate.CategoryName);

                        SetDynamicValue(node, nameof(SelectedExampleNode));
                    }
                    else if (value.TileDataContext is Example example)
                    {
                        var node = FindNodeByName(ExampleNodes, example.Group, example.TopLevelCategory);

                        SetDynamicValue(node, nameof(SelectedExampleNode));
                    }
                }

                SetDynamicValue(value);
            }
        }

        public void CleanSelectedNode()
        {
            NavigatedTileItem = null;
            SelectedExampleNode = ExampleNodes.First().Children.First();
        }

        private void SetItemSources(IModule module)
        {
            Examples = module.Examples;
            var tileViewModels = new ObservableCollection<TileViewModel>();
            var treeViewNodeViewModels = new List<ExampleTreeNodeViewModel>();

            var comparer = new CategoryComparer();
            var categories = module.Examples
                .GroupBy(e => e.Value.TopLevelCategory)
                .OrderBy(g => g.Key, comparer);
            foreach (var category in categories)
            {
                var subcategories = category
                    .GroupBy(e => e.Value.Group)
                    .OrderBy(g=>g.Key, comparer);

                var categoryNodeViewModel = new ExampleTreeNodeViewModel(category.Key, null);
                foreach (var subcategory in subcategories)
                {
                    var examples = subcategory.Select(x => x.Value).ToList();

                    // Add groups to the all list and tree view
                    tileViewModels.Add(new TileViewModel
                    {
                        TileDataContext = new EverythingGroupViewModel
                        {
                            CategoryName = category.Key,
                            SubcategoryName = subcategory.Key,
                            ExamplesCount = examples.Count
                        }
                    });

                    var subcategoryNodeViewModel = new ExampleTreeNodeViewModel(subcategory.Key, category.Key);
                    subcategoryNodeViewModel.ShowExpander = true;

                    // Add examples for every subcategory
                    foreach (var example in examples)
                    {
                        tileViewModels.Add(new TileViewModel { TileDataContext = example });
                        subcategoryNodeViewModel.Children.Add(new ExampleTreeNodeViewModel(example.Title, subcategoryNodeViewModel.Name, example));
                    }

                    categoryNodeViewModel.Children.Add(subcategoryNodeViewModel);
                }
                treeViewNodeViewModels.Add(categoryNodeViewModel);
            }
            ExampleNodes = treeViewNodeViewModels;
            EverythingSource = tileViewModels;
        }

        private static ExampleTreeNodeViewModel FindNodeByName(IEnumerable<ExampleTreeNodeViewModel> nodes, string name, string groupName)
        {
            foreach (var node in nodes)
            {
                if (node.Name == name && node.GroupName == groupName)
                {
                    return node;
                }

                var result = FindNodeByName(node.Children, name, groupName);

                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}