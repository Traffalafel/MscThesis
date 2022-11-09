using MscThesis.Core.Formats;

namespace MscThesis.Core.FitnessFunctions
{
    public class LeadingOnes : FitnessFunction<BitString>
    {
        public override FitnessComparison Comparison => FitnessComparison.Maximization;

        public LeadingOnes(int size) : base(size) { }

        public override double? Optimum(int problemSize)
        {
            return problemSize;
        }

        protected override double Compute(BitString instance)
        {
            var i = 0;
            while (i < instance.Size && instance.Values[i])
            {
                i++;
            }
            return i;
        }
    }
}
