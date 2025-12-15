using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.Examples.Demo.ViewModels;
using SciChart.UI.Bootstrap;
using Unity;

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
        void ReloadCurrentExample();
    }

    [ExportType(typeof(IModule), CreateAs.Singleton)]
    public class Module : IModule
    {
        private Example _currentExample;

        public Module()
        {
            NavigateToExampleCommand = new ActionCommand<Example>(example =>
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

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced); //NOSONAR
                GC.WaitForPendingFinalizers();
                GC.Collect(); //NOSONAR
            });
        }

        public Example CurrentExample
        {
            get => _currentExample;
            set
            {
                _currentExample = value;
                ServiceLocator.Container.Resolve<IExampleViewModel>().SelectedExample = value;
            }
        }

        public ICommand NavigateToExampleCommand { get; }

        public static Dictionary<Guid, AppPage> ChartingPages { get; } = new Dictionary<Guid, AppPage>();

        public IDictionary<Guid, Example> Examples { get; } = new Dictionary<Guid, Example>();

        public ReadOnlyCollection<string> AllCategories { get; private set; }

        public IDictionary<string, ReadOnlyCollection<string>> GroupsByCategory { get; private set; }

        public void Initialize()
        {
            var exampleDefinitions = LoadExampleDefinitions();

            InitializeExamplesAndPages(exampleDefinitions);
          
            InitializeNewNavigator();
        }

        public IEnumerable<Example> ExamplesByCategoryAndGroup(string category, string @group)
        {
            return Examples.Where(x => x.Value.Group == @group && x.Value.TopLevelCategory == category).Select(x => x.Value).ToList();
        }

        private static IEnumerable<ExampleDefinition> LoadExampleDefinitions()
        {
            var loader = new ExampleLoader();
            var xmlExamples = loader.DiscoverAllXmlFiles();
           
            IEnumerable<ExampleDefinition> exampleDefinitions = xmlExamples.Select(loader.Parse);

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

                var example = new Example(appPage, definition) {SelectCommand = NavigateToExampleCommand};

                Examples.Add(appPage.PageId, example);
                categories.Add(example.TopLevelCategory);
            }

            AllCategories = new ReadOnlyCollection<string>(categories.ToList());
            GroupsByCategory = new Dictionary<string, ReadOnlyCollection<string>>();
            
            foreach (var category in AllCategories)
            {
                var groups = Examples.Where(ex => ex.Value.TopLevelCategory == category).Select(y => y.Value.Group).Distinct().ToList();
                GroupsByCategory.Add(category, new ReadOnlyCollection<string>(groups));
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

        public void ReloadCurrentExample()
        {
            var currentExample = CurrentExample;
            if (currentExample == null) return;

            if (Navigator.Instance.NavigateToHomeCommand.CanExecute(null))
            {
                Navigator.Instance.NavigateToHomeCommand.Execute(null);
            }

            NavigateToExampleCommand.Execute(currentExample);
        }
    }
}