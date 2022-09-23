using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Factories.Termination;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Runner.Factories
{
    public class BitStringTestFactory : TestFactory<BitString>
    {
        public BitStringTestFactory()
        {
            _optimizers = new Dictionary<string, OptimizerFactory<BitString>>
            {
                { "MIMIC", new MIMICFactory() }
            };
            _problems = new Dictionary<string, ProblemFactory<BitString>>
            {
                { "OneMax", new OneMaxFactory() },
                { "JumpOffsetSpike", new JumpOffsetSpikeFactory() }
            };
            _terminations = new Dictionary<string, TerminationFactory<BitString>>
            {
                { "Stagnation", new StagnationFactory<BitString>() }
            };
        }
    }
}
