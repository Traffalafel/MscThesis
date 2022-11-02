using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core
{
    public class RunIteration<T> where T : InstanceFormat
    {
        public RunIteration() { }
        public RunIteration(Population<T> population)
        {
            Population = population;
            Statistics = new Dictionary<Property, double>();
        }

        public Population<T> Population;
        public IDictionary<Property, double> Statistics;
        public TimeSpan CpuTime;
    }
}
