﻿using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Factories.Termination;
using System.Collections.Generic;
using TspLibNet;

namespace MscThesis.Runner.Factories
{
    public class PermutationTestCaseFactory : TestCaseFactory<Permutation>
    {
        public PermutationTestCaseFactory(TspLib95 tspLib)
        {
            _optimizers = new Dictionary<string, OptimizerFactory<Permutation>>
            {
                { "MIMICTSP", new MIMICTSPFactory(_parameterFactory) }
            };
            _problems = new Dictionary<string, IProblemFactory<Permutation>>
            {
                { "TSPLib", new TSPLibFactory(tspLib) }
            };
            _terminations = new Dictionary<string, ITerminationFactory<Permutation>>
            {
                { "Optimum reached", new GlobalOptimumFactory<Permutation>() },
                { "Stagnation", new StagnationFactory<Permutation>(_parameterFactory) },
                { "Max iterations", new MaxIterationsFactory<Permutation>(_parameterFactory) }
            };
        }
    }
}
