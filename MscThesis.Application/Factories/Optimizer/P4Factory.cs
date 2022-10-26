using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.Algorithms.Tours;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Optimizer
{
    public class P4Factory : OptimizerFactory<Tour>
    {
        public override IEnumerable<Parameter> Parameters => new List<Parameter>();

        public override Func<FitnessFunction<Tour>, Optimizer<Tour>> BuildCreator(OptimizerSpecification spec)
        {
            var random = BuildRandom(spec.Seed);

            return problem =>
            {
                return new P4(random, problem.Size, problem);
            };
        }
    }
}
