using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity;
using SciChart.Examples.Demo.Behaviors;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Helpers.Grouping;
using SciChart.UI.Reactive.Observability;
using Unity.Attributes;

namespace SciChart.Examples.Demo.ViewModels
{
    public class EverythingViewModel : ViewModelWithTraitsBase
    {
        private List<IGrouping> _sortingGroups;

        public EverythingViewModel(IDictionary<Guid, Example> stubExamples)
        {
            Examples = stubExamples;
        }

        [InjectionConstructor]
        public EverythingViewModel()
        {
            WithTrait<PopulateExamplesBehaviour>();            
            SelectedGroupingMode = SortingGroups[0];
        }

        public IDictionary<Guid, Example> Examples
        {
            get => GetDynamicValue<IDictionary<Guid, Example>>(); 
            set => this.SetDynamicValue(value);
        }

        public ObservableCollection<TileViewModel> EverythingSource
        {
            get => GetDynamicValue<ObservableCollection<TileViewModel>>(); 
            set => this.SetDynamicValue(value);
        }

        public string SelectedCategory
        {
            get => this.GetDynamicValue<string>(); 
            set => this.SetDynamicValue(value);
        }

        public IGrouping SelectedGroupingMode
        {
            get => this.GetDynamicValue<IGrouping>(); 
            set
            {                
                this.SetDynamicValue(value);
            }
        }

        public List<IGrouping> SortingGroups
        {
            get
            {
                return _sortingGroups ?? (_sortingGroups = new List<IGrouping>
                {
                    new GroupingByCategory(),
                    new GroupingByFeature(),
                    new GroupingByName(),
                    new GroupingByMostUsed(),
                });
            }
        }

        public void UpdateEverythingSource(Func<IDictionary<Guid, Example>, ObservableCollection<TileViewModel>> groupingPredicate)
        {
            EverythingSource = groupingPredicate(Examples);
        }
    }
}