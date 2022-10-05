using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public class MIMICFactory : OptimizerFactory<BitString>
    {
        private static double _selectionQuartile = 0.5d;

        public override IEnumerable<Parameter> RequiredParameters => new List<Parameter> 
        { 
            Parameter.InitialPopulationSize
        };

        public override Optimizer<BitString> BuildOptimizer(OptimizerSpecification spec)
        {
            var initialPopSize = Convert.ToInt32(spec.Parameters[Parameter.InitialPopulationSize]);

            var random = BuildRandom(spec.Seed);
            var selection = new QuartileSelection<BitString>(_selectionQuartile);
            return new MIMIC(random, initialPopSize, selection);
        }
    }

}
