using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;

namespace MscThesis.Runner.Factories
{
    public interface ProblemFactory<T> where T : InstanceFormat
    {
        public FitnessFunction<T> BuildProblem(ProblemSpecification spec);
    }

}
