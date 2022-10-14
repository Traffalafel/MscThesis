using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Specification;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public interface ITestCaseFactory<out T> where T : InstanceFormat
    {
        public ISet<string> Algorithms { get; }
        public ISet<string> Problems { get; }
        public ISet<string> Terminations { get; }

        public IEnumerable<ITestCase<InstanceFormat>> BuildTestCases(TestSpecification spec);

        public ProblemDefinition GetProblemDefinition(string problemName);
        public ProblemInformation GetProblemInformation(ProblemSpecification specification);
        public IEnumerable<Parameter> GetAlgorithmParameters(string algorithmName);
        public IEnumerable<Parameter> GetTerminationParameters(string terminationName);
    }
}
