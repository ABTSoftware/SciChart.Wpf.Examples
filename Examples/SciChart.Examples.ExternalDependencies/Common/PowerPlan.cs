// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PowerPlan.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Diagnostics.CodeAnalysis;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class PowerPlan
    {
        public readonly string Name;

        public Guid Guid;

        public PowerPlan(string name, Guid guid)
        { 
            Name = name; 
            Guid = guid;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PowerPlan);
        }

        protected bool Equals(PowerPlan other)
        {
            return string.Equals(Name, other.Name) && Guid.Equals(other.Guid);
        }

        [SuppressMessage("SonarQube", "S2328:GetHashCode should not reference mutable fields")]
        public override int GetHashCode()
        {
            unchecked
            {
                return (Guid.GetHashCode() * 397) ^ (Name?.GetHashCode() ?? 0);
            }
        }
    }
}