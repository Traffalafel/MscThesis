using MscThesis.Core.Formats;

namespace MscThesis.Core.TerminationCriteria
{
    public class OptimumReached<T> : TerminationCriterion<T> where T : InstanceFormat
    {
        private double _optimum;

        public override string TerminationMessage => "optimum reached";

        public OptimumReached(double optimum)
        {
            _optimum = optimum;
        }

        public override bool ShouldTerminate(Population<T> pop)
        {
            return pop.Fittest.Fitness == _optimum;
        }
    }
}
