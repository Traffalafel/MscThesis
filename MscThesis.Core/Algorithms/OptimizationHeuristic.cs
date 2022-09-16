using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core.Algorithms
{
    public abstract class OptimizationHeuristic<T> where T : InstanceFormat
    {
        public abstract Individual<T> Optimize(FitnessFunction<T> function, int problemsSize);
    }
}
