using MscThesis.Core;
using MscThesis.Framework.Factories.Problem;
using MscThesis.Framework.Tests;
using MscThesis.Framework.Specification;
using System.Collections.Generic;

namespace MscThesis.Framework.Factories
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
