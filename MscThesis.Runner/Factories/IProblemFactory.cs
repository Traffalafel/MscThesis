using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Specification;
using System;

namespace MscThesis.Runner.Factories
{
    public interface IProblemFactory<T> where T : Instance
    {
        public ProblemDefinition GetDefinition(ProblemSpecification spec);
        public ProblemInformation GetInformation(ProblemSpecification spec);
        public Func<int, VariableSpecification, FitnessFunction<T>> BuildProblem(ProblemSpecification spec);
    }

}
