using System.Linq;

namespace MscThesis.Core.FitnessFunctions
{
    public class JumpOffsetSpike : FitnessFunction<BitString>
    {
        private int _m; // = width of "gap"

        public JumpOffsetSpike(int m)
        {
            _m = m;
        }

        public override double ComputeFitness(BitString instance)
        {
            var numOnes = instance.Values.Where(val => val == true).Count();
            var n = instance.GetSize();

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
