using MscThesis.Core.InstanceFormats;
using System;

namespace MscThesis.Core
{
    public abstract class TerminationCriterion<T> where T : InstanceFormat
    {
        public abstract void Iteration(Population<T> pop);
        public abstract bool ShouldTerminate();

    }
}
