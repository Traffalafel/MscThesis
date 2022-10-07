using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Termination;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public class PermutationTestCaseFactory : TestCaseFactory<Permutation>
    {
        public PermutationTestCaseFactory()
        {
            _optimizers = new Dictionary<string, OptimizerFactory<Permutation>>
            {
            };
            _problems = new Dictionary<string, IProblemFactory<Permutation>>
            {
            };
            _terminations = new Dictionary<string, ITerminationFactory<Permutation>>
            {
                { "Stagnation", new StagnationFactory<Permutation>(_parameterFactory) }
            };
        }
    }
}
