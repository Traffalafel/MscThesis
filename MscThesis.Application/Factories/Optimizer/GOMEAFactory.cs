using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Optimizer
{
    public class GOMEAFactory : IOptimizerFactory<BitString>
    {
        private static readonly int _tournamentSize = 2;
        private readonly IParameterFactory _parameterFactory;

        public GOMEAFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter>
        {
            Parameter.PopulationSize
        };

        public Func<FitnessFunction<BitString>, Optimizer<BitString>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return problem =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problem.Size);
                var selection = new TournamentSelection<BitString>(populationSize, _tournamentSize);
                return new GOMEA(problem.Size, problem.Comparison, populationSize, selection);
            };
        }
    }
}
