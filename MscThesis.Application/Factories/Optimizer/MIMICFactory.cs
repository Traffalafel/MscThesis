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
    public class MIMICFactory : OptimizerFactory<BitString>
    {
        private static readonly double _selectionQuartile = 0.5d;

        private readonly IParameterFactory _parameterFactory;

        public MIMICFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public override IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.PopulationSize
        };

        public override Func<int, Optimizer<BitString>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            var random = BuildRandom(spec.Seed);
            var selection = new QuartileSelection<BitString>(_selectionQuartile, SelectionMethod.Maximize);

            return problemSize =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problemSize);
                return new MIMIC(random, problemSize, populationSize, selection);
            };
        }
    }

}
