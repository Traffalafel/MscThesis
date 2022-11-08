using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Optimizer
{
    public class P3Factory : IOptimizerFactory<BitString>
    {
        public IEnumerable<Parameter> Parameters => new List<Parameter>();

        public Func<FitnessFunction<BitString>, Optimizer<BitString>> BuildCreator(OptimizerSpecification spec)
        {
            return problem =>
            {
                return new P3(problem.Size, problem);
            };
        }
    }
}
