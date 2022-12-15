using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.Algorithms.BitStrings;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public class CGAFactory : IOptimizerFactory<BitString>
    {
        private readonly IParameterFactory _parameterFactory;

        public CGAFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.K
        };

        public Func<FitnessFunction<BitString>, VariableSpecification, Optimizer<BitString>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return (problem, varSpec) =>
            {
                var k = parameters(Parameter.K, problem.Size, varSpec);
                return new CGA(problem.Size, problem.Comparison, k);
            };
        }
    }

}
