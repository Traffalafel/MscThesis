using MscThesis.Core.Formats;
using System.Collections.Generic;

namespace MscThesis.Core.Algorithms
{
    public abstract class OptimizationHeuristic<T> where T : InstanceFormat
    {
        protected readonly int _problemSize;

        protected OptimizationHeuristic(int problemsSize)
        {
            _problemSize = problemsSize;
        }

        protected abstract IterationResult<T> NextIteration(FitnessFunction<T> fitnessFunction);

        public IEnumerable<IterationResult<T>> GetResults(FitnessFunction<T> fitnessFunction)
        {
            while (true)
            {
                yield return NextIteration(fitnessFunction);
            }
        }
    }
}
