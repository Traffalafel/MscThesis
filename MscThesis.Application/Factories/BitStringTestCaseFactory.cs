using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Optimizer;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Factories.Termination;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public class BitStringTestCaseFactory : TestCaseFactory<BitString>
    {
        public BitStringTestCaseFactory() : base()
        {
            _optimizers = new Dictionary<string, IOptimizerFactory<BitString>>
            {
                { "MIMIC", new MIMICFactory(_parameterFactory) },
                { "FastMIMIC", new FastMIMICFactory(_parameterFactory) },
                { "GOMEA", new GOMEAFactory(_parameterFactory) },
                { "P3", new P3Factory() },
            };
            _problems = new Dictionary<string, IProblemFactory<BitString>>
            {
                { "OneMax", new OneMaxFactory() },
                { "JumpOffsetSpike", new JumpOffsetSpikeFactory(_parameterFactory) }
            };
            _terminations = new Dictionary<string, ITerminationFactory<BitString>>
            {
                { "Optimum reached", new GlobalOptimumFactory<BitString>() },
                { "Stagnation", new StagnationFactory<BitString>(_parameterFactory) },
                { "Max iterations", new MaxIterationsFactory<BitString>(_parameterFactory) }
            };
        }
    }
}
