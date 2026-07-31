// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomAspectRatioModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers
{
    /// <summary>
    /// Hosts the Dev-Mode toolbar's aspect-ratio control. Toggling it on installs an
    /// <see cref="AspectRatioViewportManager"/> on the parent surface, wrapping the surface's existing
    /// ViewportManager via <see cref="AspectRatioViewportManager.ChildViewportManager"/>, so any example
    /// can be made aspect-correct; the flyout edits the mode, anchor and ratio. When the example already
    /// supplies its own AspectRatioViewportManager, this drives that instance instead of wrapping it.
    /// </summary>
    public class CustomAspectRatioModifier : ChartModifierBase
    {
        private AspectRatioViewportManager _viewportManager;
        private bool _ownsViewportManager;

        private AspectRatioMode _aspectRatioMode = AspectRatioMode.Fit;
        private AspectRatioAnchor _anchor = AspectRatioAnchor.Center;
        private double _aspectRatio = 1.0;

        /// <summary>Gets or sets the aspect-ratio mode applied to the managed ViewportManager.</summary>
        public AspectRatioMode AspectRatioMode
        {
            get => _aspectRatioMode;
            set { _aspectRatioMode = value; ApplyToViewportManager(); }
        }

        /// <summary>Gets or sets the anchor applied to the managed ViewportManager.</summary>
        public AspectRatioAnchor Anchor
        {
            get => _anchor;
            set { _anchor = value; ApplyToViewportManager(); }
        }

        /// <summary>Gets or sets the data aspect ratio applied to the managed ViewportManager.</summary>
        public double AspectRatio
        {
            get => _aspectRatio;
            set { _aspectRatio = value; ApplyToViewportManager(); }
        }

        /// <inheritdoc/>
        public override void OnDetached()
        {
            Deactivate();
            base.OnDetached();
        }

        /// <inheritdoc/>
        protected override void OnIsEnabledChanged()
        {
            base.OnIsEnabledChanged();

            if (IsEnabled)
                Activate();
            else
                Deactivate();
        }

        private void Activate()
        {
            if (_viewportManager != null) return;

            var surface = ParentSurface as SciChartSurface;
            if (surface == null) return;

            if (surface.ViewportManager is AspectRatioViewportManager existing)
            {
                // The example manages aspect itself; drive that instance and adopt its current settings
                // so toggling this control does not overwrite the example's configuration.
                _viewportManager = existing;
                _ownsViewportManager = false;
                _aspectRatioMode = existing.AspectRatioMode;
                _anchor = existing.Anchor;
                _aspectRatio = existing.AspectRatio;
            }
            else
            {
                _viewportManager = new AspectRatioViewportManager
                {
                    AspectRatioMode = _aspectRatioMode,
                    Anchor = _anchor,
                    AspectRatio = _aspectRatio,
                    ChildViewportManager = surface.ViewportManager
                };
                _ownsViewportManager = true;
                surface.ViewportManager = _viewportManager;
            }

            surface.InvalidateElement();
        }

        private void Deactivate()
        {
            if (_viewportManager == null) return;

            var surface = ParentSurface as SciChartSurface;

            if (_ownsViewportManager && surface != null)
            {
                // Restore the surface's original ViewportManager (the wrapped child).
                surface.ViewportManager = _viewportManager.ChildViewportManager;
            }

            _viewportManager = null;
            _ownsViewportManager = false;
            surface?.InvalidateElement();
        }

        private void ApplyToViewportManager()
        {
            if (_viewportManager == null) return;

            _viewportManager.AspectRatioMode = _aspectRatioMode;
            _viewportManager.Anchor = _anchor;
            _viewportManager.AspectRatio = _aspectRatio;
        }
    }
}
