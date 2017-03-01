using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using SciChart.Charting.HistoryManagers;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using SciChart.Examples.Demo.ViewModels;
using SciChart.Wpf.UI.Bootstrap;
using SciChart.Wpf.UI.Reactive;

namespace SciChart.Examples.Demo.Helpers.Navigation
{
    public class NavigationHistory : HistoryStack<object>
    {
        public NavigationHistory()
        {
            Current = AppPage.HomePageId;
        }
    }

    public class Navigator
    {
        private class ApplicationPageWrapper
        {
            public AppPage AppPage { get; set; }
            public UserControl View { get; set; }
        }

        #region AttachedProperties

        public static readonly DependencyProperty StartPageProperty = DependencyProperty.RegisterAttached("StartPage", typeof(Guid), typeof(Navigator), new PropertyMetadata(OnStartPageAttached));

        public static void SetStartPage(DependencyObject element, Guid value)
        {
            element.SetValue(StartPageProperty, value);
        }

        public static Guid GetStartPage(DependencyObject element)
        {
            return (Guid)element.GetValue(StartPageProperty);
        }

        public static readonly DependencyProperty ExampleStartPageProperty = DependencyProperty.RegisterAttached("ExampleStartPage", typeof(Guid), typeof(Navigator), new PropertyMetadata(OnExampleStartPageAttached));

        public static void SetExampleStartPage(DependencyObject element, Guid value)
        {
            element.SetValue(ExampleStartPageProperty, value);
        }

        public static Guid GetExampleStartPage(DependencyObject element)
        {
            return (Guid)element.GetValue(ExampleStartPageProperty);
        }

        #endregion

        public const string ExampleNavigationPattern = AppConstants.ComponentPath + "Examples/{example}";
        public const string OldAppNavigationPattern = AppConstants.OldAppPath + "Examples/IWantTo/{example}";

        private readonly IUsageCalculator _usageCalculator;

        private NavigationHistory _history;

        private readonly MainFrame _mainFrame;
        private readonly ExamplesFrame _examplesFrame;

        private ApplicationPageWrapper _currentExample;

        public AppPage CurrentPage { get; protected set; }
        public AppPage CurrentExample { get { return _currentExample != null ? _currentExample.AppPage : null; } }

        public Action<UserControl, AppPage> AfterNavigation { get; set; }

        public void Navigate(Guid pageId)
        {
            var page = Module.ChartingPages[pageId];
            _mainFrame.Navigate(new Uri(page.Uri, UriKind.RelativeOrAbsolute));

            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
            NavigateToHomeCommand.RaiseCanExecuteChanged();
        }

