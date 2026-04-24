// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// FrameBase.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public abstract class FrameBase
    {
        private Frame _frame;

        public Frame Frame
        {
            get { return _frame; }
            set
            {
                _frame = value;
#if SILVERLIGHT
                _frame.UriMapper = CreateUriMapper();
#endif
            }
        }

#if SILVERLIGHT
        protected const string ExampleNavigationPattern = AppConstants.ComponentPath + "Examples/{example}";

        protected abstract UriMapper CreateUriMapper();
#endif

        public abstract void Navigate(Uri uri);

        public abstract void GoBack();

        public abstract void GoForward();

        public bool CanGoBack()
        {
            return _frame != null && _frame.CanGoBack;
        }

        public bool CanGoForward()
        {
            return _frame != null && _frame.CanGoForward;
        }



        public bool CanNavigateTo(string uri)
        {
            var isCurrent = IsCurrentSource(uri);
            return Frame != null && !isCurrent;
        }

        private bool IsCurrentSource(string uri)
        {
            return Frame != null &&
                   Frame.CurrentSource != null &&
                   Frame.CurrentSource.OriginalString == uri;
        }

        public Uri GetCurrentSource()
        {
            Uri uri = null;

            if (Frame != null)
            {
                uri = Frame.CurrentSource;
            }

            return uri;
        }
    }
}