using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Optimizer;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Factories.Termination;
using System.Collections.Generic;
using TspLibNet;

namespace MscThesis.Runner.Factories
{
    public class TourTestCaseFactory : TestCaseFactory<Tour>
    {
        public TourTestCaseFactory(TspLib95 tspLib)
        {
            _optimizers = new Dictionary<string, IOptimizerFactory<Tour>>
            {
                { "TourMIMIC", new TourMIMICFactory(_parameterFactory) },
                { "P4", new P4Factory() }
            };
            _problems = new Dictionary<string, IProblemFactory<Tour>>
            {
                { "TSPLib", new TSPLibFactory(tspLib) }
            };
            _terminations = new Dictionary<string, ITerminationFactory<Tour>>
            {
                { "Optimum", new GlobalOptimumFactory<Tour>() },
                { "Stagnation", new StagnationFactory<Tour>(_parameterFactory) },
                { "Max iterations", new MaxIterationsFactory<Tour>(_parameterFactory) }
            };
        }
    }
}
