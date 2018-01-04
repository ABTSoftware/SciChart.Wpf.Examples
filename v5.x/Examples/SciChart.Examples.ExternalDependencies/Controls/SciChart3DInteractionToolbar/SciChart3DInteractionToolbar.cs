// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SciChart3DInteractionToolbar.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using SciChart.Core.Extensions;
using SciChart.Core.Utility.Mouse;
using SciChart.Charting3D;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Modifiers.Tooltip3D;
using SciChart.Charting.ChartModifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChart3DInteractionToolbar
{
    [ContentProperty("ExtraContent")]
    [TemplatePart(Name = "PART_Container", Type = typeof(Border))]
    public class SciChart3DInteractionToolbar : ContentControl
    {
        public class SciChart3DToolbarItem : INotifyPropertyChanged
        {
            private IChartModifier3D _modifier;

            public IChartModifier3D Modifier
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
        
        public static readonly DependencyProperty TargetSurfaceProperty = DependencyProperty.Register("TargetSurface", typeof(ISciChart3DSurface), typeof(SciChart3DInteractionToolbar), new PropertyMetadata(default(ISciChart3DSurface), OnTargetSurfaceDependencyPropertyChanged));

        public static readonly DependencyProperty IsDeveloperModeProperty = DependencyProperty.Register("IsDeveloperMode", typeof(bool), typeof(SciChart3DInteractionToolbar), new PropertyMetadata(default(bool), OnIsDeveloperModeChanged));

        public static readonly DependencyProperty ExtraContentProperty = DependencyProperty.Register("ExtraContent", typeof(List<ContentControl>), typeof(SciChart3DInteractionToolbar), new PropertyMetadata(OnExtraContentChanged));

        public static readonly DependencyProperty ModifiersSourceProperty = DependencyProperty.Register("ModifiersSource", typeof(ICollection<SciChart3DToolbarItem>), typeof(SciChart3DInteractionToolbar));

        private WrapPanel _toolBarWrapPanel;
        private ModifierGroup3D _modifiersInDevMode;
        private ModifierGroup3D _modifiersInUserMode;
        private readonly ModifierGroup3D _modifiersInAllMode;

        public SciChart3DInteractionToolbar()
        {
            DefaultStyleKey = typeof(SciChart3DInteractionToolbar);
            ExtraContent = new List<ContentControl>();

            _modifiersInUserMode = new ModifierGroup3D();
            _modifiersInAllMode = new ModifierGroup3D();
            _modifiersInDevMode = new ModifierGroup3D();
        }

        public ISciChart3DSurface TargetSurface
        {
            get { return (ISciChart3DSurface)GetValue(TargetSurfaceProperty); }
            set { SetValue(TargetSurfaceProperty, value); }
        }

        public bool IsDeveloperMode
        {
            get { return (bool)GetValue(IsDeveloperModeProperty); }
            set { SetValue(IsDeveloperModeProperty, value); }
        }

        public List<ContentControl> ExtraContent
        {
            get { return (List<ContentControl>)GetValue(ExtraContentProperty); }
            set { SetValue(ExtraContentProperty, value); }
        }

        public ICollection<SciChart3DToolbarItem> ModifiersSource
        {
            get { return (ICollection<SciChart3DToolbarItem>)GetValue(ModifiersSourceProperty); }
            set { SetValue(ModifiersSourceProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _toolBarWrapPanel = GetTemplateChild("PART_Container") as WrapPanel;

            AddExtraContent();
        }

        private static void OnTargetSurfaceDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scs = e.NewValue as ISciChart3DSurface;

            if (scs != null)
            {
                var toolbar = (SciChart3DInteractionToolbar)d;
                toolbar.OnCreateModifiers(toolbar, scs);
            }
        }

        private static void OnIsDeveloperModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toolbar = (SciChart3DInteractionToolbar)d;
            var scs = toolbar.TargetSurface;

            if (scs != null)
            {
                var devModOn = (bool)e.NewValue;

                scs.ChartModifier = devModOn ? toolbar._modifiersInDevMode : toolbar._modifiersInUserMode;

                var listMod = new List<SciChart3DToolbarItem>();

                var wrappers = toolbar.IsDeveloperMode
                    ? toolbar._modifiersInDevMode.ChildModifiers
                    : toolbar._modifiersInUserMode.ChildModifiers;

                listMod.AddRange(wrappers.Select(mod => new SciChart3DToolbarItem { Modifier = mod }));

                toolbar.ModifiersSource = listMod;
            }
        }


        private static void OnExtraContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toolbar = (SciChart3DInteractionToolbar)d;
            toolbar.AddExtraContent();
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

        protected virtual void OnCreateModifiers(SciChart3DInteractionToolbar toolbar, ISciChart3DSurface scs)
        {
            var freeLookModifier = new FreeLookModifier3D { IsEnabled = false };
            var orbitModifier = new OrbitModifier3D { IsEnabled = true, ExecuteOn = ExecuteOn.MouseLeftButton };
            var zoomExtentsModifier = new ZoomExtentsModifier3D { AnimateDurationMs = 500, AutoFitRadius = true, ResetPosition = new Vector3(-300, 100, -300) };
            var mouseWheelZoomPanModifier = new MouseWheelZoomModifier3D();

            var vertexSelectionMod = new VertexSelectionModifier3D { IsEnabled = false, ExecuteOn = ExecuteOn.MouseLeftButton, ExecuteWhen = MouseModifier.Ctrl };
            var tooltipMod = new TooltipModifier3D { IsEnabled = false, ShowTooltipOn = ShowTooltipOptions.MouseOver, ShowAxisLabels = true };
            var legendMod = new LegendModifier3D { LegendPlacement = LegendPlacement.Inside, ShowLegend = false };

            _modifiersInAllMode.ChildModifiers.Add(orbitModifier);
            _modifiersInDevMode.ChildModifiers.Add(orbitModifier);

            _modifiersInAllMode.ChildModifiers.Add(freeLookModifier);
            _modifiersInDevMode.ChildModifiers.Add(freeLookModifier);

            _modifiersInAllMode.ChildModifiers.Add(zoomExtentsModifier);
            _modifiersInDevMode.ChildModifiers.Add(zoomExtentsModifier);

            _modifiersInAllMode.ChildModifiers.Add(mouseWheelZoomPanModifier);
            _modifiersInDevMode.ChildModifiers.Add(mouseWheelZoomPanModifier);

            _modifiersInDevMode.ChildModifiers.Add(vertexSelectionMod);
            _modifiersInDevMode.ChildModifiers.Add(legendMod);
            _modifiersInDevMode.ChildModifiers.Add(tooltipMod);

            var exampleModifiers = (scs.ChartModifier as ModifierGroup3D);

            if (exampleModifiers == null)
            {
                exampleModifiers = new ModifierGroup3D();

                if (scs.ChartModifier != null)
                {
                    exampleModifiers.ChildModifiers.Add(scs.ChartModifier);
                }
            }

            var devMods = new ModifierGroup3D();
            var userMods = new ModifierGroup3D();

            foreach (var devMod in _modifiersInDevMode.ChildModifiers)
            {
                var devModName = devMod.ModifierName;

                if (!(exampleModifiers.ChildModifiers.Any(x => x.ModifierName == devModName)))
                {
                    devMods.ChildModifiers.Add(devMod);
                }
                else
                {
                    if (exampleModifiers.ChildModifiers.Count(x => x.ModifierName == devModName) == 1)
                    {
                        var exampleMod = exampleModifiers.ChildModifiers.Single(x => x.ModifierName == devModName);
                        devMods.ChildModifiers.Add(exampleMod);
                    }
                    else 
                    {
                        foreach (var exampleMod in exampleModifiers.ChildModifiers.Where(x => x.ModifierName == devModName))
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
                        userMods.ChildModifiers.Add(exampleMod);
                    }
                    else
                    {
                        foreach (var exampleMod in exampleModifiers.ChildModifiers.Where(x => x.ModifierName == modName))
                        {
                            userMods.ChildModifiers.Add(exampleMod);
                        }
                    }
                }
            }

            foreach (var exampleMod in exampleModifiers.ChildModifiers)
            {
                if (!devMods.ChildModifiers.Any(x => x.ModifierName == exampleMod.ModifierName))
                {
                    devMods.ChildModifiers.Add(exampleMod);
                }

                if (!userMods.ChildModifiers.Any(x => x.ModifierName == exampleMod.ModifierName))
                {
                    userMods.ChildModifiers.Add(exampleMod);
                }
            }

            _modifiersInDevMode = devMods;
            _modifiersInUserMode = userMods;

            // Set modifiers to the chart
            scs.ChartModifier = IsDeveloperMode ? _modifiersInDevMode : _modifiersInUserMode;

            var wrappers = toolbar.IsDeveloperMode
                ? _modifiersInDevMode.ChildModifiers
                : _modifiersInUserMode.ChildModifiers;

            // Set modifiers to the ItemSource for ItemsControl
            var listMod = new List<SciChart3DToolbarItem>();

            listMod.AddRange(wrappers.Select(mod => new SciChart3DToolbarItem { Modifier = mod }));

            ModifiersSource = listMod;

        }
    }
}