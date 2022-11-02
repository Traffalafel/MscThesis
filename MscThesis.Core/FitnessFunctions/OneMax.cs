using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MscThesis.Core.FitnessFunctions
{
    public class OneMax : FitnessFunction<BitString>
    {
        public override FitnessComparison Comparison => FitnessComparison.Maximization;

        public OneMax(int size) : base(size)
        {

        }

        public override double? Optimum(int problemSize)
        {
            return problemSize;
        }

        protected override double Compute(BitString instance)
        {
            return instance.Values.Where(val => val == true).Count();
        }
    }
}
