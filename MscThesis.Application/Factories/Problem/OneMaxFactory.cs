using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;

namespace MscThesis.Runner.Factories.Problem
{
    public class OneMaxFactory : ProblemFactory<BitString>
    {
        public FitnessFunction<BitString> BuildProblem(ProblemSpecification _)
        {
            return new OneMax();
        }
    }
}
