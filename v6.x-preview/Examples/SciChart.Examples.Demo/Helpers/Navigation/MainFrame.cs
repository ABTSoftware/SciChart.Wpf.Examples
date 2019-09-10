using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SciChart.Examples.Demo.Helpers.Navigation
{
    public class MainFrame : FrameBase
    {
        public Action AfterNavigation { get; set; }
      
        public override void Navigate(Uri uri)
        {
            if (Frame != null)
            {
                AddHandler();
                Frame.Navigate(uri);
            }
        }

        public override void GoBack()
        {
            if (Frame != null)
            {
                AddHandler();
                Frame.GoBack();
            }
        }

        public override void GoForward()
        {
            if (Frame != null)
            {
                AddHandler();
                Frame.GoForward();
            }
        }

        private void AddHandler()
        {
            NavigatedEventHandler successHandler = null;
            NavigationFailedEventHandler failureHandler = null;

            successHandler = (s, e) =>
            {
                Frame.Navigated -= successHandler;
                Frame.NavigationFailed -= failureHandler;

                AfterNavigation();
            };

            failureHandler = (s, e) =>
            {
                Frame.Navigated -= successHandler;
                Frame.NavigationFailed -= failureHandler;

                AfterNavigation();
            };

            Frame.Navigated += successHandler;
            Frame.NavigationFailed += failureHandler;
        }

#if SILVERLIGHT
        protected override UriMapper CreateUriMapper()
        {
            var mapper = new UriMapper();

            //Mapps "" to HomeView
            mapper.UriMappings.Add(new UriMapping
            {
                Uri = new Uri("", UriKind.RelativeOrAbsolute),
                MappedUri = new Uri(Module.ChartingPages[AppPage.HomePageId].Uri, UriKind.RelativeOrAbsolute)
            });

            //Mapps pages to desired URIs
            var applicationPages = Module.ChartingPages.Where(x => x.Value is ApplicationAppPage).Select(x => x.Value);
            foreach (var page in applicationPages)
            {
                mapper.UriMappings.Add(new UriMapping
                {
                    Uri = new Uri(page.Uri, UriKind.RelativeOrAbsolute),
                    MappedUri = new Uri(page.MappedUri, UriKind.RelativeOrAbsolute)
                }); 

                //This is for browser navigation, so "/Url" and "/Url/" both works
                mapper.UriMappings.Add(new UriMapping
                {
                    Uri = new Uri(string.Format("{0}/",page.Uri), UriKind.RelativeOrAbsolute),
                    MappedUri = new Uri(page.MappedUri, UriKind.RelativeOrAbsolute)
                });
            }

            //Mapps all examples to ExamplesView so main frame won't navigate to particular example, but to ExampleView, and stay on it.
            var examplePages = Module.ChartingPages.Where(x => x.Value is ExampleAppPage).Select(x => x.Value);
            foreach (var page in examplePages)
            {
                mapper.UriMappings.Add(new UriMapping
                {
                    Uri = new Uri(page.Uri, UriKind.RelativeOrAbsolute),
                    MappedUri = new Uri(Module.ChartingPages[AppPage.ExamplesPageId].MappedUri, UriKind.RelativeOrAbsolute)
                });
            }

            return mapper;
        }
#endif
    }
}