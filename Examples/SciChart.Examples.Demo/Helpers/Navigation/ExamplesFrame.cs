// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ExamplesFrame.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SciChart.Examples.Demo.Helpers.Navigation
{
    public class ExamplesFrame : FrameBase
    {
        public Action<Exception, UserControl> AfterNavigation { get; set; }
        public Action BeforeNavigation { get; set; }

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
            NavigatingCancelEventHandler beforeHandler = null;

            successHandler = (s, e) =>
            {
                Frame.Navigated -= successHandler;
                Frame.NavigationFailed -= failureHandler;

                AfterNavigation(null, e.Content as UserControl);
            };

            failureHandler = (s, e) =>
            {
                Frame.Navigated -= successHandler;
                Frame.NavigationFailed -= failureHandler;

                AfterNavigation(e.Exception, null);
            };

            beforeHandler = (s, e) =>
            {
                Frame.Navigating -= beforeHandler;

                BeforeNavigation();
            };

            Frame.Navigated += successHandler;
            Frame.NavigationFailed += failureHandler;
            Frame.Navigating += beforeHandler;
        }

#if SILVERLIGHT
        protected override UriMapper CreateUriMapper()
        {
            var mapper = new UriMapper();

            //Mapps all examples to desired URIs
            foreach (var page in Module.ChartingPages)
            {
                mapper.UriMappings.Add(new UriMapping
                {
                    Uri = new Uri(page.Value.Uri, UriKind.RelativeOrAbsolute),
                    MappedUri = new Uri(page.Value.MappedUri, UriKind.RelativeOrAbsolute)
                });
            }

            return mapper;
        }
#endif
    }
}