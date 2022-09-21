﻿using MscThesis.Core.Algorithms;
using MscThesis.Core.Formats;
using System.Collections.Generic;

namespace MscThesis.Core.TerminationCriteria
{
    public abstract class TerminationCriterion<T> where T : InstanceFormat
    {
        protected abstract bool ShouldTerminate(Population<T> pop);

        public IEnumerable<IterationResult<T>> AddTerminationCriterion(IEnumerable<IterationResult<T>> results)
        {
            foreach (var result in results)
            {
                yield return result;
                if (ShouldTerminate(result.Population))
                {
                    yield break;
                }
            }
        }

    }
}
