using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public class MIMICTSPFactory : OptimizerFactory<Permutation>
    {
        private static readonly double _selectionQuartile = 0.5d;

        private readonly IParameterFactory _parameterFactory;

        public MIMICTSPFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public override IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.PopulationSize
        };

        public override Func<int, Optimizer<Permutation>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            var random = BuildRandom(spec.Seed);
            var selection = new QuartileSelection<Permutation>(_selectionQuartile);

            return problemSize =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problemSize);
                return new MIMICTSP(random, problemSize, populationSize, selection);
            };
        }
    }

}
