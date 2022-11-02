using MscThesis.Core.Formats;
using TspLibNet;

namespace MscThesis.Core.FitnessFunctions.TSP
{
    public class TSP : FitnessFunction<Tour>
    {
        private readonly TspLib95Item _item;

        public TSP(TspLib95Item item) : base(item.Problem.NodeProvider.CountNodes()) // problem size irrelevant
        {
            _item = item;
        }

        public override FitnessComparison Comparison => FitnessComparison.Minimization;

        public override double? Optimum(int problemSize)
        {
            return _item.OptimalTourDistance;
        }

        protected override double Compute(Tour instance)
        {
            return _item.Problem.TourDistance(instance);
        }
    }
}
