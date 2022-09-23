using MscThesis.Core.Algorithms;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;

namespace MscThesis.Runner.Factories
{
    public abstract class OptimizerFactory<T> where T : InstanceFormat
    {
        public abstract Optimizer<T> BuildOptimizer(OptimizerSpecification spec);

        protected Random BuildRandom(int? seed)
        {
            if (seed == null)
            {
                return new Random();
            }
            else
            {
                return new Random(seed.Value);
            }
        }
    }

}
