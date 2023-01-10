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