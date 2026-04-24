// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PopupPlacementBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.ExternalDependencies.Behaviors
{
    public class PopupPlacementBehavior : Behavior<Popup>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Placement = PlacementMode.Custom;

            AssociatedObject.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(PlacePopup);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.CustomPopupPlacementCallback = null;
        }

        private CustomPopupPlacement[] PlacePopup(Size popupSize, Size targetSize, Point offset)
        {
            return new[] { new CustomPopupPlacement(new Point(0, 0), PopupPrimaryAxis.Vertical) };
        }
    }
}