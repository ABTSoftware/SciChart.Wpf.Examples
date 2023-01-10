// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Modifiers.Tooltip3D;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChart3DInteractionToolbar
{
    public class SciChart3DModifierButtonTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FreeLookModifierTemplate { get; set; }
        public DataTemplate OrbitModifierTemplate { get; set; }
        public DataTemplate MouseWheel3DModifierTemplate { get; set; }
        public DataTemplate CustomZoomExtentsModifier3DTemplate { get; set; }
        public DataTemplate VertexSelectionModifier3DTemplate { get; set; }
        public DataTemplate TooltipModifier3DTemplate { get; set; }
        public DataTemplate LegendModifier3DTemplate { get; set; }

        public DataTemplate EmptyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var mod = (SciChart3DInteractionToolbar.SciChart3DToolbarItem)item;

            if (mod != null)
            {
                var modifierType = mod.Modifier.GetType();

                if (modifierType == typeof(FreeLookModifier3D)) return FreeLookModifierTemplate;
                if (modifierType == typeof(OrbitModifier3D)) return OrbitModifierTemplate;
                if (modifierType == typeof(MouseWheelZoomModifier3D)) return MouseWheel3DModifierTemplate;
                if (modifierType == typeof(ZoomExtentsModifier3D)) return CustomZoomExtentsModifier3DTemplate;
                if (modifierType == typeof(VertexSelectionModifier3D)) return VertexSelectionModifier3DTemplate;
                if (modifierType == typeof(TooltipModifier3D)) return TooltipModifier3DTemplate;
                if (modifierType == typeof(LegendModifier3D)) return LegendModifier3DTemplate;
            }
            return EmptyTemplate;
        }
    }
}
