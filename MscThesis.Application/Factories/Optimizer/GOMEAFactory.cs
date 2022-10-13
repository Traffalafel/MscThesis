using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Optimizer
{
    public class GOMEAFactory : OptimizerFactory<BitString>
    {
        private static readonly int _tournamentSize = 2;
        private readonly IParameterFactory _parameterFactory;

        public GOMEAFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public override IEnumerable<Parameter> RequiredParameters => new List<Parameter>
        {
            Parameter.PopulationSize
        };

        public override Func<int, Optimizer<BitString>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            var random = BuildRandom(spec.Seed);

            return problemSize =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problemSize);
                var selection = new TournamentSelection<BitString>(random, populationSize, _tournamentSize);
                return new GOMEA(random, problemSize, populationSize, selection);
            };
        }
    }
}
