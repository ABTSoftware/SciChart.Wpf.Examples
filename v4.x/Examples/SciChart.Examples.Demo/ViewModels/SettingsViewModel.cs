using System;
using System.Reactive.Linq;
using System.Windows;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Visuals;
using SciChart.Drawing.DirectX.Context.D3D10;
using SciChart.Drawing.HighSpeedRasterizer;
using SciChart.Charting.Visuals.TradeChart;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using SciChart.Charting3D;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Reactive.Traits;

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
            SelectedRenderer = Direct3D10CompatibilityHelper.SupportsDirectX10
                ? typeof (Direct3D10RenderSurface)
                : typeof (HighSpeedRenderSurface);          

            IsDirectXAvailable = Direct3D10CompatibilityHelper.SupportsDirectX10;

            this.WithTrait<AllowFeedbackSettingBehaviour>();

            Observable.CombineLatest(this.WhenPropertyChanged(x => x.UseAlternativeFillSourceD3D),
                this.WhenPropertyChanged(x => EnableForceWaitForGPU), 
                Tuple.Create)
                .Subscribe(t =>
                {
                    Direct3D10RenderSurface.UseAlternativeFillSource = t.Item1;
                    Viewport3D.UseAlternativeFillSource = t.Item1;
                    Direct3D10RenderSurface.EnableForceWaitForGPU = t.Item2;
                    Viewport3D.ForceStallUntilGPUIsIdle = t.Item2;
                    CreateGlobalStyle<SciChartSurface>();
                    CreateGlobalStyle<SciStockChart>();
                })
                .DisposeWith(this);
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

                if (value == typeof(Direct3D10RenderSurface))
                {
                    try
                    {
                        Direct3D10RenderSurface.AssertSupportsDirectX();
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