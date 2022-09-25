using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Termination;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public class PermutationTestFactory : TestFactory<Permutation>
    {
        public PermutationTestFactory()
        {
            _optimizers = new Dictionary<string, OptimizerFactory<Permutation>>
            {
            };
            _problems = new Dictionary<string, IProblemFactory<Permutation>>
            {
            };
            _terminations = new Dictionary<string, ITerminationFactory<Permutation>>
            {
                { "Stagnation", new StagnationFactory<Permutation>() }
            };
        }
    }
}
