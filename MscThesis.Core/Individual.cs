using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace MscThesis.Core
{
    public class Individual<T> : IComparable<Individual<T>> where T : InstanceFormat
    {
        public Individual(T value)
        {
            Value = value;
        }

        public T Value { get; }
        public double? Fitness { get; set; }

        public int CompareTo([AllowNull] Individual<T> other)
        {
            if (other == null)
            {
                return 1;
            }
            if (other.Fitness == null && Fitness == null)
            {
                return 0;
            }
            if (other.Fitness != null && Fitness == null)
            {
                return -1;
            }
            if (other.Fitness == null && Fitness != null)
            {
                return 1;
            }

            var diff = Fitness.Value - other.Fitness.Value;
            return diff == 0.0d ? 0 : diff > 0 ? 1 : -1;
        }

        public static bool operator <(Individual<T> i1, Individual<T> i2)
        {
            return i1.CompareTo(i2) < 0;
        }
        public static bool operator >(Individual<T> i1, Individual<T> i2)
        {
            return i1.CompareTo(i2) > 0;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
