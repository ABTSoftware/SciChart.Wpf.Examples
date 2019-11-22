using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Utility;
using SciChart.Core.Utility.Mouse;

namespace CustomModifierSandboxExample
{
    /// <summary>
    /// A Simple Implementation of a RolloverModifier, to demonstrate the ChartModifier API for creating cursors in SciChart
    /// 
    /// TODOS: Some things which could be improved / added here include
    /// 
    ///    1. Expose styles for the line, rollover points and allow them to be customized in XAML where you declare your <SimpleRolloverModifier/>
    ///    2. Manage UIElements better, do not Clear and Re-add every frame as this is slow. Update existing ones instead. 
    ///    3. Handle OnParentSurfaceRendered to update ellipse positions and hit-test results
    ///    4. Instead of manually creating tooltip UI, you could expose a TooltipTemplate property on the SimpleRolloverModifier
    ///       and add a new ContentControl() { Template = TooltipTemplate } to the ModifierSurface instead
    /// </summary>
    public class SimpleRolloverModifier : ChartModifierBase 
    {
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush", typeof(Brush), typeof(SimpleRolloverModifier), new PropertyMetadata(new SolidColorBrush(Colors.LimeGreen), OnLineBrushChanged));

        public static readonly DependencyProperty TextForegroundProperty = DependencyProperty.Register("TextForeground", typeof (Brush), typeof (SimpleRolloverModifier), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush TextForeground
        {
            get { return (Brush) GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }
        private Line _line;

        public SimpleRolloverModifier()
        {
            CreateLine(this);
        }

        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        public override void OnParentSurfaceRendered(SciChartRenderedMessage e)
        {
            base.OnParentSurfaceRendered(e);

            // TODO HERE: You could perform a hit-test again and update your ellipse positions on the rendered event
        }

        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            base.OnModifierMouseMove(e);
                        
            var allSeries = this.ParentSurface.RenderableSeries;

            // Translates the mouse point to chart area, e.g. when you have left axis
            var pt = GetPointRelativeTo(e.MousePoint, this.ModifierSurface);

            // Position the rollover line
            _line.Y1 = 0;
            _line.Y2 = ModifierSurface.ActualHeight;
            _line.X1 = pt.X;
            _line.X2 = pt.X;
                        
            ClearModifierSurface();

            // Add the rollover line to the ModifierSurface, which is just a canvas over the main chart area, on top of series
            this.ModifierSurface.Children.Add(_line);

            // Add the rollover points to the surface
            var hitTestResults = allSeries.Select(x => x.VerticalSliceHitTest(pt)).ToArray();
            foreach (var hitTestResult in hitTestResults)
            {
                const int markerSize = 7;

                // Create one ellipse per HitTestResult and position on the canvas
                var ellipse = new Ellipse()
                    {
                        Width = markerSize,
                        Height = markerSize, 
                        Fill = _line.Stroke,    
                        IsHitTestVisible = false,
                        Tag = typeof(SimpleRolloverModifier)
                    };

                //ToolTip = 

                Canvas.SetLeft(ellipse, hitTestResult.HitTestPoint.X - markerSize*0.5);
                Canvas.SetTop(ellipse, hitTestResult.HitTestPoint.Y - markerSize*0.5);

                this.ModifierSurface.Children.Add(ellipse);

                // Create one label per HitTestResult and position on the canvas
                // TODO: Could this be templated? Yes it could! 
                var text = new Border()
                    {
                        IsHitTestVisible = false,
                        BorderBrush = TextForeground,
                        BorderThickness = new Thickness(1),
                        Background = LineBrush,
                        CornerRadius = new CornerRadius(2,2,2,2),      
                        Tag = typeof(SimpleRolloverModifier),
                        Child = new TextBlock()
                            {
                                Text = string.Format("X: {0}, Y: {1}", XAxis.FormatCursorText(hitTestResult.XValue), YAxis.FormatCursorText(hitTestResult.YValue)),
                                FontSize = 11,
                                Margin = new Thickness(3),
                                Foreground = TextForeground, 
                            }
                    };

                Canvas.SetLeft(text, hitTestResult.HitTestPoint.X + 5);
                Canvas.SetTop(text, hitTestResult.HitTestPoint.Y - 5);

                this.ModifierSurface.Children.Add(text);
            }
        }

        private void ClearModifierSurface()
        {
            for (int i = ParentSurface.ModifierSurface.Children.Count - 1; i >= 0; --i)
            {
                if (((FrameworkElement)ParentSurface.ModifierSurface.Children[i]).Tag == typeof (SimpleRolloverModifier))
                {
                    ParentSurface.ModifierSurface.Children.RemoveAt(i);
                }
            }
        }

        public override void OnMasterMouseLeave(ModifierMouseArgs e)
        {
            base.OnMasterMouseLeave(e);

            // Remove the rollover line and markers from the surface
            ClearModifierSurface();
        }

        private static void OnLineBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var modifier = d as SimpleRolloverModifier;
            CreateLine(modifier);
        }

        private static void CreateLine(SimpleRolloverModifier modifier)
        {
            modifier._line = new Line() { Stroke = modifier.LineBrush, StrokeThickness = 1, IsHitTestVisible = false, Tag = typeof(SimpleRolloverModifier) };
        }
    }
}
