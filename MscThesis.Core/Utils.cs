using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core
{
    public static class Utils
    {
        public static bool SampleBit(double p)
        {
            var rnd = new Random();
            var d = rnd.NextDouble();
            return d < p;
        }

        public static int Sample(double[] distribution)
        {
            var rnd = new Random();
            var d = rnd.NextDouble();

            var acc = 0.0d;
            for (int i = 0; i < distribution.Length; i++)
            {
                var p = distribution[i];
                if (d <= p + acc)
                {
                    return i;
                }
                else
                {
                    acc += p;
                }
            }
            throw new Exception("Probabilities do not sum to 1");
        }
    }
}
