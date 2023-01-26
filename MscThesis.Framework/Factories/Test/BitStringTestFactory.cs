using MscThesis.Core.Formats;
using MscThesis.Framework.Factories.Optimizer;
using MscThesis.Framework.Factories.Problem;
using MscThesis.Framework.Factories.Termination;
using System.Collections.Generic;

namespace MscThesis.Framework.Factories
{
    public class BitStringTestFactory : TestFactory<BitString>
    {
        public BitStringTestFactory() : base()
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
