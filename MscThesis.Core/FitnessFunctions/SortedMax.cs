using MscThesis.Core.Formats;
using System;

namespace MscThesis.Core.FitnessFunctions.TSP
{
    public class SortedMax : FitnessFunction<Tour>
    {
        public override FitnessComparison Comparison => FitnessComparison.Minimization;

        public SortedMax(int size) : base(size) { }

        public override double? Optimum(int problemSize)
        {
            return 0;
        }

        protected override double Compute(Tour instance)
        {
            return NumSwaps(instance);
        }

        private int NumSwaps(Tour tour)
        {
            var swapCount = 0;
            var pos = 0;
            foreach (var val in tour)
            {
                var diff = Math.Abs(pos - val);
                swapCount += diff;
                pos++;
            }
            return swapCount / 2;
        }
    }
}
