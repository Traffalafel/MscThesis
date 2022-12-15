using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public abstract class OptimizerFactory<T> : IOptimizerFactory<T> where T : InstanceFormat
    {
        public abstract IEnumerable<Parameter> Parameters { get; }
        public abstract Func<FitnessFunction<T>, VariableSpecification, Optimizer<T>> BuildCreator(OptimizerSpecification spec);


    }

}
