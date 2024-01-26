// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ModifierButtonTemplateSelector.cs is part of SCICHART®, High Performance Scientific Charts
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
using SciChart.Charting.ChartModifiers;
using SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D
{
    public class ModifierButtonTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RolloverModifierTemplate { get; set; }
        public DataTemplate MouseWheelZoomModifierTemplate { get; set; }
        public DataTemplate TooltipModifierTemplate { get; set; }
        public DataTemplate SeriesSelectionModifierTemplate { get; set; }
        public DataTemplate LegendModifierTemplate { get; set; }
        public DataTemplate CursorModifierTemplate { get; set; }
        public DataTemplate AnnotationCreationModifierTemplate { get; set; }
        public DataTemplate ZoomPanModifierTemplate { get; set; }
        public DataTemplate YAxisDragModifierTemplate { get; set; }
        public DataTemplate XAxisDragModifierTemplate { get; set; }
        public DataTemplate ZoomExtentsModifierTemplate { get; set; }
        public DataTemplate FlipModifierTemplate { get; set; }
        public DataTemplate ExportModifierTemplate { get; set; }
        public DataTemplate ThemeModifierTemplate { get; set; }
        public DataTemplate RotateChartModifierTemplate { get; set; }
        public DataTemplate RubberBandXyZoomModifierTemplate { get; set; }
        public DataTemplate FlyoutSeparatorTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }
        public DataTemplate PointMarkersModifierTemplate { get; set; }
        public DataTemplate AnimationsModifierTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var mod = (SciChartInteractionToolbar.ToolbarItem)item;
            
            if (mod != null)
            {
                switch (mod.Modifier.ModifierName)
                {
                    case nameof(RolloverModifier):
                        return RolloverModifierTemplate;

                    case nameof(MouseWheelZoomModifier):
                        return MouseWheelZoomModifierTemplate;

                    case nameof(TooltipModifier):
                        return TooltipModifierTemplate;

                    case nameof(SeriesSelectionModifier):
                        return SeriesSelectionModifierTemplate;

                    case nameof(LegendModifier):
                        return LegendModifierTemplate;

                    case nameof(CursorModifier):
                        return CursorModifierTemplate;

                    case nameof(CustomAnnotationCreationModifier):
                        return AnnotationCreationModifierTemplate;

                    case nameof(ZoomPanModifier):
                        return ZoomPanModifierTemplate;

                    case nameof(YAxisDragModifier):
                        return EmptyTemplate;

                    case nameof(XAxisDragModifier):
                        return EmptyTemplate;

                    case nameof(ZoomExtentsModifier):
                        return ZoomExtentsModifierTemplate;

                    case nameof(CustomFlipModifier):
                        return FlipModifierTemplate;

                    case nameof(CustomThemeChangeModifier):
                        return ThemeModifierTemplate;

                    case nameof(CustomRotateChartModifier):
                        return RotateChartModifierTemplate;

                    case nameof(RubberBandXyZoomModifier):
                        return RubberBandXyZoomModifierTemplate;

                    case nameof(DataPointSelectionModifier):
                        return PointMarkersModifierTemplate;

                    case nameof(CustomExportModifier):
                        return ExportModifierTemplate;

                    case nameof(SeriesAnimationCustomModifier):
                        return AnimationsModifierTemplate;

                    default:
                        return EmptyTemplate;
                }
            }
            return EmptyTemplate;

        }
    }
}
