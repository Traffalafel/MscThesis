using MscThesis.Core.Formats;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MscThesis.Core
{
    public class IndividualImpl<T> : Individual<T> where T : InstanceFormat
    {
        public IndividualImpl(T value)
        {
            Value = value;
        }
        public IndividualImpl(T value, double fitness)
        {
            Value = value;
            Fitness = fitness;
        }

        public T Value { get; }
        public double? Fitness { get; set; }
        public int Size { get
            {
                return Value.GetSize();
            } 
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
