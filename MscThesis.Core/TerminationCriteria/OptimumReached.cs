using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.TerminationCriteria
{
    public class OptimumReached : ITerminationCriterion
    {
        private double _optimum;

        public Property Reason => Property.OptimumReached;

        public OptimumReached(double optimum)
        {
            _optimum = optimum;
        }

        public bool ShouldTerminate(IEnumerable<double?> fitnesses)
        {
            return fitnesses.Any(f => f == _optimum);
        }
    }
}
