using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public abstract class OptimizerFactory<T> : IParameterProvider where T : InstanceFormat
    {
        public abstract IEnumerable<Parameter> RequiredParameters { get; }

        public abstract Func<int, Optimizer<T>> BuildCreator(OptimizerSpecification spec);

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
