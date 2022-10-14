using MscThesis.Core.Formats;
using TspLibNet;
using TspLibNet.Tours;

namespace MscThesis.Core.FitnessFunctions.TSP
{
    public class TSP : FitnessFunction<Permutation>
    {
        private TspLib95Item _item;

        public TSP(TspLib95Item item)
        {
            _item = item;
        }

        public override double? Optimum => _item.OptimalTourDistance;

        protected override double Compute(Permutation instance)
        {
            var tour = new Tour(string.Empty, string.Empty, instance.Size, instance);
            return _item.Problem.TourDistance(tour);
        }
    }
}
