using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

        protected virtual void Initialize(FitnessFunction<T> fitnessFunction)
        {
            _random = RandomUtils.BuildRandom();
        }

        protected abstract RunIteration<T> NextIteration(FitnessFunction<T> fitnessFunction);

        public Run<T> Run(FitnessFunction<T> fitnessFunction)
        {
            Initialize(fitnessFunction);
            var enumerator = BuildEnumerator(fitnessFunction);
            return new Run<T>(enumerator);
        }

        private IEnumerable<RunIteration<T>> BuildEnumerator(FitnessFunction<T> fitnessFunction)
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
