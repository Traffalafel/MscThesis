using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public interface IOptimizerFactory<T> where T : Instance
    {
        public IEnumerable<Parameter> Parameters { get; }
        public Func<FitnessFunction<T>, VariableSpecification, Optimizer<T>> BuildCreator(OptimizerSpecification spec);
    }

}
