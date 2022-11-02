using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using System.Collections;
using System.Collections.Generic;

namespace MscThesis.Core
{
    public class Run<T> : IEnumerable<RunIteration<T>> where T : InstanceFormat
    {
        private IEnumerable<RunIteration<T>> _iterator;
        private List<TerminationCriterion<T>> _terminations;
        private Property _terminationReason;

        public Property TerminationReason => _terminationReason;

        public Run(IEnumerable<RunIteration<T>> iterator)
        {
            _iterator = iterator;
            _terminations = new List<TerminationCriterion<T>>();
        }

        public void AddTerminationCriterion(TerminationCriterion<T> termination)
        {
            _terminations.Add(termination);
        }

        public IEnumerator<RunIteration<T>> GetEnumerator()
        {
            return BuildIterator().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<RunIteration<T>> BuildIterator()
        {
            foreach (var iteration in _iterator)
            {
                yield return iteration;
                foreach (var termination in _terminations)
                {
                    if (termination.ShouldTerminate(iteration.Population))
                    {
                        _terminationReason = termination.Reason;
                        yield break;
                    }
                }
            }
        }

    }
}
