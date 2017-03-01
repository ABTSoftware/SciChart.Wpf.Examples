using System;
using System.Text.RegularExpressions;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Demo.Helpers
{
    public abstract class AppPage
    {
        protected Type _viewModelType;
        private BaseViewModel _viewModel;

        static AppPage()
        {
            HomePageId = Guid.NewGuid();
            ChartingPageId = Guid.NewGuid();
            ExamplesPageId = Guid.NewGuid();
            EverythingPageId = Guid.NewGuid();
        }
     
        public string Uri { get; protected set; }
#if SILVERLIGHT
        public string MappedUri { get; protected set; }
#endif
        public BaseViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    NewViewModel();

                return _viewModel;
            }
            internal set { _viewModel = value; }
        }

        public void NewViewModel()
        {
            _viewModel = _viewModelType != null ? (BaseViewModel)Activator.CreateInstance(_viewModelType) : null;
        }

        public Guid PageId { get; protected set; }

        public static Guid HomePageId { get; protected set; }
        public static Guid ChartingPageId { get; protected set; }
        public static Guid ExamplesPageId { get; protected set; }
        public static Guid EverythingPageId { get; protected set; }
    }

    public class ExampleAppPage : AppPage
    {
        public string Title { get; set; }

        public ExampleAppPage(string title, string viewModel, string view)
        {
            Title = title;

            var examplesAssembly = typeof(ExampleLoader).Assembly;

            string typeName = AppConstants.AssemblyName + viewModel;
            _viewModelType = Type.GetType(typeName + ", " + examplesAssembly.FullName);
#if !SILVERLIGHT
            Uri = AppConstants.ComponentPath + view;
#else
            Uri = string.Format("/Examples/{0}",Regex.Replace(title, @"[^A-Za-z0-9]+", string.Empty));
            MappedUri = AppConstants.ComponentPath + view;
#endif     
            PageId = Guid.NewGuid();
        }

        //TODO Remove later, added for testing purpose
        public override string ToString()
        {
            return string.Format("Page : {0}", Title);
        }
    }

    public abstract class ApplicationAppPage : AppPage
    {
        protected abstract string View { get; }
        protected abstract string DesiredUri { get; }
        protected abstract Guid PageGuid { get; }

        protected ApplicationAppPage()
        {
#if !SILVERLIGHT
            Uri = AppConstants.DemoComponentPath + View;
#else
            Uri = DesiredUri;
            MappedUri = AppConstants.DemoComponentPath + View;
#endif
            PageId = PageGuid;      
        }
    }

    public class HomeAppPage : ApplicationAppPage
    {
        protected override string View { get { return "Views/HomeView.xaml"; } }
        protected override string DesiredUri { get { return "/Home"; } }
        protected override Guid PageGuid { get { return HomePageId; } }
    }
       
    public class EverythingAppPage : ApplicationAppPage
    {
        protected override string View { get { return "Views/EverythingView.xaml"; } }
        protected override string DesiredUri { get { return "/Everything"; } }
        protected override Guid PageGuid { get { return EverythingPageId; } }
    }
   

    public class ExamplesHostAppPage : ApplicationAppPage
    {
        protected override string View { get { return "Views/ExampleView.xaml"; } }
        protected override string DesiredUri { get { return "/Example"; } }
        protected override Guid PageGuid { get { return ExamplesPageId; } }
    }  
}
