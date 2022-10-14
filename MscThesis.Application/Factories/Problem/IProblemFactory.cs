using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Specification;
using System;

namespace MscThesis.Runner.Factories
{
    public interface IProblemFactory<T> where T : InstanceFormat
    {
        public ProblemDefinition Definition { get; }
        public ProblemInformation GetInformation(ProblemSpecification spec);
        public Func<int, FitnessFunction<T>> BuildProblem(ProblemSpecification spec);
    }

}
