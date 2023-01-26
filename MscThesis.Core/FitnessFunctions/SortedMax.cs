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
            var swapCount = 0;
            for (var i = 0; i < instance.Values.Length; i++)
            {
                var diff = Math.Abs(i - instance.Values[i]);
                swapCount += diff;
            }
            return swapCount / 2;
        }
    }
}
