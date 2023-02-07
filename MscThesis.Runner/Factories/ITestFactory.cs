using MscThesis.Core;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Tests;
using MscThesis.Runner.Specification;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public interface ITestFactory
    {
        public ISet<string> Algorithms { get; }
        public ISet<string> Problems { get; }
        public ISet<string> Terminations { get; }

        public ITest BuildTest(TestSpecification spec);

        public ProblemDefinition GetProblemDefinition(ProblemSpecification specification);
        public ProblemInformation GetProblemInformation(ProblemSpecification specification);
        public IEnumerable<Parameter> GetAlgorithmParameters(string algorithmName);
        public IEnumerable<Parameter> GetTerminationParameters(string terminationName);
    }
}
