using MscThesis.Core.Formats;

namespace MscThesis.Core
{
    public class MinimizationComparison : FitnessComparisonStrategy
    {
        public override bool IsFitter(double val1, double val2)
        {
            return val1 < val2;
        }
    }
}
