using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Problem
{
    public class OneMaxFactory : IProblemFactory<BitString>
    {
        public ProblemDefinition Parameters => new ProblemDefinition
        {
            AllowsMultipleSizes = true,
            ExpressionParameters = new List<Parameter>(),
            OptionParameters = new Dictionary<Parameter, IEnumerable<string>>()
        };

        public Func<int, FitnessFunction<BitString>> BuildProblem(ProblemSpecification _)
        {
            return (_) => new OneMax();
        }
    }
}
