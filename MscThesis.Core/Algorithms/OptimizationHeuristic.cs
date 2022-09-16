using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core
{
    public abstract class OptimizationHeuristic<T> where T : InstanceFormat
    {
        public abstract T Optimize(FitnessFunction<T> function, int problemsSize);
    }
}
