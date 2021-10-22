﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using SciChart.Charting;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Common.MarkupExtensions;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.TradeChart;
using SciChart.Charting3D;
using SciChart.Data.Numerics.PointResamplers;
using SciChart.Drawing.HighSpeedRasterizer;
using SciChart.Drawing.VisualXcceleratorRasterizer;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.UI.Reactive;
using SciChart.UI.Reactive.Observability;
using FullScreenAntiAliasingMode = SciChart.Charting3D.FullScreenAntiAliasingMode;

namespace SciChart.Examples.Demo.ViewModels
{
    public class SettingsViewModel : ViewModelWithTraitsBase
    {
        private Type _selectedRenderer;

        public SettingsViewModel()
        {
            SelectedRenderer = VisualXcceleratorEngine.SupportsHardwareAcceleration &&
                               !VisualXcceleratorEngine.IsGpuBlacklisted
                ? typeof (VisualXcceleratorRenderSurface)
                : typeof (HighSpeedRenderSurface);          

            WithTrait<AllowFeedbackSettingBehaviour>();
            IsDirectXAvailable = VisualXcceleratorEngine.SupportsHardwareAcceleration;

            if (VisualXcceleratorEngine.HasDirectX10OrBetterCapableGpu)
            {
                UseD3D11 = true;
                UseD3D9 = false;
            }
            else
            {
                UseD3D11 = false;
                UseD3D9 = true;
            }

            UseD3D10AsFallback = true;
            Use3DAA4x = false;
            Use3DAANone = true;

            EnableResamplingCPlusPlus = true;
            EnableExtremeDrawingManager = false;

            EnableSimd = true;
            EnableImpossibleMode = false;
            EnableDropShadows = true;

            UseAlternativeFillSourceD3D = true;
            // Always force wait for draw in UIAutomationTestMode 
            EnableForceWaitForGPU = App.UIAutomationTestMode;

            Observable.CombineLatest(
                this.WhenPropertyChanged(x => x.UseAlternativeFillSourceD3D),
                this.WhenPropertyChanged(x => EnableForceWaitForGPU), 
                Tuple.Create)
                .Subscribe(t =>
                {
                    VisualXcceleratorEngine.UseAlternativeFillSource = t.Item1;
                    Viewport3D.UseAlternativeFillSource = t.Item1;
                    VisualXcceleratorEngine.EnableForceWaitForGPU = t.Item2;
                    Viewport3D.ForceStallUntilGPUIsIdle = t.Item2;
                    VisualXcceleratorRenderSurface.UseAlternativeFillSource = t.Item1;
                    VisualXcceleratorRenderSurface.ForceStallUntilGPUIsIdle = t.Item2;
                    CreateGlobalStyle<SciChartSurface>();
                    CreateGlobalStyle<SciStockChart>();
                })
                .DisposeWith(this);

            Observable.CombineLatest(
                    this.WhenPropertyChanged(x => x.UseD3D9),
                    this.WhenPropertyChanged(x => x.UseD3D11),
                    this.WhenPropertyChanged(x => x.UseD3D10AsFallback),
                    this.WhenPropertyChanged(x => x.Use3DAANone),
                    this.WhenPropertyChanged(x => x.Use3DAA4x),
                    Tuple.Create)
                .Skip(1)
                .Throttle(TimeSpan.FromMilliseconds(1))
                .ObserveOn(DispatcherSchedulerEx.Current)
                .Subscribe(t =>
                {                    
                    GoHomeInCaseOfProblemExample();

                    // Restart 3D Engine with D3D9/D3D10/Auto, AntiAliasing mode 
                    Viewport3D.Restart3DEngineWith(
                        UseD3D9 ? DirectXMode.DirectX9c : DirectXMode.AutoDetect,
                        Use3DAA4x ? FullScreenAntiAliasingMode.MSAA4x : FullScreenAntiAliasingMode.None,
                        UseD3D10AsFallback);

                    // Restart 2D engine with D3D9
                    VisualXcceleratorRenderSurface.RestartEngineWith(
                        UseD3D9 ? DirectXMode.DirectX9c : DirectXMode.AutoDetect);
                });

            this.WhenPropertyChanged(x => x.EnableDropShadows)
                .Subscribe(b => EffectManager.EnableDropShadows = b)
                .DisposeWith(this);
        }

