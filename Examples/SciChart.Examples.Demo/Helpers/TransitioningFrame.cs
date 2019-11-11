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

                if (!App.QuickStart)
                {
                    VisualStateManager.GoToState(this, "Normal", false);
                    VisualStateManager.GoToState(this, "Transition", true);
                }
            }
        }
    }
}
