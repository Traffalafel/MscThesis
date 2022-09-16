using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MscThesis.Core
{
    public class OneMax : FitnessFunction<BitString>
    {
        public override double ComputeFitness(BitString instance)
        {
            return instance.Values.Where(val => val == true).Count();
        }
    }
}
