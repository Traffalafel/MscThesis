using MscThesis.Core.Formats;
using System.Linq;

namespace MscThesis.Core.FitnessFunctions
{
    public class JumpOffsetSpike : FitnessFunction<BitString>
    {
        private int _m; // = width of "gap"

        public JumpOffsetSpike(int size, int m) : base(size)
        {
            _m = m;
        }

        public override FitnessComparison Comparison => FitnessComparison.Maximization;

        public override double? Optimum(int problemSize)
        {
            return problemSize + _m + 1;
        }

        protected override double Compute(BitString instance)
        {
            var numOnes = instance.Values.Where(val => val == true).Count();
            var n = instance.Size;

            if (numOnes == (3*n)/4 + _m/2)
            {
                return n + _m + 1;
            }
            else if (numOnes <= (3*n)/4 || numOnes >= (3*n)/4 + _m)
            {
                return _m + numOnes;
            }
            else
            {
                return (3 * n) / 4 + _m - numOnes;
            }
        }
    }
}
