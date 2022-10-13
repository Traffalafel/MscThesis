using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core.Algorithms
{
    // 1 configuration of 1 algorithm
    public abstract class Optimizer<T> where T : InstanceFormat
    {
        protected readonly Random _random;
        protected readonly int _problemSize;

        public abstract ISet<Property> StatisticsProperties { get; }

        protected Optimizer(Random random, int problemSize)
        {
            _random = random;
            _problemSize = problemSize;
        }

        protected virtual void Initialize(FitnessFunction<T> fitnessFunction) { }
        protected abstract RunIteration<T> NextIteration(FitnessFunction<T> fitnessFunction);

        public IEnumerable<RunIteration<T>> Run(FitnessFunction<T> fitnessFunction)
        {
            Initialize(fitnessFunction);
            while (true)
            {
                yield return NextIteration(fitnessFunction);
            }
        }
    }
}
