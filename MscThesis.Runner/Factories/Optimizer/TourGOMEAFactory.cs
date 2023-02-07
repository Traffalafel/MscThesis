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
    public class TourGOMEAFactory : IOptimizerFactory<Tour>
    {
        private readonly ParameterFactory _parameterFactory;

        public TourGOMEAFactory(ParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter>
        {
            Parameter.PopulationSize
        };

        public Func<FitnessFunction<Tour>, VariableSpecification, Optimizer<Tour>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return (problem, varSpec) =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problem.Size, varSpec);
                return new TourGOMEA(problem.Size, populationSize);
            };
        }
    }
}
