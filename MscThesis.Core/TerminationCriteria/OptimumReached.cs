using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.TerminationCriteria
{
    public class OptimumReached : TerminationCriterion
    {
        private double _optimum;

        public override Property Reason => Property.OptimumReached;

        public OptimumReached(double optimum)
        {
            _optimum = optimum;
        }

        public override bool ShouldTerminate(IEnumerable<double?> fitnesses)
        {
            return fitnesses.Any(f => f == _optimum);
        }
    }
}
