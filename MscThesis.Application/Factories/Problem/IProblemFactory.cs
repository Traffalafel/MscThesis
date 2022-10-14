using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;

namespace MscThesis.Runner.Factories
{
    public interface IProblemFactory<T> where T : InstanceFormat
    {
        public ProblemDefinition Parameters { get; }
        public Func<int, FitnessFunction<T>> BuildProblem(ProblemSpecification spec);
    }

}
