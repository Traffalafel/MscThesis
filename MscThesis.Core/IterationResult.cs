using MscThesis.Core.Formats;
using System.Collections.Generic;

namespace MscThesis.Core
{
    public class IterationResult<T> where T : InstanceFormat
    {
        public Population<T> Population;
        public IDictionary<Property, double> Statistics;
    }
}
