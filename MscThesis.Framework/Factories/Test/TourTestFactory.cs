using MscThesis.Core.Formats;
using MscThesis.Framework.Factories.Optimizer;
using MscThesis.Framework.Factories.Problem;
using MscThesis.Framework.Factories.Termination;
using System.Collections.Generic;
using TspLibNet;

namespace MscThesis.Framework.Factories
{
    public class TourTestFactory : TestFactory<Tour>
    {
        public TourTestFactory(TspLib95 tspLib)
        {
            _optimizers = new Dictionary<string, IOptimizerFactory<Tour>>
            {
                { "TourMIMIC", new TourMIMICFactory(_parameterFactory) },
                { "FastTourMIMIC", new FastTourMIMICFactory(_parameterFactory) },
                { "P4", new P4Factory() },
                { "FastP4", new FastP4Factory(_parameterFactory) },
                { "TourGOMEA", new TourGOMEAFactory(_parameterFactory) }
            };
            _problems = new Dictionary<string, IProblemFactory<Tour>>
            {
                { "TSPLib", new TSPLibFactory(tspLib) },
                { "PerturbedTSPLib", new PerturbedTSPLibFactory(tspLib, _parameterFactory) },
                { "UniformTSP", new UniformTSPFactory() },
                { "SortedMax", new SortedMaxFactory() }
            };
            _terminations = new Dictionary<string, ITerminationFactory<Tour>>
            {
                { "Optimum reached", new GlobalOptimumFactory<Tour>() },
                { "Stagnation", new StagnationFactory<Tour>(_parameterFactory) },
            };
        }
    }
}
