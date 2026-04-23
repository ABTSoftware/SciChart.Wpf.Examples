// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// StrongTyped.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class StrongTyped<T> : IEquatable<StrongTyped<T>>
    {
        public T Value { get; protected set; }

        public StrongTyped(T value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StrongTyped<T>);
        }

        public bool Equals(StrongTyped<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(other.Value, Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(StrongTyped<T> left, StrongTyped<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StrongTyped<T> left, StrongTyped<T> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}