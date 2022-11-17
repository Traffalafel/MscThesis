using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.Algorithms.Tours;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Optimizer
{
    public class FastP4Factory : IOptimizerFactory<Tour>
    {
        private readonly IParameterFactory _parameterFactory;

        public FastP4Factory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter>
        {
            Parameter.SheddingInterval
        };

        public Func<FitnessFunction<Tour>, Optimizer<Tour>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return problem =>
            {
                var sheddingInterval = (int)parameters.Invoke(Parameter.SheddingInterval, problem.Size);
                return new FastP4(problem.Size, sheddingInterval, problem);
            };
        }
    }
}
