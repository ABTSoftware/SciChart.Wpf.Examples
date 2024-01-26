// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting3D;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Modifiers.Tooltip3D;
using SciChart.Core.Extensions;
using SciChart.Core.Utility.Mouse;
using SciChart.Examples.ExternalDependencies.Controls.Toolbar3D.CustomModifiers;

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
                get => _modifier;
                set
                {
                    _modifier = value;
                    OnPropertyChanged(nameof(Modifier));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;

                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static readonly DependencyProperty TargetSurfaceProperty = DependencyProperty.Register
            (nameof(TargetSurface), typeof(ISciChart3DSurface), typeof(SciChart3DInteractionToolbar),
            new PropertyMetadata(default(ISciChart3DSurface), OnTargetSurfaceDependencyPropertyChanged));

        public static readonly DependencyProperty IsDeveloperModeProperty = DependencyProperty.Register
            (nameof(IsDeveloperMode), typeof(bool), typeof(SciChart3DInteractionToolbar),
            new PropertyMetadata(false, OnIsDeveloperModeChanged));

        public static readonly DependencyProperty ExtraContentProperty = DependencyProperty.Register
            (nameof(ExtraContent), typeof(List<ContentControl>), typeof(SciChart3DInteractionToolbar),
            new PropertyMetadata(OnExtraContentChanged));

        public static readonly DependencyProperty ModifiersSourceProperty = DependencyProperty.Register
            (nameof(ModifiersSource), typeof(ICollection<SciChart3DToolbarItem>), typeof(SciChart3DInteractionToolbar));

        private WrapPanel _toolBarWrapPanel;
        private ModifierGroup3D _modifiersInDevMode;
        private ModifierGroup3D _modifiersInUserMode;

        public SciChart3DInteractionToolbar()
        {
            DefaultStyleKey = typeof(SciChart3DInteractionToolbar);
            ExtraContent = new List<ContentControl>();

            _modifiersInUserMode = new ModifierGroup3D();
            _modifiersInDevMode = new ModifierGroup3D();
        }

        public ISciChart3DSurface TargetSurface
        {
            get => (ISciChart3DSurface)GetValue(TargetSurfaceProperty);
            set => SetValue(TargetSurfaceProperty, value);
        }

        public bool IsDeveloperMode
        {
            get => (bool)GetValue(IsDeveloperModeProperty);
            set => SetValue(IsDeveloperModeProperty, value);
        }

        public List<ContentControl> ExtraContent
        {
            get => (List<ContentControl>)GetValue(ExtraContentProperty);
            set => SetValue(ExtraContentProperty, value);
        }

        public ICollection<SciChart3DToolbarItem> ModifiersSource
        {
            get => (ICollection<SciChart3DToolbarItem>)GetValue(ModifiersSourceProperty);
            set => SetValue(ModifiersSourceProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _toolBarWrapPanel = GetTemplateChild("PART_Container") as WrapPanel;

            AddExtraContent();
        }

        private static void OnTargetSurfaceDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ISciChart3DSurface scs)
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

                scs.ChartModifier = devModOn
                    ? toolbar._modifiersInDevMode
                    : toolbar._modifiersInUserMode;

                var wrappers = toolbar.IsDeveloperMode
                    ? toolbar._modifiersInDevMode.ChildModifiers
                    : toolbar._modifiersInUserMode.ChildModifiers;

                toolbar.ModifiersSource = wrappers.Select(x => new SciChart3DToolbarItem { Modifier = x }).ToList();
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
            var freeLookModifier = new FreeLookModifier3D
            {
                IsEnabled = false
            };

            var orbitModifier = new OrbitModifier3D
            {
                IsEnabled = true,
                ExecuteOn = ExecuteOn.MouseLeftButton
            };

            var zoomExtentsModifier = new ZoomExtentsModifier3D
            {
                AnimateDurationMs = 500,
                AutoFitRadius = true,
                ResetPosition = new Vector3(-300, 100, -300)
            };

            var mouseWheelModifier = new MouseWheelZoomModifier3D();

            var vertexSelectionModifier = new VertexSelectionModifier3D
            {
                IsEnabled = false,
                ExecuteOn = ExecuteOn.MouseLeftButton,
                ExecuteWhen = MouseModifier.Ctrl
            };

            var tooltipModifier = new TooltipModifier3D
            {
                IsEnabled = false,
                ShowTooltipOn = ShowTooltipOptions.MouseOver
            };

            var legendModifier = new LegendModifier3D
            {
                LegendPlacement = LegendPlacement.Inside,
                ShowLegend = false
            };

            var devModifiers = new List<IChartModifier3D>();
            var userModifiers = new List<IChartModifier3D>();
            var exampleModifiers = new List<IChartModifier3D>();

            userModifiers.Add(orbitModifier);
            devModifiers.Add(orbitModifier);

            userModifiers.Add(freeLookModifier);
            devModifiers.Add(freeLookModifier);

            userModifiers.Add(zoomExtentsModifier);
            devModifiers.Add(zoomExtentsModifier);

            userModifiers.Add(mouseWheelModifier);
            devModifiers.Add(mouseWheelModifier);

            devModifiers.Add(vertexSelectionModifier);
            devModifiers.Add(legendModifier);
            devModifiers.Add(tooltipModifier);

            devModifiers.Add(new CoordinateSystemModifier());
            devModifiers.Add(new CameraModeModifier());

            devModifiers.Add(new AxisLabelOrientationModifier());
            devModifiers.Add(new AxisTitleOrientationModifier());

            if (scs.ChartModifier is ModifierGroup3D modifierGroup)
            {
                modifierGroup.ChildModifiers.ForEachDo(exampleModifiers.Add);
            }
            else if (scs.ChartModifier != null)
            {
                exampleModifiers.Add(scs.ChartModifier);
            }

            _modifiersInUserMode = new ModifierGroup3D();
            _modifiersInDevMode = new ModifierGroup3D();

            foreach (var devMod in devModifiers)
            {
                var devModName = devMod.ModifierName;

                if (exampleModifiers.All(x => x.ModifierName != devModName))
                {
                    _modifiersInDevMode.ChildModifiers.Add(devMod);
                }
                else
                {
                    foreach (var exampleMod in exampleModifiers.Where(x => x.ModifierName == devModName))
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
                    foreach (var exampleMod in exampleModifiers.Where(x => x.ModifierName == userModName))
                    {
                        _modifiersInUserMode.ChildModifiers.Add(exampleMod);
                    }
                }
            }

            foreach (var exampleMod in exampleModifiers)
            {
                if (_modifiersInDevMode.ChildModifiers.All(x => x.ModifierName != exampleMod.ModifierName))
                {
                    _modifiersInDevMode.ChildModifiers.Add(exampleMod);
                }

                if (_modifiersInUserMode.ChildModifiers.All(x => x.ModifierName != exampleMod.ModifierName))
                {
                    _modifiersInUserMode.ChildModifiers.Add(exampleMod);
                }
            }

            // Set modifiers to the chart
            scs.ChartModifier = IsDeveloperMode
                ? _modifiersInDevMode
                : _modifiersInUserMode;

            var wrappers = toolbar.IsDeveloperMode
                ? _modifiersInDevMode.ChildModifiers
                : _modifiersInUserMode.ChildModifiers;

            // Set modifiers to the ItemSource for ItemsControl
            ModifiersSource = wrappers.Select(x => new SciChart3DToolbarItem { Modifier = x }).ToList();
        }
    }
}