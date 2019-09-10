using System.Collections.Generic;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public interface IPowerManager
    {
        /// <returns>
        /// All supported power plans.
        /// </returns>
        List<PowerPlan> GetPlans();

        PowerPlan GetCurrentPlan();

        void SetActive(PowerPlan plan);

        /// <summary>
        /// Opens Power Options section of the Control Panel.
        /// </summary>
        void OpenControlPanel();
    }
}