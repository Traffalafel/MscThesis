using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core
{
    public class RunIteration
    {
        public RunIteration()
        {
            Statistics = new Dictionary<Property, double>();
        }

        public IEnumerable<Individual<InstanceFormat>> Population;
        public IDictionary<Property, double> Statistics;
        public TimeSpan CpuTime;
    }
}
