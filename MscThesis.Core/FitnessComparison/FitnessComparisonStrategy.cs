using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core
{

    public abstract class FitnessComparisonStrategy
    {
        // true => val1 is fitter than val2
        public abstract bool IsFitter(double val1, double val2);

        // true => i1 is fitter than i2
        public bool IsFitter(Individual<InstanceFormat> i1, Individual<InstanceFormat> i2)
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

            if (i1.Fitness == null && i2.Fitness == null)
            {
                return false;
            }
            if (i1.Fitness != null && i2.Fitness == null)
            {
                return true;
            }
            if (i1.Fitness == null && i2.Fitness != null)
            {
                return false;
            }

            return IsFitter(i1.Fitness.Value, i2.Fitness.Value);
        }
    }
}
