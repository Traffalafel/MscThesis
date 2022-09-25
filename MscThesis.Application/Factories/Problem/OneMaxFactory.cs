using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Problem
{
    public class OneMaxFactory : IProblemFactory<BitString>
    {
        public IEnumerable<Parameter> RequiredParameters => new List<Parameter>();

        public FitnessFunction<BitString> BuildProblem(ProblemSpecification _)
        {
            return new OneMax();
        }
    }
}
