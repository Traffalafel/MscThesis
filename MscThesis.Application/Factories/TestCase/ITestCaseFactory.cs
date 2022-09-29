using MscThesis.Core;
using MscThesis.Core.Formats;
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

        public IEnumerable<Parameter> GetAlgorithmParameters(string algorithmName);
        public IEnumerable<Parameter> GetProblemParameters(string problemName);
        public IEnumerable<Parameter> GetTerminationParameters(string terminationName);
    }
}
