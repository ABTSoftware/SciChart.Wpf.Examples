using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.Examples.Demo.ViewModels;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Examples.Demo.Helpers
{
    public interface IModule
    {
        ReadOnlyCollection<string> AllCategories { get; }
        IDictionary<string, ReadOnlyCollection<string>> GroupsByCategory { get; }
        IDictionary<Guid, Example> Examples { get; }
        Example CurrentExample { get; set; }
        void Initialize();
        IEnumerable<Example> ExamplesByCategoryAndGroup(string category, string group);
    }

    [ExportType(typeof(IModule), CreateAs.Singleton)]
    public class Module : IModule
    {
        public IDictionary<Guid, Example> _examples = new Dictionary<Guid, Example>();

        public static readonly Dictionary<Guid, AppPage> ChartingPages = new Dictionary<Guid, AppPage>();

        private readonly ActionCommand<Example> _navigateToExampleCommand;
        private IDictionary<string, ReadOnlyCollection<string>> _groupsByCategory;
        private ReadOnlyCollection<string> _allCategories;
        private Example _currentExample;

        public Module()
        {
            _navigateToExampleCommand = new ActionCommand<Example>(example =>
            {
                var lastExamplePage = CurrentExample != null ? CurrentExample.Page as ExampleAppPage : null;
                
                CurrentExample = example;
                Navigator.Instance.Navigate(example);
                Navigator.Instance.Push(example);

                if (lastExamplePage != null)
                {
                    // Required to release memory on example switch
                    lastExamplePage.ViewModel = null;                    
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            });
        }

        public Example CurrentExample
        {
            get { return _currentExample; }
            set
            {
                _currentExample = value;
                ServiceLocator.Container.Resolve<IExampleViewModel>().SelectedExample = value;
            }
        }

        public IDictionary<Guid, Example> Examples { get { return _examples; } }

        public ReadOnlyCollection<string> AllCategories { get { return _allCategories; } }
        public IDictionary<string, ReadOnlyCollection<string>> GroupsByCategory { get { return _groupsByCategory; } }

        private ICommand NavigateToExample
        {
            get { return _navigateToExampleCommand; }
        }

        public void Initialize()
        {
            var exampleDefinitions = LoadExampleDefinitions();
            InitializeExamplesAndPages(exampleDefinitions);
          
            InitializeNewNavigator();
        }

        public IEnumerable<Example> ExamplesByCategoryAndGroup(string category, string @group)
        {
            return _examples.Where(x => x.Value.Group == @group && x.Value.TopLevelCategory == category).Select(x => x.Value).ToList();
        }

        private static IEnumerable<ExampleDefinition> LoadExampleDefinitions()
        {
            var loader = new ExampleLoader();

            var xmlExamples = loader.DiscoverAllXmlFiles();
            IEnumerable<ExampleDefinition> exampleDefinitions = xmlExamples.Select(e => loader.Parse(e));

            return exampleDefinitions;
        }

        private void InitializeExamplesAndPages(IEnumerable<ExampleDefinition> exampleDefinitions)
        {
            AppPage appPage;
            HashSet<string> categories = new HashSet<string>();
            foreach (var definition in exampleDefinitions)
            {
                appPage = new ExampleAppPage(definition.Title ,definition.ViewModel, definition.View);
                ChartingPages.Add(appPage.PageId, appPage);

                var example = new Example(appPage, definition) {SelectCommand = NavigateToExample};

                _examples.Add(appPage.PageId, example);
                categories.Add(example.TopLevelCategory);
            }

            _allCategories = new ReadOnlyCollection<string>(categories.ToList());
            _groupsByCategory = new Dictionary<string, ReadOnlyCollection<string>>();
            foreach (var category in _allCategories)
            {
                var groups = _examples.Where(ex => ex.Value.TopLevelCategory == category).Select(y => y.Value.Group).Distinct().ToList();
                _groupsByCategory.Add(category, new ReadOnlyCollection<string>(groups));
            }

            appPage = new HomeAppPage();
            ChartingPages.Add(appPage.PageId, appPage);

            appPage = new EverythingAppPage();
            ChartingPages.Add(appPage.PageId, appPage);

            appPage = new ExamplesHostAppPage();
            ChartingPages.Add(appPage.PageId, appPage);
        }

        private static void InitializeNewNavigator()
        {
            Navigator.Instance.AfterNavigation = (view, page) => 
            {
                if (view != null && page is ExampleAppPage)
                {
                    view.DataContext = page.ViewModel;
                }
            };
        }
    }
}