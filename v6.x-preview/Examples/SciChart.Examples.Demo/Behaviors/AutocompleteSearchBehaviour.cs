using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using SciChart.Core.Extensions;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Search;
using SciChart.Examples.Demo.ViewModels;
using SciChart.UI.Reactive;
using SciChart.UI.Reactive.Async;
using SciChart.UI.Reactive.Observability;
using SciChart.UI.Reactive.Traits;
using SciChart.Wpf.UI.Transitionz;

namespace SciChart.Examples.Demo.Behaviors
{
    public class AutoCompleteSearchBehaviour : ViewModelTrait<MainWindowViewModel>
    {
        private readonly IExampleSearchProvider _searchProvider;
        private readonly IModule _module;

        private readonly IBlurParams _defaultParams = new BlurParams { Duration = 200, From = 10, To = 0 };
        private readonly IBlurParams _blurredParams = new BlurParams { Duration = 200, From = 0, To = 10 };

        public AutoCompleteSearchBehaviour(MainWindowViewModel target, ISchedulerContext schedulerContext, IExampleSearchProvider searchProvider, IModule module)
            : base(target)
        {
            _searchProvider = searchProvider;
            _module = module;

            var searchTextObs = Target.WhenPropertyChanged(x => x.SearchText);
            searchTextObs.Skip(1).Subscribe(UpdateBlurEffect).DisposeWith(this);

            searchTextObs.Where(x => !String.IsNullOrEmpty(x))
                .Throttle(TimeSpan.FromMilliseconds(200), schedulerContext.Default)
                .Select(SearchExamples)
                .ObserveOn(schedulerContext.Dispatcher)
                .Subscribe(UpdateSearchResults)
                .DisposeWith(this);
        }

        private void UpdateBlurEffect(string text)
        {
            Target.BlurOnSearchParams = string.IsNullOrEmpty(text) ? _defaultParams : _blurredParams;
        }

        private IEnumerable<SearchExampleViewModel> SearchExamples(string searchText)
        {
            //Example.Title treated as the most relevant property
            var examplesByTitle = _module.Examples.Where(x => x.Value.Title.Contains(searchText)).Select(x => new ExampleId{Id = x.Key});

// ReSharper disable PossibleMultipleEnumeration
            var examplesBySearch = _searchProvider.Query(searchText);
            examplesBySearch = examplesBySearch != null
                ? examplesBySearch.Except(examplesByTitle)
                : examplesByTitle;

            var examples = examplesByTitle.Concat(examplesBySearch);
// ReSharper restore PossibleMultipleEnumeration

            return examples.Select(key => new SearchExampleViewModel { Example = _module.Examples[key.Id] }).ToList();
        }

        private void UpdateSearchResults(IEnumerable<SearchExampleViewModel> examples)
        {            
            if (examples != null)
            {
                Target.SearchResults = new ObservableCollection<ISelectable>(examples);
            }
            else
            {
                Target.SearchResults.Clear();
            }
            Target.HasSearchResults = !Target.SearchResults.IsNullOrEmpty();
        }
    }
}