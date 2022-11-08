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
    public static class ThreadIdProvider
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }

    // 1 configuration of 1 algorithm
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
            var processThread = GetCurrentProcessThread();
            while (true)
            {
                var prevTime = processThread?.UserProcessorTime;
                var iteration = NextIteration(fitnessFunction);

                if (processThread != null)
                {
                    iteration.CpuTime = processThread.UserProcessorTime - prevTime.Value;
                }

                yield return iteration;
            }
        }

        private ProcessThread GetCurrentProcessThread()
        {
            uint currentThreadId;
            try
            {
                currentThreadId = ThreadIdProvider.GetCurrentThreadId();
            }
            catch
            {
                return null;
            }

            var processThreads = Process.GetCurrentProcess().Threads.Cast<ProcessThread>();
            var processThread = processThreads.FirstOrDefault(pt => pt.Id == currentThreadId);
            if (processThread == null)
            {
                throw new Exception("Could not find current thread");
            }
            return processThread;
        }
    }
}
