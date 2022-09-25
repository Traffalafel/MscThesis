using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;

namespace MscThesis.Runner.Factories
{
    public interface IProblemFactory<T> : IParameterProvider where T : InstanceFormat
    {
        public FitnessFunction<T> BuildProblem(ProblemSpecification spec);
    }

}
