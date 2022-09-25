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
        public override IEnumerable<Parameter> RequiredParameters => new List<Parameter> 
        { 
            Parameter.InitialPopulationSize,
            Parameter.SelectionQuartile,
        };

        public override Optimizer<BitString> BuildOptimizer(OptimizerSpecification spec)
        {
            var initialPopSize = Convert.ToInt32(spec.Parameters[Parameter.InitialPopulationSize]);
            var quartile = spec.Parameters[Parameter.SelectionQuartile];

            var random = BuildRandom(spec.Seed);
            var selection = new QuartileSelection<BitString>(quartile);
            return new MIMIC(random, initialPopSize, selection);
        }
    }

}
