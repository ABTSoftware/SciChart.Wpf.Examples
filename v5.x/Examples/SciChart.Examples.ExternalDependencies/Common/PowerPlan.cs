using System;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class PowerPlan
    {
        public readonly string Name;
        public Guid Guid;

        public PowerPlan(string name, Guid guid)
        {
            this.Name = name;
            this.Guid = guid;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PowerPlan);
        }

        protected bool Equals(PowerPlan other)
        {
            return string.Equals(Name, other.Name) && Guid.Equals(other.Guid);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ Guid.GetHashCode();
            }
        }
    }
}