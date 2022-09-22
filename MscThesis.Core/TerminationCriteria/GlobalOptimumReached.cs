using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core.TerminationCriteria
{
    public class GlobalOptimumReached<T> : TerminationCriterion<T> where T : InstanceFormat
    {
        protected override bool ShouldTerminate(Population<T> pop)
        {
            throw new NotImplementedException();
        }
    }
}
