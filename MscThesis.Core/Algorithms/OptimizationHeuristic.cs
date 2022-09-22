using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core.Algorithms
{
    public abstract class OptimizationHeuristic<T> where T : InstanceFormat
    {
        protected readonly int _problemSize;
        protected readonly Random _random;

        protected OptimizationHeuristic(int problemSize, Random random)
        {
            _problemSize = problemSize;
            _random = random;
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
