using MscThesis.Core.Formats;

namespace MscThesis.Core
{
    public class MinimizationComparison : FitnessComparisonStrategy
    {
        protected override bool IsFitter(double val1, double val2)
        {
            return val1 < val2;
        }
    }
}
