using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting.Common.AttachedProperties;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Common.MarkupExtensions;
using SciChart.Charting.Visuals;
using SciChart.Drawing.DirectX.Context.D3D11;
using SciChart.Drawing.HighSpeedRasterizer;
using SciChart.Charting.Visuals.TradeChart;
using SciChart.Charting3D;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.UI.Reactive;
using SciChart.UI.Reactive.Observability;

namespace SciChart.Examples.Demo.ViewModels
{
    public class SettingsViewModel : ViewModelWithTraitsBase
    {
        private Type _selectedRenderer;
        private bool _enabled;

        public SettingsViewModel()
        {
            SelectedRenderer = Direct3D11CompatibilityHelper.SupportsDirectX10
                ? typeof (Direct3D11RenderSurface)
                : typeof (HighSpeedRenderSurface);          

            this.WithTrait<AllowFeedbackSettingBehaviour>();
            IsDirectXAvailable = Direct3D11CompatibilityHelper.SupportsDirectX10;

            if (Direct3D11CompatibilityHelper.HasDirectX10CapableGpu)
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
            EnableResamplingCPlusPlus = false;
            EnableExtremeDrawingManager = true;
            EnableDropShadows = true;

            Observable.CombineLatest(
                this.WhenPropertyChanged(x => x.UseAlternativeFillSourceD3D),
                this.WhenPropertyChanged(x => EnableForceWaitForGPU), 
                Tuple.Create)
                .Subscribe(t =>
                {
                    Direct3D11RenderSurface.UseAlternativeFillSource = t.Item1;
                    Viewport3D.UseAlternativeFillSource = t.Item1;
                    Direct3D11RenderSurface.EnableForceWaitForGPU = t.Item2;
                    Viewport3D.ForceStallUntilGPUIsIdle = t.Item2;
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

                    Viewport3D.Restart3DEngineWith(
                        UseD3D9 ? DirectXMode.DirectX9c : DirectXMode.AutoDetect,
                        Use3DAA4x ? FullScreenAntiAliasingMode.MSAA4x : FullScreenAntiAliasingMode.None,
                        UseD3D10AsFallback);
                });

            this.WhenPropertyChanged(x => x.EnableDropShadows).Subscribe(b => EffectManager.EnableDropShadows = b)
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
            var problemEmaples = new []
            {
                        "SciChart.Examples;component/Examples/Charts3D/Customize3DChart/AddObjectsToA3DChart.xaml"
            };
            bool needGoHome = !String.IsNullOrWhiteSpace(curExampleUri) && problemEmaples.Any(ex => ex == curExampleUri);
            if (needGoHome && Navigator.Instance.NavigateToHomeCommand.CanExecute(null))
            {
                Navigator.Instance.NavigateToHomeCommand.Execute(null);
            }
        }

        public bool UseD3D11
        {
            get => this.GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool UseD3D10AsFallback
        {
            get => this.GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool UseD3D9
        {
            get => this.GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool Use3DAANone
        {
            get => this.GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool Use3DAA4x
        {
            get => this.GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool AllowFeedback
        {
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public bool EnableDropShadows
        {
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public bool EnableResamplingCPlusPlus
        {
            get => this.GetDynamicValue<bool>(); 
            set
            {
                this.SetDynamicValue(value);

                RecreateStyles();
            }
        }

        public bool EnableExtremeDrawingManager
        {
            get => this.GetDynamicValue<bool>(); 
            set
            {
                this.SetDynamicValue(value);

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
            get { return _selectedRenderer; }
            set
            {
                if (_selectedRenderer == value || value == null) return;

                _selectedRenderer = value;
                OnPropertyChanged(value);

                if (value == typeof(Direct3D11RenderSurface))
                {
                    try
                    {
                        Direct3D11RenderSurface.AssertSupportsDirectX();
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
            get => this.GetDynamicValue<bool>(); 
            set => this.SetDynamicValue(value);
        }

        public bool UseAlternativeFillSourceD3D
        {
            get => this.GetDynamicValue<bool>(); 
            set => SetDynamicValue(value);
        }

        public bool IsDirectXAvailable
        {
            get => this.GetDynamicValue<bool>(); 
            set {  this.SetDynamicValue(value); }
        }

        private void CreateGlobalStyle<T>() where T : SciChartSurface
        {
            try
            {
                var overrideStyle = new Style(typeof(T));

                overrideStyle.Setters.Add(new Setter(RenderSurfaceExtensions.RenderSurfaceTypeProperty, _selectedRenderer.AssemblyQualifiedName));
                overrideStyle.Setters.Add(new Setter(PerformanceHelper.EnableExtremeResamplersProperty, EnableResamplingCPlusPlus));
                overrideStyle.Setters.Add(new Setter(PerformanceHelper.EnableExtremeDrawingManagerProperty, EnableExtremeDrawingManager));
                if (Application.Current.Resources.Contains(typeof(T)))
                {
                    Application.Current.Resources.Remove(typeof(T));
                }

                Application.Current.Resources.Add(typeof(T), overrideStyle);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}