// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// FlyoutMenuButton.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.Visuals.Shapes;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class FlyoutSeparator : ContentControl
    {
        public FlyoutSeparator()
        {
            this.DefaultStyleKey = typeof(FlyoutSeparator);
        }
    }

    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class FlyoutMenuButton : Button
    {
        public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register
            (nameof(PopupContent), typeof(object), typeof(FlyoutMenuButton), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty PopupAlignmentProperty = DependencyProperty.Register
            (nameof(PopupAlignment), typeof(PopupAlignment), typeof(FlyoutMenuButton), new PropertyMetadata(PopupAlignment.Left));

        public static readonly DependencyProperty PopupOffsetProperty = DependencyProperty.Register
            (nameof(PopupOffset), typeof(double), typeof(FlyoutMenuButton), new PropertyMetadata(0d));

        private Popup _popup;
        private Border _border;

        private Grid _root;
        private Panel _panel;

        public FlyoutMenuButton()
        {
            this.DefaultStyleKey = typeof(FlyoutMenuButton);
        }
        public object PopupContent
        {
            get => GetValue(PopupContentProperty);
            set => SetValue(PopupContentProperty, value);
        }

        public PopupAlignment PopupAlignment
        {
            get => (PopupAlignment)GetValue(PopupAlignmentProperty);
            set => SetValue(PopupAlignmentProperty, value);
        }

        public double PopupOffset
        {
            get => (double)GetValue(PopupOffsetProperty);
            set => SetValue(PopupOffsetProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = GetTemplateChild("PART_Popup") as Popup;
            _border = GetTemplateChild("PART_Border") as Border;
            _root = GetTemplateChild("PART_Root") as Grid;
            _panel = GetTemplateChild("PART_Panel") as Panel;

            if (_root == null || _border == null || _panel == null || _popup == null) return;

            _popup.Placement = PlacementMode.Custom;
            _popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(PlacePopup);

            if (_panel.Background == null || _panel.Background == Brushes.Transparent)
            {
                _panel.Background = new SolidColorBrush { Color = Colors.Black, Opacity = 0.01 };
            }

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                _popup.HorizontalOffset = GetPopupOffset();

                _popup.Visibility = Visibility.Visible;
            }

            _panel.MouseDown += (s, e) => e.Handled = true;

            MouseLeave += (s, e) =>
            {
                if (_popup.IsOpen)
                {
                    _popup.IsOpen = false;
                }
            };

            MouseEnter += (s, e) =>
            {
                if (PopupContent != null && !_popup.IsOpen)
                {
                    _popup.HorizontalOffset = GetPopupOffset();

                    _popup.IsOpen = true;
                }
            };
        }

        protected override void OnClick()
        {
            var point = Mouse.GetPosition(this);

            if (point.X >= 0 && point.X <= ActualWidth &&
                point.Y >= 0 && point.Y <= ActualHeight)
            {
                base.OnClick();
            }
        }

        private double GetPopupOffset()
        {
            if (PopupAlignment == PopupAlignment.Left)
            {
                return PopupOffset;
            }

            if (PopupAlignment == PopupAlignment.Right)
            {
                return ActualWidth + PopupOffset;
            }

            return 0d;
        }

        private CustomPopupPlacement[] PlacePopup(Size popupSize, Size targetSize, Point offset)
        {
            return new[] { new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Horizontal) };
        }
    }
}