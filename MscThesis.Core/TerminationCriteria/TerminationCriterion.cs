using MscThesis.Core.Formats;
using System;

namespace MscThesis.Core.TerminationCriteria
{
    public abstract class TerminationCriterion<T> where T : InstanceFormat
    {
        public abstract void Iteration(Population<T> pop);
        public abstract bool ShouldTerminate();

    }
}
