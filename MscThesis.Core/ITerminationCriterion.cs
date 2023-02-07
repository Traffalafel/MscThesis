using System.Collections.Generic;

namespace MscThesis.Core.TerminationCriteria
{
    public interface ITerminationCriterion
    {
        public bool ShouldTerminate(IEnumerable<double?> fitnesses);
        public Property Reason { get; }
    }
}
