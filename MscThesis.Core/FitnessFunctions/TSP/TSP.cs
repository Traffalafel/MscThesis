using Microsoft.VisualBasic;
using MscThesis.Core.Formats;
using TspLibNet;

namespace MscThesis.Core.FitnessFunctions.TSP
{
    public class TSP : FitnessFunction<Formats.Tour>
    {
        private TspLib95Item _item;

        public TSP(TspLib95Item item)
        {
            _item = item;
        }

        public override double? Optimum => _item.OptimalTourDistance;

        protected override double Compute(Tour instance)
        {
            return _item.Problem.TourDistance(instance);
        }
    }
}
