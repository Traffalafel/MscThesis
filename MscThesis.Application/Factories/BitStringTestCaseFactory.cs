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
                { "cGA", new CGAFactory(_parameterFactory) },
                { "MIMIC", new MIMICFactory(_parameterFactory) },
                { "FastMIMIC", new FastMIMICFactory(_parameterFactory) },
                { "GOMEA", new GOMEAFactory(_parameterFactory) },
                { "P3", new P3Factory() },
            };
            _problems = new Dictionary<string, IProblemFactory<BitString>>
            {
                { "OneMax", new OneMaxFactory() },
                { "LeadingOnes", new LeadingOnesFactory() },
                { "JumpOffsetSpike", new JumpOffsetSpikeFactory(_parameterFactory) },
                { "DeceptiveLeadingBlocks", new DeceptiveLeadingBlocksFactory(_parameterFactory) }
            };
            _terminations = new Dictionary<string, ITerminationFactory<BitString>>
            {
                { "Optimum reached", new GlobalOptimumFactory<BitString>() },
                { "Stagnation", new StagnationFactory<BitString>(_parameterFactory) },
            };
        }
    }
}
