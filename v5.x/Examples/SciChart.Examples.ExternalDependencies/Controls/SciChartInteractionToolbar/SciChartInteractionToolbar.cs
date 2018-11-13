// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SciChartInteractionToolbar.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar.CustomModifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar
{
    /// <summary>
    /// A toolbar used in examples to simplify zoom, pan, zoom extents, rollover, cursor etc... This also helps us with
    /// testing ;-)
    /// </summary>
    [ContentProperty("ExtraContent")]
    [TemplatePart(Name = "PART_Container", Type = typeof (Border))]
    public class SciChartInteractionToolbar : ContentControl
    {
        public class ToolbarItem : INotifyPropertyChanged
        {
            private IChartModifier _modifier;

            public IChartModifier Modifier
            {
                get { return _modifier; }
                set
                {
                    _modifier = value;
                    OnPropertyChanged("Modifier");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class CursorModifierToolbarItem : ToolbarItem
        {
            public ActionCommand<BaseRenderableSeries> SnapToSeriesCommand
            {
                get
                {
                    return new ActionCommand<BaseRenderableSeries>(s =>
                    {
                        if (s != null)
                        {
                            var allSeries = Modifier.ParentSurface.RenderableSeries;
                            foreach (var series in allSeries.OfType<BaseRenderableSeries>())
                            {
                                CursorModifier.SetSnapToSeries(series, false);
                            }

                            CursorModifier.SetSnapToSeries(s, true);
                        }
                    });
                }
            }
        }

        public static readonly DependencyProperty AppearceInToolbarProperty =
            DependencyProperty.RegisterAttached("AppearceInToolbar",
                typeof (bool),
                typeof (SciChartInteractionToolbar),
                new PropertyMetadata(true));

        public static void SetAppearceInToolbar(ChartModifierBase element, String value)
        {
            element.SetValue(AppearceInToolbarProperty, value);
        }

        public static bool GetAppearceInToolbar(ChartModifierBase element)
        {
            return (bool) element.GetValue(AppearceInToolbarProperty);
        }

        public static readonly DependencyProperty IsZoomXAxisOnlyProperty = DependencyProperty.Register(
            "IsZoomXAxisOnly", typeof (bool), typeof (SciChartInteractionToolbar), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty TargetSurfaceProperty =
            DependencyProperty.Register("TargetSurface", typeof (ISciChartSurface),
                typeof (SciChartInteractionToolbar),
                new PropertyMetadata(default(ISciChartSurface), OnTargetSurfaceDependencyPropertyChanged));

        public static readonly DependencyProperty ExtraContentProperty =
            DependencyProperty.Register("ExtraContent", typeof (List<ContentControl>),
                typeof (SciChartInteractionToolbar), new PropertyMetadata(OnExtraContentChanged));

        public static readonly DependencyProperty IsDeveloperModeProperty =
            DependencyProperty.Register("IsDeveloperMode", typeof (bool),
                typeof (SciChartInteractionToolbar),
                new PropertyMetadata(default(bool), OnIsDeveloperModeChanged));

        public static readonly DependencyProperty ModifiersSourceProperty =
            DependencyProperty.Register("ModifiersSource", typeof (ICollection<ToolbarItem>),
                typeof (SciChartInteractionToolbar));

        private WrapPanel _toolBarWrapPanel;
        private ModifierGroup _modifiersInDevMode;
        private ModifierGroup _modifiersInUserMode;
        private ModifierGroup _modifiersInAllMode;

        public SciChartInteractionToolbar()
        {
            DefaultStyleKey = typeof (SciChartInteractionToolbar);

            ExtraContent = new List<ContentControl>();
            _modifiersInDevMode = new ModifierGroup();
            _modifiersInUserMode = new ModifierGroup();
            _modifiersInAllMode = new ModifierGroup();
        }

        public bool IsZoomXAxisOnly
        {
            get { return (bool) GetValue(IsZoomXAxisOnlyProperty); }
            set { SetValue(IsZoomXAxisOnlyProperty, value); }
        }

        public ICollection<ToolbarItem> ModifiersSource
        {
            get { return (ICollection<ToolbarItem>) GetValue(ModifiersSourceProperty); }
            set { SetValue(ModifiersSourceProperty, value); }
        }

        public List<ContentControl> ExtraContent
        {
            get { return (List<ContentControl>) GetValue(ExtraContentProperty); }
            set { SetValue(ExtraContentProperty, value); }
        }

        public ISciChartSurface TargetSurface
        {
            get { return (ISciChartSurface) GetValue(TargetSurfaceProperty); }
            set { SetValue(TargetSurfaceProperty, value); }
        }

        public bool IsDeveloperMode
        {
            get { return (bool) GetValue(IsDeveloperModeProperty); }
            set { SetValue(IsDeveloperModeProperty, value); }
        }

        private static void OnIsDeveloperModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toolbar = (SciChartInteractionToolbar) d;
            var scs = toolbar.TargetSurface;

            if (scs != null)
            {
                bool devModOn = (bool) e.NewValue;

                scs.ChartModifier = devModOn ? toolbar._modifiersInDevMode : toolbar._modifiersInUserMode;

                var listMod = new List<ToolbarItem>();

                var wrappers = WrapModifiers(toolbar.IsDeveloperMode
                    ? toolbar._modifiersInDevMode.ChildModifiers
                    : toolbar._modifiersInUserMode.ChildModifiers);

                listMod.AddRange(wrappers);

                if (
                    listMod.Any(
                        x =>
                            x.Modifier.ModifierName == "AnnotationCreationModifier" ||
                            x.Modifier is VerticalSliceModifier))
                {
                    listMod.Remove(listMod.FirstOrDefault(x => x.Modifier.ModifierName == "AnnotationCreationModifier"));
                    listMod.Remove(listMod.FirstOrDefault(x => x.Modifier is VerticalSliceModifier));
                }

                toolbar.ModifiersSource = listMod;
            }
        }

        private static IEnumerable<ToolbarItem> WrapModifiers(IEnumerable<IChartModifier> modifiers)
        {
            return modifiers.Select(mod =>
            {
                var wrapper = mod is CursorModifier ? new CursorModifierToolbarItem() : new ToolbarItem();
                wrapper.Modifier = mod;

                return wrapper;
            });
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _toolBarWrapPanel = GetTemplateChild("PART_Container") as WrapPanel;

            AddExtraContent();

        }

        private void AddExtraContent()
        {
            if (_toolBarWrapPanel != null && !ExtraContent.IsNullOrEmpty())
            {
                foreach (var child in ExtraContent)
                {
                    _toolBarWrapPanel.Children.Add(child);
                }
            }
        }

        private static void OnExtraContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toolbar = (SciChartInteractionToolbar) d;
            toolbar.AddExtraContent();
        }

        private static void OnTargetSurfaceDependencyPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var toolbar = (SciChartInteractionToolbar) d;
            var scs = e.NewValue as ISciChartSurface;

            if (scs == null)
                return;

            toolbar.OnCreateModifiers(toolbar, scs);
        }

        protected virtual void OnCreateModifiers(SciChartInteractionToolbar toolbar, ISciChartSurface scs)
        {
            var listMod = new List<ToolbarItem>();

            var surface = scs as SciChartSurface;
            var isPolar = surface != null && (surface.IsPolarChart || surface.XAxes.Any(x => x.IsPolarAxis) || surface.YAxes.Any(x => x.IsPolarAxis));

            // RubberBandXyZoomModifier
            var rbzm = new RubberBandXyZoomModifier { IsXAxisOnly = IsZoomXAxisOnly };
            _modifiersInAllMode.ChildModifiers.Add(rbzm);
            _modifiersInDevMode.ChildModifiers.Add(rbzm);
            
            if (!isPolar)
            {
                // ZoomPanModifier
                var zpm = new ZoomPanModifier { ClipModeX = ClipMode.None, IsEnabled = false };
                _modifiersInAllMode.ChildModifiers.Add(zpm);
                _modifiersInDevMode.ChildModifiers.Add(zpm);
            }

            // ZoomExtentsModifier
            var zoomExtents = new ZoomExtentsModifier { ExecuteOn = ExecuteOn.MouseDoubleClick };
            _modifiersInAllMode.ChildModifiers.Add(zoomExtents);
            _modifiersInDevMode.ChildModifiers.Add(zoomExtents);

            // SeriesSelectionModifier
            var selStyle = new Style(typeof(BaseRenderableSeries));
            selStyle.Setters.Add(new Setter(BaseRenderableSeries.StrokeProperty, Colors.Red));
            selStyle.Setters.Add(new Setter(BaseRenderableSeries.StrokeThicknessProperty, 2));
            selStyle.Seal();

            var seriesSelection = new SeriesSelectionModifier
            {
                SelectedSeriesStyle = selStyle,
                ReceiveHandledEvents = true,
                IsEnabled = false
            };
            _modifiersInDevMode.ChildModifiers.Add(seriesSelection);

            // AnnotationCreationModifier
            var annotationMod = new CustomAnnotationCreationModifier();
            annotationMod.AnnotationCreated += (sender, args) =>
            {
                var modifier = (CustomAnnotationCreationModifier)sender;
                if (modifier != null)
                {
                    foreach (var annotation in scs.Annotations)
                    {
                        var newAnnotation = (annotation as AnnotationBase);
                        if (newAnnotation != null)
                        {
                            newAnnotation.IsEditable = true;
                            newAnnotation.CanEditText = true;
                        }
                    }

                    modifier.IsEnabled = false;
                }
            };
            annotationMod.IsEnabled = false;
            _modifiersInDevMode.ChildModifiers.Add(annotationMod);

            // CustomRotateChartModifier
            var rotate = new CustomRotateChartModifier();

            var propertyPath = new PropertyPath(CustomRotateChartModifier.IsRotationEnabledProperty);
            var binding = new Binding() { Source = this, Path = propertyPath };
            rotate.SetBinding(ChartModifierBase.IsEnabledProperty, binding);

            _modifiersInDevMode.ChildModifiers.Add(rotate);

            // Custom Export Modifier
            var export = new CustomExportModifier();
            _modifiersInDevMode.ChildModifiers.Add(export);

            // CustomThemeChangeModifier
            var theme = new CustomThemeChangeModifier();
            _modifiersInDevMode.ChildModifiers.Add(theme);

            // LegendModifier
            var legend = new LegendModifier
            {
                UseInterpolation = true,
                ShowLegend = false,
                ShowVisibilityCheckboxes = true,
                ShowSeriesMarkers = true
            };
            _modifiersInDevMode.ChildModifiers.Add(legend);

            // MouseWheelZoomModifier
            var mouseWheel = new MouseWheelZoomModifier();
            _modifiersInAllMode.ChildModifiers.Add(mouseWheel);
            _modifiersInDevMode.ChildModifiers.Add(mouseWheel);

            // CustomFlipModifier
            var flip = new CustomFlipModifier();
            _modifiersInDevMode.ChildModifiers.Add(flip);

            // RolloverModifier
            var rollover = new RolloverModifier
            {
                IsEnabled = false,
                UseInterpolation = true,
                DrawVerticalLine = true,
                ReceiveHandledEvents = true,
                ShowAxisLabels = true,
                ShowTooltipOn = ShowTooltipOptions.Always
            };
            _modifiersInDevMode.ChildModifiers.Add(rollover);

            // CursorModifier 
            var cursorMod = new CursorModifier
            {
                IsEnabled = false,
                ShowTooltipOn = ShowTooltipOptions.MouseOver,
                ReceiveHandledEvents = true,
                ShowAxisLabels = false,
                ShowTooltip = true
            };
            _modifiersInDevMode.ChildModifiers.Add(cursorMod);

            // TooltipModifier
            var toolTipMod = new TooltipModifier
            {
                ReceiveHandledEvents = true,
                IsEnabled = false,
                UseInterpolation = true
            };
            _modifiersInDevMode.ChildModifiers.Add(toolTipMod);

            if (!isPolar)
            {
                // YAxisDragModifier
                var yAxisDrag = new YAxisDragModifier();
                _modifiersInDevMode.ChildModifiers.Add(yAxisDrag);

                // XAxisDragModifier
                var xAxisDrag = new XAxisDragModifier();
                _modifiersInDevMode.ChildModifiers.Add(xAxisDrag);
            }

            var exampleModifiers = (scs.ChartModifier as ModifierGroup);

            if (exampleModifiers == null)
            {
                exampleModifiers = new ModifierGroup();

                if (scs.ChartModifier != null)
                {
                    exampleModifiers.ChildModifiers.Add(scs.ChartModifier);
                }
            }

            var devMods = new ModifierGroup();
            var userMods = new ModifierGroup();

            foreach (var devMod in _modifiersInDevMode.ChildModifiers)
            { 
                var devModName = devMod.ModifierName;

                if (devMod is CustomAnnotationCreationModifier)
                    devModName = "AnnotationCreationModifier";

                if (!(exampleModifiers.ChildModifiers.Any(x => x.ModifierName == devModName)))
                {
                    devMods.ChildModifiers.Add(devMod);
                }
                else
                {
                    if (exampleModifiers.ChildModifiers.Count(x => x.ModifierName == devModName) == 1)
                    {
                        var exampleMod = exampleModifiers.ChildModifiers.Single(x => x.ModifierName == devModName);

                        if (!GetAppearceInToolbar((ChartModifierBase)exampleMod))
                            continue;

                        devMods.ChildModifiers.Add(exampleMod);
                    }
                    else 
                    {
                        foreach (var exampleMod in exampleModifiers.ChildModifiers.Where(x => x.ModifierName == devModName && GetAppearceInToolbar((ChartModifierBase)x)))
                        {
                            devMods.ChildModifiers.Add(exampleMod);
                        }
                    }
                }
            }

            foreach (var inAllMod in _modifiersInAllMode.ChildModifiers)
            {
                var modName = inAllMod.ModifierName;

                if (!(exampleModifiers.ChildModifiers.Any(x => x.ModifierName == modName)))
                {
                    userMods.ChildModifiers.Add(inAllMod);
                }
                else
                {
                    if (exampleModifiers.ChildModifiers.Count(x => x.ModifierName == modName) == 1)
                    {
                        var exampleMod = exampleModifiers.ChildModifiers.Single(x => x.ModifierName == modName);

                        if (!GetAppearceInToolbar((ChartModifierBase)exampleMod))
                            continue;

                        userMods.ChildModifiers.Add(exampleMod);
                    }
                    else
                    {
                        foreach (var exampleMod in exampleModifiers.ChildModifiers.Where(x => x.ModifierName == modName && GetAppearceInToolbar((ChartModifierBase)x)))
                        {
                            userMods.ChildModifiers.Add(exampleMod);
                        }
                    }
                }
            }

            foreach (var exampleMod in exampleModifiers.ChildModifiers.Where(x => GetAppearceInToolbar((ChartModifierBase)x)))
            {
                if (HasToAddUserModifierToModifierGroup(exampleMod, devMods))
                {
                    devMods.ChildModifiers.Add(exampleMod);
                }

                if (HasToAddUserModifierToModifierGroup(exampleMod, userMods))
                {
                    userMods.ChildModifiers.Add(exampleMod);
                }
            }

            _modifiersInDevMode = devMods;
            _modifiersInUserMode = userMods;

            // Set modifiers to the chart
            scs.ChartModifier = IsDeveloperMode ? _modifiersInDevMode : _modifiersInUserMode;

            var wrappers = WrapModifiers(toolbar.IsDeveloperMode
                ? toolbar._modifiersInDevMode.ChildModifiers
                : toolbar._modifiersInUserMode.ChildModifiers);

            // Set modifiers to the ItemSource for ItemsControl
            listMod.AddRange(wrappers);

            if (listMod.Any(x => x.Modifier.ModifierName == "AnnotationCreationModifier" || x.Modifier is VerticalSliceModifier))
            {
                listMod.Remove(listMod.FirstOrDefault(x => x.Modifier.ModifierName == "AnnotationCreationModifier"));
                listMod.Remove(listMod.FirstOrDefault(x => x.Modifier is VerticalSliceModifier));
            }

            ModifiersSource = listMod;
        }

        private bool HasToAddUserModifierToModifierGroup(IChartModifier userModifier, ModifierGroup modifierGroup)
        {
            AxisDragModifierBase axisModifier = userModifier as AxisDragModifierBase;
            if (axisModifier == null)
            {
                return !modifierGroup.ChildModifiers.Any(x => x.ModifierName == userModifier.ModifierName);
            }

            foreach (var mod in modifierGroup.ChildModifiers.OfType<AxisDragModifierBase>())
            {
                if (mod.ModifierName == axisModifier.ModifierName && mod.AxisId == axisModifier.AxisId) return false;
            }

            return true;
        }
    }
}

