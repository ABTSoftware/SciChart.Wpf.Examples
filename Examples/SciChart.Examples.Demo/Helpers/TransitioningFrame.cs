// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// TransitioningFrame.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Helpers
{
    [TemplateVisualState(GroupName = "TransitionStates", Name = "Normal")]
    [TemplateVisualState(GroupName = "TransitionStates", Name = "Transition")]
    public class TransitioningFrame : Frame
    {
        private ContentPresenter _currentContentPresentationSite;

        private ContentPresenter _previousContentPresentationSite;

        public TransitioningFrame()
        {
            DefaultStyleKey = typeof(TransitioningFrame);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _previousContentPresentationSite = GetTemplateChild("PreviousContentPresentationSite") as ContentPresenter;

            _currentContentPresentationSite = GetTemplateChild("CurrentContentPresentationSite") as ContentPresenter;

            if (_currentContentPresentationSite != null)
            {
                _currentContentPresentationSite.Content = Content;
            }
        }

        public void SetContentNull()
        {
            if (_currentContentPresentationSite != null)
                _currentContentPresentationSite.Content = null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if ((_currentContentPresentationSite != null) && (_previousContentPresentationSite != null))
            {
                _currentContentPresentationSite.Content = newContent;

                // Attempting to optimise memory 
                // 

                //_previousContentPresentationSite.Content = oldContent;
                //_previousContentPresentationSite.IsHitTestVisible = false;

                if (!App.UIAutomationTestMode)
                {
                    VisualStateManager.GoToState(this, "Normal", false);
                    VisualStateManager.GoToState(this, "Transition", true);
                }
            }
        }
    }
}
