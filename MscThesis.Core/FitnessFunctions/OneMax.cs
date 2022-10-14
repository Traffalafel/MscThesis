using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MscThesis.Core.FitnessFunctions
{
    public class OneMax : FitnessFunction<BitString>
    {
        public override double? Optimum => throw new NotImplementedException();

        protected override double Compute(BitString instance)
        {
            return instance.Values.Where(val => val == true).Count();
        }
    }
}