        public void Navigate(Example example)
        {
#if SILVERLIGHT
            var page = Module.ChartingPages[example.PageId];
            _mainFrame.Navigate(new Uri(page.Uri, UriKind.RelativeOrAbsolute));
#else
            if (CurrentPage.PageId == AppPage.ExamplesPageId)
            {
                if (CanNavigateTo(example))
                {
                    var page = Module.ChartingPages[example.PageId];
                    _examplesFrame.Navigate(new Uri(page.Uri, UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                Navigate(AppPage.ExamplesPageId);
            }
#endif
        }

        public bool CanNavigateTo(Guid pageId)
        {
            return _mainFrame.CanNavigateTo(Module.ChartingPages[pageId].Uri);
        }

        public bool CanNavigateTo(Example example)
        {
            return _examplesFrame.CanNavigateTo(Module.ChartingPages[example.PageId].Uri);
        }

        public ActionCommand GoBackCommand
        {
            get { return _goBackCommand; }
        }

        public ActionCommand GoForwardCommand
        {
            get { return _goForwardCommand; }
        }

        public ActionCommand NavigateToHomeCommand
        {
            get { return _navigateToHomeCommand; }
        }

        public bool CanGoBack
        {
            get { return _history.CanUndo(); }
        }

        public bool CanGoForward
        {
            get { return _history.CanRedo(); }

        }

        public void GoBack()
        {
            var lastExamplePage = CurrentExample as ExampleAppPage;
            var lastExampleView = _currentExample;

            _history.Undo();

            var example = _history.Current as Example;
            if (example != null)
            {
                Navigate(example);
                ServiceLocator.Container.Resolve<IModule>().CurrentExample = example;
            }
            else
            {
                Navigate((Guid)_history.Current);
            }

            ServiceLocator.Container.Resolve<ExampleViewModel>().BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
            ServiceLocator.Container.Resolve<ExampleViewModel>().ExportExampleViewModel.IsExportVisible = false;

            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
            NavigateToHomeCommand.RaiseCanExecuteChanged();

            if (lastExamplePage != null)
            {
                // Required to release memory from last example 
                lastExamplePage.ViewModel = null;                
            }

            if (lastExampleView != null)
            {
                lastExampleView.View = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void GoForward()
        {
            var lastExamplePage = CurrentExample as ExampleAppPage;
            var lastExampleView = _currentExample;
            _history.Redo();

            var example = _history.Current as Example;
            if (example != null)
            {
                Navigate(example);
                ServiceLocator.Container.Resolve<IModule>().CurrentExample = example;
            }
            else
            {
                Navigate((Guid)_history.Current);
            }

            ServiceLocator.Container.Resolve<ExampleViewModel>().ExportExampleViewModel.IsExportVisible = false;
            ServiceLocator.Container.Resolve<ExampleViewModel>().BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;

            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
            NavigateToHomeCommand.RaiseCanExecuteChanged();

            if (lastExamplePage != null)
            {
                // Required to release memory from last example 
                lastExamplePage.ViewModel = null;
            }

            if (lastExampleView != null)
            {
                lastExampleView.View = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void OnExamplesFrameBeforeNavigation()
        {
            if (_currentExample != null)
            {
                _currentExample.View = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private void OnExamplesFrameAfterNavigation(Exception e, UserControl view)
        {
            _currentExample = _currentExample ?? (new ApplicationPageWrapper());
            _currentExample.View = view;

            if (view != null)
            {
                _currentExample.AppPage = GetCurrentExample();

                if (_currentExample.AppPage != null)
                {
                    _currentExample.AppPage.NewViewModel();

                    AfterNavigation(_currentExample.View, _currentExample.AppPage);
                }
            }
            else
            {
                _currentExample.AppPage = null;
            }

            _usageCalculator.UpdateUsage(CurrentExample);
        }

        private void OnMainFrameAfterNavigation()
        {
            CurrentPage = GetCurrentPage();

            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
            NavigateToHomeCommand.RaiseCanExecuteChanged();

            _usageCalculator.UpdateUsage(CurrentPage);
        }

        public AppPage GetCurrentExample()
        {
            var uri = _examplesFrame.GetCurrentSource();

            if (uri == null)
                return null;

            var firstOrDefault = Module.ChartingPages.Where(item => item.Value.Uri == uri.OriginalString)
                .Select(item => item.Value)
                .FirstOrDefault();
            return firstOrDefault;
        }

        public AppPage GetCurrentPage()
        {
            var uri = _mainFrame.GetCurrentSource();

            if (uri == null)
                return null;

            var firstOrDefault = Module.ChartingPages.Where(item => item.Value.Uri == uri.OriginalString)
                .Select(item => item.Value)
                .FirstOrDefault();
            return firstOrDefault;
        }

        private static void OnStartPageAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navigationFrame = (Frame)d;
            var page = GetStartPage(navigationFrame);

#if SILVERLIGHT
            navigationFrame.Navigated += NavigationFrameOnNavigated;
            navigationFrame.Navigating += NavigationFrameOnNavigating;
#endif
            _instance._mainFrame.Frame = navigationFrame;

            _instance._mainFrame.AfterNavigation = _instance.OnMainFrameAfterNavigation;
            _instance.Navigate(page);

            _instance._history.Push(AppPage.HomePageId);
        }

#if SILVERLIGHT
        private readonly UriMapper _oldAppRedirectionMapping = CreateOldAppMapping();

        private static UriMapper CreateOldAppMapping()
        {
            var mapping = new UriMapping { Uri = new Uri(ExampleNavigationPattern, UriKind.RelativeOrAbsolute), MappedUri = new Uri(OldAppNavigationPattern, UriKind.RelativeOrAbsolute) };
            var mapper = new UriMapper();
            mapper.UriMappings.Add(mapping);

            var examples = Module.ChartingPages.Where(x => x.Value is ExampleAppPage).Select(x => x.Value);
            foreach (var example in examples)
            {
                mapper.UriMappings.Add(new UriMapping
                {
                    Uri = mapping.MapUri(new Uri(example.MappedUri, UriKind.RelativeOrAbsolute)),
                    MappedUri = new Uri(example.Uri, UriKind.RelativeOrAbsolute),
                });
            }

            return mapper;
        }

        private static void NavigationFrameOnNavigating(object sender, NavigatingCancelEventArgs navigatingCancelEventArgs)
        {
            var navigationUri = _instance._oldAppRedirectionMapping.MapUri(navigatingCancelEventArgs.Uri);
            var navService = (NavigationService)sender;

            //Prevent navigation to the same source from old application twice
            if (navService.CurrentSource == navigationUri)
            {
                navigatingCancelEventArgs.Cancel = true;
                return;
            }

            var example = _instance.GetExampleByUri(navigationUri ?? navigatingCancelEventArgs.Uri);

            var module = ServiceLocator.Container.Resolve<Module>();
            var vm = ServiceLocator.Container.Resolve<MainWindowViewModel>();

            if ((example != null && module.CurrentExample != example))
            {
                navigatingCancelEventArgs.Cancel = true;

                example.SelectCommand.Execute(example);
                //vm.SelectedGroup = vm.Groups.FirstOrDefault(group => group.Group == example.Group);
            }
        }

        private Example GetExampleByUri(Uri uri)
        {
            Example result = null;

            if (uri != null)
            {
                var module = ServiceLocator.Container.Resolve<Module>();

                var pageToNavigate = Module.ChartingPages.Where(item => item.Value.Uri == uri.OriginalString)
                    .Select(item => item.Value)
                    .SingleOrDefault();

                if (pageToNavigate != null && module.Examples.ContainsKey(pageToNavigate.PageId))
                {
                    result = module.Examples[pageToNavigate.PageId];
                }
            }

            return result;
        }

        private static void NavigationFrameOnNavigated(object sender, NavigationEventArgs e)
        {
            // Redirect each example page to /ExamplesView
            // Pass example uri as parameter
            // Navigate to it here
            var uri = e.Uri;

            var example = _instance.GetExampleByUri(uri);

            if (example != null)
            {
                _instance._examplesFrame.Navigate(uri);
            }
        }
#endif

        private static void OnExampleStartPageAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navigationFrame = (Frame)d;

            _instance._examplesFrame.Frame = navigationFrame;
            _instance._examplesFrame.AfterNavigation = _instance.OnExamplesFrameAfterNavigation;
            _instance._examplesFrame.BeforeNavigation = _instance.OnExamplesFrameBeforeNavigation;
#if !SILVERLIGHT
            var page = GetExampleStartPage(navigationFrame);

            var module = ServiceLocator.Container.Resolve<IModule>();
            var example = module.Examples.Single(ex => ex.Key == page).Value;

            _instance.Navigate(example);
#endif
        }

        #region Singleton

        private static Navigator _instance;
        private readonly ActionCommand _goBackCommand;
        private readonly ActionCommand _goForwardCommand;
        private readonly ActionCommand _navigateToHomeCommand;

        public static Navigator Instance
        {
            get { return _instance ?? (_instance = new Navigator()); }
        }

        private Navigator()
        {
            _usageCalculator = ServiceLocator.Container.Resolve<IUsageCalculator>();
            _history = new NavigationHistory();

            _mainFrame = new MainFrame();
            _examplesFrame = new ExamplesFrame();
            _goBackCommand = new ActionCommand(GoBack, () => CanGoBack);
            _goForwardCommand = new ActionCommand(GoForward, () => CanGoForward);
            _navigateToHomeCommand = new ActionCommand(() => Instance.GoToHome(), () => Instance.CurrentPage == null || Instance.CurrentPage.PageId != AppPage.HomePageId);
        }

        private void GoToHome()
        {
            var lastExamplePage = CurrentExample as ExampleAppPage;
            var lastExampleView = _currentExample;

            Instance.Navigate(AppPage.HomePageId);
            _history.Push(AppPage.HomePageId);
            ServiceLocator.Container.Resolve<ExampleViewModel>().BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
            ServiceLocator.Container.Resolve<ExampleViewModel>().ExportExampleViewModel.IsExportVisible = false;
            ServiceLocator.Container.Resolve<MainWindowViewModel>().HideSearchCommand.Execute(null);

            // Clears memory on going to home
            ((TransitioningFrame)_examplesFrame.Frame).SetContentNull();

            if (lastExamplePage != null)
            {
                // Required to release memory from last example 
                lastExamplePage.ViewModel = null;
            }

            if (lastExampleView != null)
            {
                lastExampleView.View = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #endregion

        public void Push(Example example)
        {
            _history.Push(example);
        }
    }
}
