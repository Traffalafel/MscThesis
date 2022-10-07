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
        private static double _selectionQuartile = 0.5d;

        private IParameterFactory _parameterFactory;

        public MIMICFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public override IEnumerable<Parameter> RequiredParameters => new List<Parameter> 
        { 
            Parameter.InitialPopulationSize
        };

        public override Func<int, Optimizer<BitString>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            var random = BuildRandom(spec.Seed);
            var selection = new QuartileSelection<BitString>(_selectionQuartile);

            return size =>
            {
                var initialPopSize = (int)parameters.Invoke(Parameter.InitialPopulationSize, size);
                return new MIMIC(random, initialPopSize, selection);
            };
        }
    }

}
