// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// AntennaMetrics.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Antenna3DRadiationPattern
{
    /// <summary>
    /// Scalar figures-of-merit derived from the antenna gain grid.
    /// Bound into the LegendModifier3D metrics panel via LegendTemplate.
    /// </summary>
    public class AntennaMetrics : INotifyPropertyChanged
    {
        // Gain is stored normalised to [0,1]; DbiRange maps that to [-40, 0] dBi.
        public const double DbiRange = 40.0;
        public static double ToDbi(double normalizedGain) => normalizedGain * DbiRange - DbiRange;
        public static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
        public static double ToDegrees(double radians) => radians * 180.0 / Math.PI;

        // All metric strings default to "—" (em-dash) until computed.
        private string _peakGain = "—";
        private string _hpbwEPlane = "—";
        private string _hpbwHPlane = "—";
        private string _firstSll = "—";
        private string _frontToBack = "—";

        /// <summary>Maximum gain across the entire pattern, in dBi.</summary>
        public string PeakGain
        {
            get => _peakGain;
            set { _peakGain = value; OnPropertyChanged(); }
        }

        /// <summary>Half-power beamwidth in the E-plane (constant φ cut), in degrees.</summary>
        public string HpbwEPlane
        {
            get => _hpbwEPlane;
            set { _hpbwEPlane = value; OnPropertyChanged(); }
        }

        /// <summary>Half-power beamwidth in the H-plane (constant θ cut), in degrees.</summary>
        public string HpbwHPlane
        {
            get => _hpbwHPlane;
            set { _hpbwHPlane = value; OnPropertyChanged(); }
        }

        /// <summary>First sidelobe level relative to the main lobe peak, in dB.</summary>
        public string FirstSll
        {
            get => _firstSll;
            set { _firstSll = value; OnPropertyChanged(); }
        }

        /// <summary>Front-to-back ratio: peak forward gain vs peak rearward gain, in dB.</summary>
        public string FrontToBack
        {
            get => _frontToBack;
            set { _frontToBack = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
