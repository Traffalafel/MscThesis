using MscThesis.Core.TerminationCriteria;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core
{
    public class Run : IEnumerable<RunIteration>
    {
        private IEnumerable<RunIteration> _iterator;
        private List<TerminationCriterion> _terminations;
        private Property _terminationReason;

        public Property TerminationReason => _terminationReason;

        public Run(IEnumerable<RunIteration> iterator)
        {
            _iterator = iterator;
            _terminations = new List<TerminationCriterion>();
        }

        public void AddTerminationCriterion(TerminationCriterion termination)
        {
            _terminations.Add(termination);
        }

        public IEnumerator<RunIteration> GetEnumerator()
        {
            return BuildIterator().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<RunIteration> BuildIterator()
        {
            foreach (var iteration in _iterator)
            {
                yield return iteration;
                foreach (var termination in _terminations)
                {
                    var fitnesses = iteration.Population.Select(i => i.Fitness);
                    if (termination.ShouldTerminate(fitnesses))
                    {
                        _terminationReason = termination.Reason;
                        yield break;
                    }
                }
            }
        }

    }
}