        /// <summary>
        /// Work around for SC-4346:
        ///   Go home is case current example is one of examples that can cause application crash
        ///   during 3D engine renderer switch.
        /// </summary>
        private static void GoHomeInCaseOfProblemExample()
        {
            var curExampleUri = Navigator.Instance.CurrentExample?.Uri;
            var problemExamples = new []
            {
                "SciChart.Examples;component/Examples/Charts3D/Customize3DChart/AddObjectsToA3DChart.xaml"
            };

            bool needGoHome = !string.IsNullOrWhiteSpace(curExampleUri) && problemExamples.Any(ex => ex == curExampleUri);
            if (needGoHome && Navigator.Instance.NavigateToHomeCommand.CanExecute(null))
            {
                Navigator.Instance.NavigateToHomeCommand.Execute(null);
            }
        }

        public bool UseD3D11
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool UseD3D10AsFallback
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool UseD3D9
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool Use3DAANone
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool Use3DAA4x
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool AllowFeedback
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool EnableDropShadows
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool EnableResamplingCPlusPlus
        {
            get => GetDynamicValue<bool>(); 
            set
            {
                SetDynamicValue(value);

                RecreateStyles();
            }
        }

        public bool EnableSimd
        {
            get => GetDynamicValue<bool>();
            set
            {
                SetDynamicValue(value);

                ExtremeResamplersFactory.Instance.AccelerationMode = value ? ExtremeResamplerAccelerationMode.Auto : ExtremeResamplerAccelerationMode.None;
            }
        }

        public bool EnableImpossibleMode
        {
            get => GetDynamicValue<bool>();
            set
            {
                SetDynamicValue(value);

                RecreateStyles();
            }
        }

        public bool EnableExtremeDrawingManager
        {
            get => GetDynamicValue<bool>(); 
            set
            {
                SetDynamicValue(value);

                RecreateStyles();
            }
        }

        private void RecreateStyles()
        {
            // Creates a style with this markup and adds to application resource to affect all charts
            // <Style TargetType="s:SciChartSurface">
            //   <Setter Property="RenderSurfaceBase.RenderSurfaceType" Value="_selectedRenderer"/>
            // </Style>

            CreateGlobalStyle<SciChartSurface>();
            CreateGlobalStyle<SciStockChart>();
        }

        public Type SelectedRenderer
        {
            get => _selectedRenderer;
            set
            {
                if (_selectedRenderer == value || value == null) return;

                _selectedRenderer = value;
                OnPropertyChanged(value);

                IsDirectXEnabled2D = value == typeof(VisualXcceleratorRenderSurface);
                if (IsDirectXEnabled2D)
                {
                    try
                    {
                        VisualXcceleratorEngine.AssertSupportsDirectX();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }

                // Creates a style with this markup and adds to application resource to affect all charts
                // <Style TargetType="s:SciChartSurface">
                //   <Setter Property="RenderSurfaceBase.RenderSurfaceType" Value="_selectedRenderer"/>
                // </Style>

                CreateGlobalStyle<SciChartSurface>();
                CreateGlobalStyle<SciStockChart>();
            }
        }

        public bool EnableForceWaitForGPU
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool IsDirectXEnabled2D
        {
            get => GetDynamicValue<bool>();
            set => SetDynamicValue(value);
        }

        public bool UseAlternativeFillSourceD3D
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool IsDirectXAvailable
        {
            get => GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        private void CreateGlobalStyle<T>() where T : SciChartSurface
        {
            var overrideStyle = new Style(typeof(T));

            if (SelectedRenderer == typeof(VisualXcceleratorRenderSurface) && !App.UIAutomationTestMode)
            {
                overrideStyle.Setters.Add(new Setter(VisualXcceleratorEngine.IsEnabledProperty, true));
            }
            else
            {
                overrideStyle.Setters.Add(new Setter(RenderSurfaceExtensions.RenderSurfaceTypeProperty, _selectedRenderer.AssemblyQualifiedName));
            }
            overrideStyle.Setters.Add(new Setter(PerformanceHelper.EnableExtremeResamplersProperty, EnableResamplingCPlusPlus));
            overrideStyle.Setters.Add(new Setter(PerformanceHelper.EnableExtremeDrawingManagerProperty, EnableExtremeDrawingManager));
            overrideStyle.Setters.Add(new Setter(VisualXcceleratorEngine.EnableImpossibleModeProperty, EnableImpossibleMode));

            if (Application.Current.Resources.Contains(typeof(T)))
            {
                Application.Current.Resources.Remove(typeof(T));
            }

            Application.Current.Resources.Add(typeof(T), overrideStyle);
        }
    }
}