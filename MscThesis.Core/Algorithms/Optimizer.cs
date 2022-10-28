using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MscThesis.Core.Algorithms
{
    // 1 configuration of 1 algorithm
    public abstract class Optimizer<T> where T : InstanceFormat
    {
        protected readonly int _problemSize;
        protected readonly FitnessComparisonStrategy _comparisonStrategy;
        protected ThreadLocal<Random> _random;

        public abstract ISet<Property> StatisticsProperties { get; }

        protected Optimizer(int problemSize, FitnessComparisonStrategy comparisonStrategy)
        {
            _problemSize = problemSize;
            _comparisonStrategy = comparisonStrategy;
        }

        protected virtual void Initialize(FitnessFunction<T> fitnessFunction)
        {
            _random = RandomUtils.BuildRandom();
        }

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
