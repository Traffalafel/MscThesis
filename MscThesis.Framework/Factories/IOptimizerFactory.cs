using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Framework.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Framework.Factories
{
    public interface IOptimizerFactory<T> where T : InstanceFormat
    {
        public IEnumerable<Parameter> Parameters { get; }
        public Func<FitnessFunction<T>, VariableSpecification, Optimizer<T>> BuildCreator(OptimizerSpecification spec);
    }

}
