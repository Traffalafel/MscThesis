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
    public class P4Factory : IOptimizerFactory<Tour>
    {
        public IEnumerable<Parameter> Parameters => new List<Parameter>();

        public Func<FitnessFunction<Tour>, Optimizer<Tour>> BuildCreator(OptimizerSpecification spec)
        {
            return problem =>
            {
                return new P4(problem.Size, problem);
            };
        }
    }
}
