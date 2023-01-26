using System.Collections.Generic;

namespace MscThesis.Core.TerminationCriteria
{
    public abstract class TerminationCriterion
    {
        public abstract bool ShouldTerminate(IEnumerable<double?> fitnesses);
        public abstract Property Reason { get; }
    }
}
