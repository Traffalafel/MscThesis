using MscThesis.Core.Formats;
using System;
using System.Linq;

namespace MscThesis.Core.TerminationCriteria
{

    public abstract class TerminationCriterion<T> where T : InstanceFormat
    {
        public abstract bool ShouldTerminate(Population<T> pop);
        public abstract Property Reason { get; }
    }
}
