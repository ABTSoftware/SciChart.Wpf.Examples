// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SciChart3DModifierButtonTemplateSelector.cs is part of SCICHART®, High Performance Scientific Charts
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
