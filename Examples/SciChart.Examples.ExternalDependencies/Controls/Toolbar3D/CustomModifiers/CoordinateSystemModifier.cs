// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CoordinateSystemModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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