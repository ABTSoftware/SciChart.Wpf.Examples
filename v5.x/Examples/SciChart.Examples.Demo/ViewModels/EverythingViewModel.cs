using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using SciChart.Examples.Demo.Behaviors;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Helpers.Grouping;
using SciChart.Wpf.UI.Reactive.Observability;

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
            get { return GetDynamicValue<IDictionary<Guid, Example>>("Examples"); }
            set { SetDynamicValue("Examples", value); }
        }

        public ObservableCollection<TileViewModel> EverythingSource
        {
            get { return GetDynamicValue<ObservableCollection<TileViewModel>>("EverythingSource"); }
            set { SetDynamicValue("EverythingSource", value); }
        }

        public string SelectedCategory
        {
            get { return GetDynamicValue<string>("SelectedCategory"); }
            set { SetDynamicValue("SelectedCategory", value); }
        }

        public IGrouping SelectedGroupingMode
        {
            get { return GetDynamicValue<IGrouping>("SelectedGroupingMode"); }
            set
            {                
                SetDynamicValue("SelectedGroupingMode", value);
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