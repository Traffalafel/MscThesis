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

        public abstract ISet<Property> StatisticsProperties { get; }

        protected Optimizer(Random random)
        {
            _random = random;
        }

        protected abstract Population<T> Initialize(int size);
        protected abstract RunIteration<T> NextIteration(Population<T> population, FitnessFunction<T> fitnessFunction);

        public IEnumerable<RunIteration<T>> Run(FitnessFunction<T> fitnessFunction, int size)
        {
            var population = Initialize(size);
            while (true)
            {
                var iteration = NextIteration(population, fitnessFunction);
                population = iteration.Population;
                yield return iteration;
            }
        }
    }
}
