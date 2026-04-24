// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// RacingTelemetryDataGenerator.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;

namespace SciChart.Examples.ExternalDependencies.Data
{
    /// <summary>
    /// Provides helper methods for generating synthetic racing telemetry data.
    /// </summary>
    public class RacingTelemetryDataGenerator
    {
        // ===================================================================
        // CORE LAP PROFILE (private — used internally by all panel methods)
        // ===================================================================

        // Corner layout: each row = { brake, apex, exit, apexKph, maxKph }
        // C1: least sharp — gentle braking, quick throttle pickup
        // C2: sharpest   — heaviest braking, lowest apex speed, slow exit
        // C3: hard but SHORT braking, aggressive (fast) acceleration out
        // C4: light but LONG braking, slow cautious acceleration out
        // C5: slightly sharper than C1
        private readonly double[,] _corners =
        {
            {  820,  900,  990, 140, 210 },  // C1: 80m brake,  70 kph drop
            { 1620, 1870, 2100,  38, 220 },  // C2: 250m brake, 182 kph drop
            { 2630, 2750, 2900,  58, 215 },  // C3: 120m brake, 157 kph drop — hard+short, blasts out
            { 3240, 3470, 3750,  92, 205 },  // C4: 230m brake, 113 kph drop — soft+long, crawls out
            { 4070, 4200, 4370, 110, 200 },  // C5: 130m brake,  90 kph drop
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="RacingTelemetryDataGenerator"/> class.
        /// </summary>
        /// <param name="corners">
        /// Corner definitions where each row represents a segment in the format:
        /// { brake, apex, exit, apexSpeedKph, maxSpeedKph }.
        /// </param>
        public RacingTelemetryDataGenerator(double[,] corners)
        {
            _corners = corners;
        }

        /// <summary>
        /// Creates a uniformly spaced distance axis for a single lap.
        /// </summary>
        /// <param name="totalDistance">
        /// Total lap length in meters.
        /// </param>
        /// <param name="pointCount">
        /// Number of samples to generate across the lap.
        /// </param>
        /// <returns>
        /// A sequence of distance values ranging from 0 to <paramref name="totalDistance"/>,
        /// distributed uniformly according to <paramref name="pointCount"/>.
        /// </returns>
        public double[] GenerateDistanceAxis(double totalDistance = 4500.0, int pointCount = 1000)
        {
            var x = new double[pointCount];
            var step = totalDistance / (pointCount - 1);
            for (int i = 0; i < pointCount; i++)
            {
                x[i] = i * step;
            }

            return x;
        }

        /// <summary>
        /// Creates a uniformly spaced time axis for a single lap.
        /// </summary>
        /// <param name="totalTime">
        /// Total lap duration in seconds.
        /// </param>
        /// <param name="pointCount">
        /// Number of samples to generate across the lap time.
        /// </param>
        /// <returns>
        /// A sequence of time values ranging from 0 to <paramref name="totalTime"/>,
        /// distributed uniformly according to <paramref name="pointCount"/>.
        /// </returns>
        public double[] GenerateTimeAxis(double totalTime = 90.0, int pointCount = 1000)
        {
            var x = new double[pointCount];
            var step = totalTime / (pointCount - 1);
            for (int i = 0; i < pointCount; i++)
            {
                x[i] = i * step;
            }

            return x;
        }

        /// <summary>
        /// Computes time values from distance samples.
        /// </summary>
        /// <param name="distanceX">
        /// Cumulative distance values in meters.
        /// </param>
        /// <returns>
        /// Time values in seconds aligned with <paramref name="distanceX"/>.
        /// </returns>
        public double[] GenerateTimeAxisFromSpeed(double[] distanceX)
        {
            var speedKph = BuildBaseSpeedProfile(distanceX);
            var n = distanceX.Length;
            var timeX = new double[n];
            timeX[0] = 0.0;

            for (int i = 1; i < n; i++)
            {
                var ds = distanceX[i] - distanceX[i - 1];       // metres
                var vMs = Math.Max(1.0, speedKph[i - 1]) / 3.6;  // m/s, clamp prevents /0
                timeX[i] = timeX[i - 1] + ds / vMs;
            }
            
            return timeX;
        }

        private double[] BuildBaseSpeedProfile(double[] distanceX)
        {
            var n = distanceX.Length;
            var nc = _corners.GetLength(0);
            var speed = new double[n];

            for (int i = 0; i < n; i++)
            {
                var d = distanceX[i];
                speed[i] = 200.0;

                for (int c = 0; c < nc; c++)
                {
                    var brake = _corners[c, 0];
                    var apex = _corners[c, 1];
                    var exit = _corners[c, 2];
                    var apexKph = _corners[c, 3];
                    var maxKph = _corners[c, 4];

                    if (d >= brake && d < apex)
                    {
                        var t = (d - brake) / (apex - brake);
                        speed[i] = maxKph - (maxKph - apexKph) * (t * t);
                        break;
                    }
                    if (d >= apex && d < exit)
                    {
                        var t = (d - apex) / (exit - apex);
                        speed[i] = apexKph + (maxKph - apexKph) * Math.Sqrt(t);
                        break;
                    }
                }
            }

            return speed;
        }

        /// <summary>
        /// Generates speed series from distance samples.
        /// </summary>
        /// <param name="distanceX">
        /// Distance values in meters.
        /// </param>
        /// <param name="seed">
        /// Seed used to initialize the random number generator.
        /// </param>
        /// <returns>
        /// A collection of temperature series aligned with <paramref name="distanceX"/>.
        /// </returns>
        public Dictionary<string, double[]> GenerateSpeedPanelData(double[] distanceX, int seed = 42)
        {
            var rng = new Random(seed);
            var baseSpeed = BuildBaseSpeedProfile(distanceX);
            int n = distanceX.Length;

            var gpsSpeed = new double[n];
            var wheelFL = new double[n];
            var wheelFR = new double[n];
            var wheelRL = new double[n];
            var wheelRR = new double[n];
            var gpsSpdB = new double[n];

            var apexZones = GetApexZones();
            int naz = apexZones.GetLength(0);

            bool IsExitZone(double d)
            {
                for (int z = 0; z < naz; z++)
                {
                    if (d >= apexZones[z, 0] && d < apexZones[z, 1]) return true;
                }
                return false;
            }

            for (int i = 0; i < n; i++)
            {
                var s = baseSpeed[i];
                gpsSpeed[i] = s;

                var noiseF = (rng.NextDouble() - 0.5) * 2.5;
                wheelFL[i] = Math.Max(0, s + noiseF);
                wheelFR[i] = Math.Max(0, s - noiseF * 0.7);

                var spinR = IsExitZone(distanceX[i]) ? rng.NextDouble() * 8.0 : (rng.NextDouble() - 0.5) * 2.0;
                wheelRL[i] = Math.Max(0, s + spinR);
                wheelRR[i] = Math.Max(0, s + spinR * 0.85);

                var bFactor = distanceX[i] < 500 ? 1.02 : 0.97;
                gpsSpdB[i] = s * bFactor + (rng.NextDouble() - 0.5) * 1.5;
            }

            return new Dictionary<string, double[]>
            {
                ["GPS_Speed"] = gpsSpeed,
                ["WheelSpeed_FL"] = wheelFL,
                ["WheelSpeed_FR"] = wheelFR,
                ["WheelSpeed_RL"] = wheelRL,
                ["WheelSpeed_RR"] = wheelRR,
                ["GPS_Speed_B"] = gpsSpdB,
            };
        }

        /// <summary>
        /// Generates RPM and throttle series from distance samples.
        /// </summary>
        /// <param name="distanceX">
        /// Distance values in meters.
        /// </param>
        /// <param name="seed">
        /// Seed used to initialize the random number generator.
        /// </param>
        /// <returns>
        /// A collection of temperature series aligned with <paramref name="distanceX"/>.
        /// </returns>
        public Dictionary<string, double[]> GenerateRpmThrottlePanelData(double[] distanceX, int seed = 42)
        {
            var rng = new Random(seed);
            var baseSpeed = BuildBaseSpeedProfile(distanceX);
            int n = distanceX.Length;

            var rpm = new double[n];
            var throttle = new double[n];
            var brake = new double[n];
            var gear = new double[n];
            var drs = new double[n];

            var brakingZones = GetBrakingZones();
            var exitZones = GetApexZones();
            var nbz = brakingZones.GetLength(0);
            var nez = exitZones.GetLength(0);

            // Per-corner brake and throttle intensity
            var brakeMin = new[] { 20, 85, 70, 25, 35 };
            var brakeRange = new[] { 20, 15, 20, 25, 25 };
            var exitThrMin = new[] { 65, 10, 70, 15, 50 };
            var exitThrRange = new[] { 35, 55, 30, 40, 45 };

            int ZoneIndex(double[,] zones, int count, double d)
            {
                for (int z = 0; z < count; z++)
                {
                    if (d >= zones[z, 0] && d < zones[z, 1]) return z;
                }
                return -1;
            }

            int SpeedToGear(double kph)
            {
                if (kph < 60) return 1;
                if (kph < 90) return 2;
                if (kph < 120) return 3;
                if (kph < 150) return 4;
                if (kph < 175) return 5;
                if (kph < 195) return 6;
                if (kph < 210) return 7;
                return 8;
            }

            for (int i = 0; i < n; i++)
            {
                var s = baseSpeed[i];
                var d = distanceX[i];
                var g = SpeedToGear(s);
                gear[i] = g;

                var gearRatio = 1.0 - (g - 1) * 0.08;
                rpm[i] = 2500 + (s / 220.0) * 5500 * gearRatio + (rng.NextDouble() - 0.5) * 150;

                var bzi = ZoneIndex(brakingZones, nbz, d);
                var ezi = ZoneIndex(exitZones, nez, d);

                if (bzi >= 0)
                {
                    throttle[i] = Math.Max(0, 2 + rng.NextDouble() * 4);
                    brake[i] = Math.Min(100, brakeMin[bzi] + rng.NextDouble() * brakeRange[bzi]);
                }
                else if (ezi >= 0)
                {
                    throttle[i] = exitThrMin[ezi] + rng.NextDouble() * exitThrRange[ezi];
                    brake[i] = Math.Max(0, rng.NextDouble() * 3);
                }
                else
                {
                    throttle[i] = 95 + rng.NextDouble() * 5;
                    brake[i] = rng.NextDouble() * 2;
                }

                drs[i] = (d < 770 || (d > 2100 && d < 2580)) ? 1.0 : 0.0;
            }

            // Driveshaft RPM = Engine RPM * gearbox ratio approximation
            var driveshaft = new double[n];
            for (int i = 0; i < n; i++)
            {
                driveshaft[i] = rpm[i] * 0.85;
            }

            return new Dictionary<string, double[]>
            {
                ["RPM_Engine"] = rpm,
                ["RPM_Driveshaft"] = driveshaft,
                ["Throttle"] = throttle,
                ["Brake"] = brake,
                ["Gear"] = gear,
                ["DRS"] = drs,
            };
        }

        /// <summary>
        /// Generates temperature series from cumulative distance samples.
        /// </summary>
        /// <param name="distanceX">
        /// Distance values in meters.
        /// </param>
        /// <param name="seed">
        /// Seed used to initialize the random number generator.
        /// </param>
        /// <returns>
        /// A collection of temperature series aligned with <paramref name="distanceX"/>.
        /// </returns>
        public Dictionary<string, double[]> GenerateTemperaturePanelData(double[] distanceX, int seed = 42)
        {
            var rng = new Random(seed);
            var rpmData = GenerateRpmThrottlePanelData(distanceX, seed);
            var rpmArr = rpmData["RPM_Engine"];
            var throttleArr = rpmData["Throttle"];
            var n = distanceX.Length;

            var exhaust = new double[n];
            var coolant = new double[n];
            var oil = new double[n];

            var coolantVal = 85.0;
            var oilVal = 90.0;

            for (int i = 0; i < n; i++)
            {
                var rpmNorm = (rpmArr[i] - 2500) / 5500.0;
                var throttleNorm = throttleArr[i] / 100.0;

                exhaust[i] = 400 + rpmNorm * 350 + throttleNorm * 200 + (rng.NextDouble() - 0.5) * 30;

                var coolantTarget = 85 + rpmNorm * 25 + throttleNorm * 5;
                coolantVal += (coolantTarget - coolantVal) * 0.02;
                coolant[i] = coolantVal + (rng.NextDouble() - 0.5) * 1.5;

                var oilTarget = 90 + rpmNorm * 35 + throttleNorm * 10;
                oilVal += (oilTarget - oilVal) * 0.008;
                oil[i] = oilVal + (rng.NextDouble() - 0.5) * 2.0;
            }

            return new Dictionary<string, double[]>
            {
                ["Temp_ExhaustGas"] = exhaust,
                ["Temp_Coolant"] = coolant,
                ["Temp_Oil"] = oil,
            };
        }

        private double[,] GetApexZones()
        {
            // apex zones: { start, end } — matches Corners[c,1]..Corners[c,2]
            var rows = _corners.GetLength(0);
            var result = new double[rows, 2];

            for (int i = 0; i < rows; i++)
            {
                result[i, 0] = _corners[i, 1]; // apex
                result[i, 1] = _corners[i, 2]; // exit
            }

            return result;
        }

        private double[,] GetBrakingZones()
        {
            // brakingZones start ~50m before Corners brake point, end at apex
            var rows = _corners.GetLength(0);
            var result = new double[rows, 2];

            for (int i = 0; i < rows; i++)
            {
                result[i, 0] = _corners[i, 0]; // brake start
                result[i, 1] = _corners[i, 1]; // apex (end of braking)
            }

            return result;
        }
    }
}
