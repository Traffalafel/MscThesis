using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core
{

    public class FitnessComparison : IComparer<double?>
    {
        private readonly Func<double, double, bool> _strategy;

        public static FitnessComparison Minimization => new FitnessComparison((v1, v2) => v1 < v2);
        public static FitnessComparison Maximization => new FitnessComparison((v1, v2) => v1 > v2);

        private FitnessComparison(Func<double, double, bool> strategy)
        {
            _strategy = strategy;
        }

        public int Compare(double? x, double? y)
        {
            return IsFitter(x, y) ? 1 : -1;
        }

        public double? GetFittest(IEnumerable<double?> vals)
        {
            double? fittest = null;
            foreach (var val in vals)
            {
                if (IsFitter(val, fittest))
                {
                    fittest = val;
                }
            }
            return fittest;
        }

        // true => i1 is fitter than i2
        public bool IsFitter(Instance i1, Instance i2)
        {
            if (i1 == null && i2 == null)
            {
                return false;
            }
            if (i1 != null && i2 == null)
            {
                return true;
            }
            if (i1 == null && i2 != null)
            {
                return false;
            }

            return IsFitter(i1.Fitness, i2.Fitness);
        }

        public bool IsFitter(double? val1, double? val2)
        {
            if (val2 == null)
            {
                return true;
            }
            if (val1 == null)
            {
                return false;
            }
            return _strategy(val1.Value, val2.Value);
        }

        // true => val1 is fitter than val2
        public bool IsFitter(double val1, double val2)
        {
            return _strategy(val1, val2);
        }
    }
}
