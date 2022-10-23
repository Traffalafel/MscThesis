using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;

namespace MscThesis.Runner.Factories.Problem
{
    public abstract class ProblemFactory<T> : IProblemFactory<T> where T : InstanceFormat
    {
        public abstract Func<int, FitnessFunction<T>> BuildProblem(ProblemSpecification spec);

        public virtual ProblemDefinition GetDefinition(ProblemSpecification spec)
        {
            return new ProblemDefinition();
        }
        public virtual ProblemInformation GetInformation(ProblemSpecification spec)
        {
            return new ProblemInformation();
        }
    }
}
