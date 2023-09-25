using SciChart.Charting3D;
using SciChart.Charting3D.Modifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar3D.CustomModifiers
{
    internal class CoordinateSystemModifier : ChartModifierBase3D
    {
        private CoordinateSystem3D _selectedCoordinateSystem;

        public CoordinateSystem3D SelectedCoordinateSystem
        {
            get => _selectedCoordinateSystem;
            set
            {
                _selectedCoordinateSystem = value;
                OnPropertyChanged(nameof(SelectedCoordinateSystem));
                SetCoordinateSystem(_selectedCoordinateSystem);                
            }
        }

        private void SetCoordinateSystem(CoordinateSystem3D coordinateSystem)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                sciChartSurface.CoordinateSystem = coordinateSystem;                         
            }
        }
    }
}