using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core.TerminationCriteria
{
    public abstract class TerminationCriterion<T> where T : InstanceFormat
    {
        protected abstract bool ShouldTerminate(Population<T> pop);
        protected abstract string Message { get; }

        public IEnumerable<RunIteration<T>> AddTerminationCriterion(IEnumerable<RunIteration<T>> results)
        {
            foreach (var result in results)
            {
                yield return result;
                if (ShouldTerminate(result.Population))
                {
                    var size = result.Population.Size;
                    Console.WriteLine($"Run terminated of size {size} due to {Message}");

                    yield break;
                }
            }
        }

    }
}
