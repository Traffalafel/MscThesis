using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.FitnessFunctions.TSP;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Problem
{
    public class SortedMaxFactory : ProblemFactory<Tour>
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

        public override Func<int, VariableSpecification, FitnessFunction<Tour>> BuildProblem(ProblemSpecification spec)
        {
            return (size, _) => new SortedMax(size);
        }
    }
}
