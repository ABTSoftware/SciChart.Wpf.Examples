using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using SciChart.Examples.Demo.Helpers;
using SciChart.UI.Reactive.Observability;
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
                            g.GroupingName == value.Name &&
                            g.ParentGroupName == value.GroupName);

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
                        var node = FindNodeByName(ExampleNodes, groupToNavigate.GroupingName, groupToNavigate.ParentGroupName);

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
            var groupExamples = new ObservableCollection<TileViewModel>();

            Examples = module.Examples;

            var exampleNodes = new List<ExampleTreeNodeViewModel>();
            var topLevelCategories = module.Examples
                .GroupBy(e => e.Value.TopLevelCategory)
                .OrderBy(g => g.Key)
                .OrderBy(g => g.Key == "Featured Apps" ? 1 : 2);

            foreach (var topLevelCategory in topLevelCategories)
            {
                var topLevelNode = new ExampleTreeNodeViewModel(topLevelCategory.Key, null);

                var groups = topLevelCategory.GroupBy(e => e.Value.Group).OrderBy(g => g.Key);
                foreach (var group in groups)
                {
                    var sortedExamples = group.Select(x => x.Value).OrderBy(e => e.Title);

                    // Add groups to the all list and tree view
                    groupExamples.Add(new TileViewModel
                    {
                        TileDataContext = new EverythingGroupViewModel
                        {
                            ParentGroupName = topLevelNode.Name,
                            GroupingName = group.Key,
                            ExamplesCount = sortedExamples.Count()
                        }
                    });

                    var groupNode = new ExampleTreeNodeViewModel(group.Key, topLevelNode.Name);
                    groupNode.ShowExpander = true;

                    // Add examples into groups
                    foreach (var example in sortedExamples)
                    {
                        groupExamples.Add(new TileViewModel { TileDataContext = example });
                        groupNode.Children.Add(new ExampleTreeNodeViewModel(example.Title, groupNode.Name, example));
                    }

                    topLevelNode.Children.Add(groupNode);
                }
                exampleNodes.Add(topLevelNode);
            }
            ExampleNodes = exampleNodes;
            EverythingSource = groupExamples;
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