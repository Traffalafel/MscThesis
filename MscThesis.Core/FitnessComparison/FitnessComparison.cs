using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core
{
    public static class FitnessComparison
    {
        public static FitnessComparisonStrategy Maximization = new MaximizationComparison();
        public static FitnessComparisonStrategy Minimization = new MinimizationComparison();
    }
}
