// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SciChart3DModifierButtonTemplateSelector.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Modifiers.Tooltip3D;
using SciChart.Examples.ExternalDependencies.Controls.Toolbar3D.CustomModifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChart3DInteractionToolbar
{
    public class SciChart3DModifierButtonTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyTemplate { get; set; }

        public DataTemplate FreeLookTemplate { get; set; }

        public DataTemplate OrbitTemplate { get; set; }

        public DataTemplate MouseWheelTemplate { get; set; }

        public DataTemplate CustomZoomExtentsTemplate { get; set; }

        public DataTemplate VertexSelectionTemplate { get; set; }

        public DataTemplate TooltipTemplate { get; set; }

        public DataTemplate LegendTemplate { get; set; }

        public DataTemplate CoordinateSystemTemplate { get; set; }

        public DataTemplate CameraModeTemplate { get; set; }

        public DataTemplate AxisLabelsOrientationTemplate { get; set; }

        public DataTemplate AxisTitleOrientationTemplate { get; set; }

        public DataTemplate AxisPlaneDrawLabelsTemplate { get; set; }

        public DataTemplate AxisPlaneDrawTitlesTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var mod = (SciChart3DInteractionToolbar.SciChart3DToolbarItem)item;

            if (mod != null)
            {
                var modifierType = mod.Modifier.GetType();

                if (modifierType == typeof(FreeLookModifier3D))
                    return FreeLookTemplate;

                if (modifierType == typeof(OrbitModifier3D))
                    return OrbitTemplate;

                if (modifierType == typeof(MouseWheelZoomModifier3D))
                    return MouseWheelTemplate;

                if (modifierType == typeof(ZoomExtentsModifier3D))
                    return CustomZoomExtentsTemplate;

                if (modifierType == typeof(VertexSelectionModifier3D))
                    return VertexSelectionTemplate;

                if (modifierType == typeof(TooltipModifier3D))
                    return TooltipTemplate;

                if (modifierType == typeof(LegendModifier3D))
                    return LegendTemplate;

                if (modifierType == typeof(CoordinateSystemModifier))
                    return CoordinateSystemTemplate;

                if (modifierType == typeof(CameraModeModifier))
                    return CameraModeTemplate;

                if (modifierType == typeof(AxisLabelsOrientationModifier))
                    return AxisLabelsOrientationTemplate;

                if (modifierType == typeof(AxisTitleOrientationModifier))
                    return AxisTitleOrientationTemplate;

                if (modifierType == typeof(AxisPlaneDrawLabelsModifier))
                    return AxisPlaneDrawLabelsTemplate;

                if (modifierType == typeof(AxisPlaneDrawTitlesModifier))
                    return AxisPlaneDrawTitlesTemplate;
            }

            return EmptyTemplate;
        }
    }
}
