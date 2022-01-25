// *************************************************************************************
// SCICHART� Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FlyoutMenuButton.cs is part of SCICHART�, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using SciChart.Charting.Visuals.Shapes;
using SciChart.Core.Utility;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class FlyoutSeparator : ContentControl
    {
        public FlyoutSeparator()
        {
            this.DefaultStyleKey = typeof (FlyoutSeparator);
        }
    }

    [TemplatePart(Name = "PART_Popup", Type=typeof(Popup))]
    public class FlyoutMenuButton : Button
    {
        public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register(
            "PopupContent", typeof(object), typeof(FlyoutMenuButton), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty PopupAlignmentProperty =
            DependencyProperty.Register("PopupAlignment", typeof(PopupAlignment), typeof(FlyoutMenuButton), new PropertyMetadata(PopupAlignment.Left));

        private Popup _popup;
        private Border _border;
        private TimedMethod _popupCloseToken;
        private Storyboard _fadeStoryboard;
        private Grid _root;
        private Callout _callout;

        public FlyoutMenuButton()
        {
            this.DefaultStyleKey = typeof (FlyoutMenuButton);
        }

        public PopupAlignment PopupAlignment
        {
            get { return (PopupAlignment)GetValue(PopupAlignmentProperty); }
            set { SetValue(PopupAlignmentProperty, value); }
        }

        public object PopupContent
        {
            get { return (object)GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = GetTemplateChild("PART_Popup") as Popup;
            _border = GetTemplateChild("PART_Border") as Border;
            _root = GetTemplateChild("RootElement") as Grid;
            _callout = GetTemplateChild("Callout") as Callout;

            _popup.Placement = PlacementMode.Custom;
            _popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(placePopup);

            if (_root == null) return;
            _fadeStoryboard = ((Storyboard)_root.TryFindResource("FadeBorderAnimation"));

            if (_border == null || _callout == null || _popup == null) return;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                _popup.Visibility = Visibility.Visible;
            }

            _border.MouseEnter += (s, e) =>
            {
                if (_popupCloseToken != null)
                {
                    _popupCloseToken.Dispose();
                    _popupCloseToken = null;
                }
                if (!_popup.IsOpen)
                {
                    _popup.IsOpen = true;
                    _fadeStoryboard.Begin();
                }
            };

            _border.MouseLeave += (s, e) =>
            {
                _popupCloseToken = TimedMethod.Invoke(() =>
                {
                    _popup.IsOpen = false;
                    _fadeStoryboard.Stop();
                }).After(200).Go();
            };

            _callout.MouseEnter += (s, e) =>
            {
                if (_popupCloseToken != null)
                {
                    _popupCloseToken.Dispose();
                    _popupCloseToken = null;
                }
            };

            _callout.MouseLeave += (s, e) =>
            {
                _popupCloseToken = TimedMethod.Invoke(() =>
                {
                    _popup.IsOpen = false;
                    _fadeStoryboard.Stop();
                }).After(200).Go();
            };
        }

        public CustomPopupPlacement[] placePopup(Size popupSize, Size targetSize, Point offset)
        {
            return new[]
            {
                new CustomPopupPlacement(new Point(40, 0), PopupPrimaryAxis.Horizontal)
            };
        }
    }
}