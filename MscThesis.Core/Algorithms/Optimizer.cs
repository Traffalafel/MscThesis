using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MscThesis.Core.Algorithms
{
    public abstract class Optimizer<T> where T : InstanceFormat
    {
        protected readonly int _problemSize;
        protected readonly FitnessComparison _comparisonStrategy;
        protected ThreadLocal<Random> _random;

        public abstract ISet<Property> StatisticsProperties { get; }

        protected Optimizer(int problemSize, FitnessComparison comparisonStrategy)
        {
            _problemSize = problemSize;
            _comparisonStrategy = comparisonStrategy;
        }

        protected abstract void Initialize(FitnessFunction<T> fitnessFunction);
        protected abstract RunIteration NextIteration(FitnessFunction<T> fitnessFunction);

        public Run Run(FitnessFunction<T> fitnessFunction)
        {
            _random = RandomUtils.BuildRandom();
            Initialize(fitnessFunction);
            var enumerator = BuildEnumerator(fitnessFunction);
            return new Run(enumerator);
        }

        private IEnumerable<RunIteration> BuildEnumerator(FitnessFunction<T> fitnessFunction)
        {
            var stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();
                stopwatch.Start();
                var iteration = NextIteration(fitnessFunction);
                stopwatch.Stop();

                iteration.CpuTime = stopwatch.Elapsed;

                yield return iteration;
            }
        }

    }
}
