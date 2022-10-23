using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Problem
{
    public class OneMaxFactory : ProblemFactory<BitString>
    {
        public override ProblemDefinition GetDefinition(ProblemSpecification spec)
        {
            return new ProblemDefinition
            {
                CustomSizesAllowed = true,
                ExpressionParameters = new List<Parameter>(),
                OptionParameters = new Dictionary<Parameter, IEnumerable<string>>()
            };
        }

        public override Func<int, FitnessFunction<BitString>> BuildProblem(ProblemSpecification _)
        {
            return (_) => new OneMax();
        }
    }
}
