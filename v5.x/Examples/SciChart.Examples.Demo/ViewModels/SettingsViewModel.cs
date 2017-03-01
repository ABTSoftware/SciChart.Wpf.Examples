using System;
using System.Reactive.Linq;
using System.Windows;
using CodeHighlighter.Common;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Visuals;
using SciChart.Drawing.DirectX.Context.D3D11;
using SciChart.Drawing.HighSpeedRasterizer;
using SciChart.Charting.Visuals.TradeChart;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using SciChart.Charting3D;
using SciChart.Examples.Demo.Helpers;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Reactive.Traits;
using ServiceLocator = SciChart.Wpf.UI.Bootstrap.ServiceLocator;

namespace SciChart.Examples.Demo.ViewModels
{
    public class AllowFeedbackSettingBehaviour : ViewModelTrait<SettingsViewModel>
    {
        private readonly ISyncUsageHelper _usageHelper;

        public AllowFeedbackSettingBehaviour(SettingsViewModel target, ISyncUsageHelper usageHelper) : base(target)
        {
            _usageHelper = usageHelper;

            target.WhenPropertyChanged(x => x.AllowFeedback)
                .Skip(1)
                .Subscribe(allowFeedback =>
                {
                    usageHelper.Enabled = allowFeedback;
                }).DisposeWith(this);

            _usageHelper.EnabledChanged += (s, e) =>
            {
                target.AllowFeedback = _usageHelper.Enabled;
            };
        }
    }

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

            Use3DAA4x = false;
            Use3DAANone = true;

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
                    this.WhenPropertyChanged(x => x.Use3DAANone),
                    this.WhenPropertyChanged(x => x.Use3DAA4x),
                    Tuple.Create)
                .Skip(1)
                .Throttle(TimeSpan.FromMilliseconds(1))
                .ObserveOnDispatcher()
                .Subscribe(t =>
                {
                    Viewport3D.Restart3DEngineWith(
                        UseD3D9 ? DirectXMode.DirectX9c : DirectXMode.AutoDetect, 
                        Use3DAA4x ? FullScreenAntiAliasingMode.MSAA4x : FullScreenAntiAliasingMode.None);         
                });
        }

        public bool UseD3D11
        {
            get { return GetDynamicValue<bool>("UseD3D11"); }
            set {  SetDynamicValue("UseD3D11", value);}
        }

        public bool UseD3D9
        {
            get { return GetDynamicValue<bool>("UseD3D9"); }
            set { SetDynamicValue("UseD3D9", value); }
        }

        public bool Use3DAANone
        {
            get { return GetDynamicValue<bool>("Use3DAANone"); }
            set { SetDynamicValue("Use3DAANone", value); }
        }

        public bool Use3DAA4x
        {
            get { return GetDynamicValue<bool>("Use3DAA4x"); }
            set { SetDynamicValue("Use3DAA4x", value); }
        }

        public bool AllowFeedback
        {
            get { return GetDynamicValue<bool>("AllowFeedback"); }
            set { SetDynamicValue("AllowFeedback", value); }
        }

        public Type SelectedRenderer
        {
            get { return _selectedRenderer; }
            set
            {
                if (_selectedRenderer == value || value == null) return;

                _selectedRenderer = value;
                OnPropertyChanged("SelectedRenderer", value);

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
            get { return GetDynamicValue<bool>("EnableForceWaitForGPU"); }
            set { SetDynamicValue("EnableForceWaitForGPU", value); }
        }

        public bool UseAlternativeFillSourceD3D
        {
            get { return GetDynamicValue<bool>("UseAlternativeFillSourceD3D"); }
            set { SetDynamicValue("UseAlternativeFillSourceD3D", value); }
        }

        public bool IsDirectXAvailable
        {
            get { return GetDynamicValue<bool>("IsDirectXAvailable"); }
            set {  SetDynamicValue("IsDirectXAvailable", value); }
        }

        private void CreateGlobalStyle<T>() where T : SciChartSurface
        {
            try
            {
                var overrideStyle = new Style(typeof(T));

                overrideStyle.Setters.Add(new Setter(RenderSurfaceExtensions.RenderSurfaceTypeProperty, _selectedRenderer.AssemblyQualifiedName));
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