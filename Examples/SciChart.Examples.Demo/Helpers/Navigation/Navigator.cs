using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Unity;
using SciChart.Charting.HistoryManagers;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using SciChart.Examples.Demo.ViewModels;
using SciChart.UI.Bootstrap;
using ActionCommand = SciChart.Charting.Common.Helpers.ActionCommand;

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
            private UserControl _view;
            public AppPage AppPage { get; set; }
            public UserControl View
            {
                get { return _view; }
                set
                {
                    var dView = _view as IDisposable;
                    if (dView != null)
                    {
                        // Disposes the view if it implements IDisposable
                        dView.Dispose();
                    }
                    _view = value;
                }
            }
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
        private readonly NavigationHistory _history;

        private readonly MainFrame _mainFrame;
        private readonly ExamplesFrame _examplesFrame;

        private ApplicationPageWrapper _currentExample;

        public AppPage CurrentPage { get; protected set; }
        public AppPage CurrentExample => _currentExample?.AppPage;

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
        }

        public bool CanNavigateTo(Guid pageId)
        {
            return _mainFrame.CanNavigateTo(Module.ChartingPages[pageId].Uri);
        }

        public bool CanNavigateTo(Example example)
        {
            return _examplesFrame.CanNavigateTo(Module.ChartingPages[example.PageId].Uri);
        }

        public ActionCommand GoBackCommand { get; }

        public ActionCommand GoForwardCommand { get; }

        public ActionCommand NavigateToHomeCommand { get; }

        public bool CanGoBack => _history.CanUndo();

        public bool CanGoForward => _history.CanRedo();

        public void GoBack()
        {
            var lastExampleView = _currentExample;

            _history.Undo();

            if (_history.Current is Example example)
            {
                Navigate(example);
                ServiceLocator.Container.Resolve<IModule>().CurrentExample = example;
            }
            else
            {
                Navigate((Guid)_history.Current);
            }

            ServiceLocator.Container.Resolve<IExampleViewModel>().BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
            ServiceLocator.Container.Resolve<IExampleViewModel>().ExportExampleViewModel.IsExportVisible = false;

            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
            NavigateToHomeCommand.RaiseCanExecuteChanged();
            if (CurrentExample is ExampleAppPage lastExamplePage)
            {
                // Required to release memory from last example 
                lastExamplePage.ViewModel = null;                
            }

            if (lastExampleView != null)
            {
                lastExampleView.View = null;
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void GoForward()
        {
            var lastExampleView = _currentExample;
            _history.Redo();

            if (_history.Current is Example example)
            {
                Navigate(example);
                ServiceLocator.Container.Resolve<IModule>().CurrentExample = example;
            }
            else
            {
                Navigate((Guid)_history.Current);
            }

            ServiceLocator.Container.Resolve<IExampleViewModel>().ExportExampleViewModel.IsExportVisible = false;
            ServiceLocator.Container.Resolve<IExampleViewModel>().BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;

            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
            NavigateToHomeCommand.RaiseCanExecuteChanged();
            if (CurrentExample is ExampleAppPage lastExamplePage)
            {
                // Required to release memory from last example 
                lastExamplePage.ViewModel = null;
            }

            if (lastExampleView != null)
            {
                lastExampleView.View = null;
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void OnExamplesFrameBeforeNavigation()
        {
            if (_currentExample != null)
            {
                _currentExample.View = null;

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
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

            _instance._mainFrame.Frame = navigationFrame;
            _instance._mainFrame.AfterNavigation = _instance.OnMainFrameAfterNavigation;

            _instance.Navigate(page);
            _instance._history.Push(AppPage.HomePageId);
        }

        private static void OnExampleStartPageAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var navigationFrame = (Frame)d;

            _instance._examplesFrame.Frame = navigationFrame;
            _instance._examplesFrame.AfterNavigation = _instance.OnExamplesFrameAfterNavigation;
            _instance._examplesFrame.BeforeNavigation = _instance.OnExamplesFrameBeforeNavigation;

            var page = GetExampleStartPage(navigationFrame);
            var module = ServiceLocator.Container.Resolve<IModule>();
            var example = module.Examples.Single(ex => ex.Key == page).Value;

            _instance.Navigate(example);
        }

        #region Singleton

        private static Navigator _instance;

        public static Navigator Instance => _instance ?? (_instance = new Navigator());

        private Navigator()
        {
            _usageCalculator = ServiceLocator.Container.Resolve<IUsageCalculator>();
            _history = new NavigationHistory();

            _mainFrame = new MainFrame();
            _examplesFrame = new ExamplesFrame();

            GoBackCommand = new ActionCommand(GoBack, () => CanGoBack);
            GoForwardCommand = new ActionCommand(GoForward, () => CanGoForward);
            NavigateToHomeCommand = new ActionCommand(() => Instance.GoToHome(), () => Instance.CurrentPage == null || Instance.CurrentPage.PageId != AppPage.HomePageId);
        }

        private void GoToHome()
        {
            var lastExampleView = _currentExample;

            Instance.Navigate(AppPage.HomePageId);
            _history.Push(AppPage.HomePageId);

            ServiceLocator.Container.Resolve<IExampleViewModel>().BreadCrumbViewModel.IsShowingBreadcrumbNavigation = false;
            ServiceLocator.Container.Resolve<IExampleViewModel>().ExportExampleViewModel.IsExportVisible = false;
            ServiceLocator.Container.Resolve<IMainWindowViewModel>().HideSearchCommand.Execute(null);

            // Clears memory on going to home
            ((TransitioningFrame)_examplesFrame.Frame).SetContentNull();
            if (CurrentExample is ExampleAppPage lastExamplePage)
            {
                // Required to release memory from last example 
                lastExamplePage.ViewModel = null;
            }

            if (lastExampleView != null)
            {
                lastExampleView.View = null;
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
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