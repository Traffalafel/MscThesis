using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Framework.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Framework.Factories.Problem
{
    public class LeadingOnesFactory : ProblemFactory<BitString>
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

        public override Func<int, VariableSpecification, FitnessFunction<BitString>> BuildProblem(ProblemSpecification _)
        {
            return (size, _) => new LeadingOnes(size);
        }
    }
}
