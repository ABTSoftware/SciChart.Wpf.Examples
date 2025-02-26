// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
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
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D
{
    /// <summary>
    /// A toolbar used in examples to simplify zoom, pan, zoom extents, rollover, cursor etc... This also helps us with
    /// testing ;-)
    /// </summary>
    [ContentProperty("ExtraContent")]
    [TemplatePart(Name = "PART_Container", Type = typeof(Border))]
    public class SciChartInteractionToolbar : ContentControl
    {
        public class ToolbarItem : INotifyPropertyChanged
        {
            private IChartModifier _modifier;

            public IChartModifier Modifier
            {
                get => _modifier;
                set
                {
                    _modifier = value;
                    OnModifierAttached();
                    OnPropertyChanged(nameof(Modifier));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;

                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            protected virtual void OnModifierAttached()
            {
            }
        }

        public class CursorModifierToolbarItem : ToolbarItem
        {
            public IEnumerable<string> SeriesNames
            {
                get
                {
                    if (Modifier?.ParentSurface?.RenderableSeries.Count > 0)
                    {
                        var seriesNames = Modifier.ParentSurface.RenderableSeries
                            .Where(s => !string.IsNullOrEmpty(s.DataSeries?.SeriesName))
                            .Select(s => s.DataSeries.SeriesName)
                            .ToList();

                        if (seriesNames.Count > 0)
                        {
                            SetSnapToSeries(seriesNames.First());
                            return seriesNames;
                        }
                    }
                    return Enumerable.Empty<string>();
                }
            }

            public bool HasSeriesNames
            {
                get
                {
                    if (Modifier?.ParentSurface?.RenderableSeries.Count > 0)
                    {
                        var count = Modifier.ParentSurface.RenderableSeries
                             .Count(s => !string.IsNullOrEmpty(s.DataSeries?.SeriesName));

                        return count > 0;

                    }
                    return false;
                }
            }

            public ActionCommand<string> SnapToSeriesCommand { get; }

            public CursorModifierToolbarItem()
            {
                SnapToSeriesCommand = new ActionCommand<string>(seriesName => SetSnapToSeries(seriesName));
            }

            private void SetSnapToSeries(string seriesName)
            {
                if (!string.IsNullOrEmpty(seriesName))
                {
                    foreach (var series in Modifier.ParentSurface.RenderableSeries.OfType<BaseRenderableSeries>())
                    {
                        if (series?.DataSeries == null) continue;

                        if (series.DataSeries.SeriesName == seriesName)
                        {
                            CursorModifier.SetSnapToSeries(series, true);
                        }
                        else
                        {
                            CursorModifier.SetSnapToSeries(series, false);
                        }
                    }
                }
            }

            protected override void OnModifierAttached()
            {
                OnPropertyChanged(nameof(HasSeriesNames));
                OnPropertyChanged(nameof(SeriesNames));
            }
        }

        public static readonly DependencyProperty AppearceInToolbarProperty = DependencyProperty.RegisterAttached
            ("AppearceInToolbar", typeof(bool), typeof(SciChartInteractionToolbar), new PropertyMetadata(true));

        public static readonly DependencyProperty IsZoomXAxisOnlyProperty = DependencyProperty.Register(
            nameof(IsZoomXAxisOnly), typeof(bool), typeof(SciChartInteractionToolbar), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty TargetSurfaceProperty = DependencyProperty.Register
            (nameof(TargetSurface), typeof(ISciChartSurface), typeof(SciChartInteractionToolbar),
            new PropertyMetadata(default(ISciChartSurface), OnTargetSurfaceDependencyPropertyChanged));

        public static readonly DependencyProperty ExtraContentProperty = DependencyProperty.Register
            (nameof(ExtraContent), typeof(List<ContentControl>), typeof(SciChartInteractionToolbar),
            new PropertyMetadata(OnExtraContentChanged));

        public static readonly DependencyProperty IsDeveloperModeProperty = DependencyProperty.Register
            (nameof(IsDeveloperMode), typeof(bool), typeof(SciChartInteractionToolbar),
            new PropertyMetadata(default(bool), OnIsDeveloperModeChanged));

        public static readonly DependencyProperty ModifiersSourceProperty = DependencyProperty.Register
            (nameof(ModifiersSource), typeof(ICollection<ToolbarItem>), typeof(SciChartInteractionToolbar));

        private WrapPanel _toolBarWrapPanel;
        private ModifierGroup _modifiersInDevMode;
        private ModifierGroup _modifiersInUserMode;

        public SciChartInteractionToolbar()
        {
            DefaultStyleKey = typeof(SciChartInteractionToolbar);
            ExtraContent = new List<ContentControl>();

            _modifiersInDevMode = new ModifierGroup();
            _modifiersInUserMode = new ModifierGroup();
        }

        public bool IsZoomXAxisOnly
        {
            get => (bool)GetValue(IsZoomXAxisOnlyProperty);
            set => SetValue(IsZoomXAxisOnlyProperty, value);
        }

        public ICollection<ToolbarItem> ModifiersSource
        {
            get => (ICollection<ToolbarItem>)GetValue(ModifiersSourceProperty);
            set => SetValue(ModifiersSourceProperty, value);
        }

        public List<ContentControl> ExtraContent
        {
            get => (List<ContentControl>)GetValue(ExtraContentProperty);
            set => SetValue(ExtraContentProperty, value);
        }

        public ISciChartSurface TargetSurface
        {
            get => (ISciChartSurface)GetValue(TargetSurfaceProperty);
            set => SetValue(TargetSurfaceProperty, value);
        }

        public bool IsDeveloperMode
        {
            get => (bool)GetValue(IsDeveloperModeProperty);
            set => SetValue(IsDeveloperModeProperty, value);
        }

        public static void SetAppearceInToolbar(ChartModifierBase element, String value)
        {
            element.SetValue(AppearceInToolbarProperty, value);
        }

        public static bool GetAppearceInToolbar(ChartModifierBase element)
        {
            return (bool)element.GetValue(AppearceInToolbarProperty);
        }

        private static void OnIsDeveloperModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toolbar = (SciChartInteractionToolbar)d;
            var scs = toolbar.TargetSurface;

            if (scs != null)
            {
                bool devModOn = (bool)e.NewValue;
                scs.ChartModifier = devModOn
                    ? toolbar._modifiersInDevMode
                    : toolbar._modifiersInUserMode;

                var listMod = new List<ToolbarItem>();
                var wrappers = WrapModifiers(toolbar.IsDeveloperMode
                    ? toolbar._modifiersInDevMode.ChildModifiers
                    : toolbar._modifiersInUserMode.ChildModifiers);

                listMod.AddRange(wrappers);

                if (listMod.Any(x =>
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
            var toolbar = (SciChartInteractionToolbar)d;
            toolbar.AddExtraContent();
        }

        private static void OnTargetSurfaceDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ISciChartSurface scs)
            {
                var toolbar = (SciChartInteractionToolbar)d;
                toolbar.OnCreateModifiers(toolbar, scs);
            }
        }

        protected virtual void OnCreateModifiers(SciChartInteractionToolbar toolbar, ISciChartSurface scs)
        {
            var isPolar = (scs is SciChartSurface surface) &&
                (surface.IsPolarChart ||
                 surface.XAxes?.Any(x => x.IsPolarAxis) == true ||
                 surface.YAxes?.Any(x => x.IsPolarAxis) == true);

            var devModifiers = new List<IChartModifier>();
            var userModifiers = new List<IChartModifier>();
            var exampleModifiers = new List<IChartModifier>();

            // RubberBandXyZoomModifier
            var rbzm = new RubberBandXyZoomModifier { IsXAxisOnly = IsZoomXAxisOnly };
            userModifiers.Add(rbzm);
            devModifiers.Add(rbzm);

            if (!isPolar)
            {
                // ZoomPanModifier
                var zpm = new ZoomPanModifier { ClipModeX = ClipMode.None, IsEnabled = false };
                userModifiers.Add(zpm);
                devModifiers.Add(zpm);
            }

            // ZoomExtentsModifier
            var zoomExtents = new ZoomExtentsModifier { ExecuteOn = ExecuteOn.MouseDoubleClick };
            userModifiers.Add(zoomExtents);
            devModifiers.Add(zoomExtents);

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

            devModifiers.Add(seriesSelection);

            // AnnotationCreationModifier
            var annotationMod = new CustomAnnotationCreationModifier
            {
                XAxisId = scs.XAxes?.Any() == true ? scs.XAxes.First().Id : AxisCore.DefaultAxisId,
                YAxisId = scs.YAxes?.Any() == true ? scs.YAxes.First().Id : AxisCore.DefaultAxisId
            };

            annotationMod.AnnotationCreated += (sender, args) =>
            {
                var modifier = (CustomAnnotationCreationModifier)sender;
                if (modifier != null)
                {
                    foreach (var annotation in scs.Annotations)
                    {
                        if (annotation is AnnotationBase newAnnotation)
                        {
                            newAnnotation.IsEditable = true;
                            newAnnotation.CanEditText = true;
                        }
                    }

                    modifier.IsEnabled = false;
                }
            };

            annotationMod.IsEnabled = false;
            devModifiers.Add(annotationMod);

            // CustomRotateChartModifier
            var rotate = new CustomRotateChartModifier();
            var propertyPath = new PropertyPath(CustomRotateChartModifier.IsRotationEnabledProperty);
            var binding = new Binding() { Source = this, Path = propertyPath };

            rotate.SetBinding(ChartModifierBase.IsEnabledProperty, binding);
            devModifiers.Add(rotate);

            // Custom Export Modifier
            var export = new CustomExportModifier();
            devModifiers.Add(export);

            // CustomThemeChangeModifier
            var theme = new CustomThemeChangeModifier();
            devModifiers.Add(theme);

            // LegendModifier
            var legend = new LegendModifier
            {
                UseInterpolation = true,
                ShowLegend = false,
                ShowVisibilityCheckboxes = true,
                ShowSeriesMarkers = true
            };

            devModifiers.Add(legend);

            // MouseWheelZoomModifier
            var mouseWheel = new MouseWheelZoomModifier();
            userModifiers.Add(mouseWheel);
            devModifiers.Add(mouseWheel);

            // CustomFlipModifier
            devModifiers.Add(new CustomFlipModifier());

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

            devModifiers.Add(rollover);

            // CursorModifier 
            var cursorMod = new CursorModifier
            {
                IsEnabled = false,
                ShowTooltipOn = ShowTooltipOptions.MouseOver,
                ReceiveHandledEvents = true,
                ShowAxisLabels = false,
                ShowTooltip = true
            };

            devModifiers.Add(cursorMod);

            // TooltipModifier
            var toolTipMod = new TooltipModifier
            {
                ReceiveHandledEvents = true,
                IsEnabled = false,
                UseInterpolation = true
            };

            devModifiers.Add(toolTipMod);

            // AnimationsModifier
            var animationsModifier = new SeriesAnimationCustomModifier();

            devModifiers.Add(animationsModifier);

            if (!isPolar)
            {
                // XAxisDragModifier
                var xAxisDrag = new XAxisDragModifier();
                devModifiers.Add(xAxisDrag);

                // YAxisDragModifier
                var yAxisDrag = new YAxisDragModifier();
                devModifiers.Add(yAxisDrag);
            }

            if (scs.ChartModifier is ModifierGroup modifierGroup)
            {
                modifierGroup.ChildModifiers.ForEachDo(exampleModifiers.Add);
            }
            else if (scs.ChartModifier != null)
            {
                exampleModifiers.Add(scs.ChartModifier);
            }

            _modifiersInUserMode = new ModifierGroup();
            _modifiersInDevMode = new ModifierGroup();

            foreach (var devMod in devModifiers)
            {
                var devModName = devMod.ModifierName;

                if (devMod is CustomAnnotationCreationModifier)
                {
                    devModName = "AnnotationCreationModifier";
                }

                if (exampleModifiers.All(x => x.ModifierName != devModName))
                {
                    _modifiersInDevMode.ChildModifiers.Add(devMod);
                }
                else
                {
                    foreach (var exampleMod in exampleModifiers.Where(x => x.ModifierName == devModName && GetAppearceInToolbar((ChartModifierBase)x)))
                    {
                        _modifiersInDevMode.ChildModifiers.Add(exampleMod);
                    }                 
                }
            }

            foreach (var userMod in userModifiers)
            {
                var userModName = userMod.ModifierName;

                if (exampleModifiers.All(x => x.ModifierName != userModName))
                {
                    _modifiersInUserMode.ChildModifiers.Add(userMod);
                }
                else
                {
                    foreach (var exampleMod in exampleModifiers.Where(x => x.ModifierName == userModName && GetAppearceInToolbar((ChartModifierBase)x)))
                    {
                        _modifiersInUserMode.ChildModifiers.Add(exampleMod);
                    }                    
                }
            }

            foreach (var exampleMod in exampleModifiers.Where(x => GetAppearceInToolbar((ChartModifierBase)x)))
            {
                if (HasToAddUserModifierToModifierGroup(exampleMod, _modifiersInDevMode))
                {
                    _modifiersInDevMode.ChildModifiers.Add(exampleMod);
                }

                if (HasToAddUserModifierToModifierGroup(exampleMod, _modifiersInUserMode))
                {
                    _modifiersInUserMode.ChildModifiers.Add(exampleMod);
                }
            }

            // Set modifiers to the chart
            scs.ChartModifier = IsDeveloperMode
                ? _modifiersInDevMode
                : _modifiersInUserMode;

            var wrappers = WrapModifiers(IsDeveloperMode
                ? _modifiersInDevMode.ChildModifiers
                : _modifiersInUserMode.ChildModifiers);

            // Set modifiers to the ItemSource for ItemsControl
            var listMod = new List<ToolbarItem>();

            listMod.AddRange(wrappers);

            if (listMod.Any(x => x.Modifier.ModifierName == "AnnotationCreationModifier" || x.Modifier is VerticalSliceModifier))
            {
                listMod.Remove(listMod.FirstOrDefault(x => x.Modifier.ModifierName == "AnnotationCreationModifier"));
                listMod.Remove(listMod.FirstOrDefault(x => x.Modifier is VerticalSliceModifier));
            }

            ModifiersSource = listMod;
        }

        private static bool HasToAddUserModifierToModifierGroup(IChartModifier userModifier, ModifierGroup modifierGroup)
        {
            if (userModifier is not AxisDragModifierBase axisModifier)
            {
                return modifierGroup.ChildModifiers.All(x => x.ModifierName != userModifier.ModifierName);
            }

            foreach (var mod in modifierGroup.ChildModifiers.OfType<AxisDragModifierBase>())
            {
                if (mod.ModifierName == axisModifier.ModifierName && mod.AxisId == axisModifier.AxisId)
                {
                    return false;
                }
            }

            return true;
        }
    }
}