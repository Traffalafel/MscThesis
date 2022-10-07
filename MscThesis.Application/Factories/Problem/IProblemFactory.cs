using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;

namespace MscThesis.Runner.Factories
{
    public interface IProblemFactory<T> : IParameterProvider where T : InstanceFormat
    {
        public Func<int, FitnessFunction<T>> BuildProblem(ProblemSpecification spec);
    }

}
