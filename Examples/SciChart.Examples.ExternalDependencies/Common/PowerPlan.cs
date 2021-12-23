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