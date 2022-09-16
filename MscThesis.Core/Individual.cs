using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core
{
    public class Individual<T> where T : InstanceFormat
    {
        public Individual(T value)
        {
            Value = value;
        }

        public T Value { get; }
        public double? Fitness { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
