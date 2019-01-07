using System.Diagnostics;
using SciChart.Charting.ChartModifiers;

namespace SciChart.Sandbox.Examples.TouchScreenModifiers
{
    public class CustomTouchModifier : ChartModifierBase
    {
        public override void OnModifierTouchDown(ModifierTouchArgs e)
        {
            base.OnModifierTouchDown(e);
            Debug.WriteLine("CustomTouchModifier.OnModifierTouchDown");
        }

        public override void OnModifierTouchMove(ModifierTouchArgs e)
        {
            base.OnModifierTouchMove(e);
            Debug.WriteLine("CustomTouchModifier.OnModifierTouchMove");
        }

        public override void OnModifierTouchUp(ModifierTouchArgs e)
        {
            base.OnModifierTouchUp(e);
            Debug.WriteLine("CustomTouchModifier.OnModifierTouchUp");
        }

        public override void OnModifierTouchManipulationDelta(ModifierManipulationDeltaArgs e)
        {
            base.OnModifierTouchManipulationDelta(e);
            Debug.WriteLine("CustomTouchModifier.OnModifierTouchManipulationDelta");
        }
    }
}