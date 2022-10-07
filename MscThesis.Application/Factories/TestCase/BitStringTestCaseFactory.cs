using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Factories.Termination;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public class BitStringTestCaseFactory : TestCaseFactory<BitString>
    {
        public BitStringTestCaseFactory() : base()
        {
            _optimizers = new Dictionary<string, OptimizerFactory<BitString>>
            {
                { "MIMIC", new MIMICFactory(_parameterFactory) }
            };
            _problems = new Dictionary<string, IProblemFactory<BitString>>
            {
                { "OneMax", new OneMaxFactory() },
                { "JumpOffsetSpike", new JumpOffsetSpikeFactory(_parameterFactory) }
            };
            _terminations = new Dictionary<string, ITerminationFactory<BitString>>
            {
                { "Stagnation", new StagnationFactory<BitString>(_parameterFactory) }
            };
        }
    }
}
