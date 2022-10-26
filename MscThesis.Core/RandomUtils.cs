using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core
{
    public static class RandomUtils
    {
        public static bool[] SampleBitStringUniformly(Random random, int size)
        {
            var numBytes = (size / 8) + 1;
            var bytes = new byte[numBytes];
            random.NextBytes(bytes);
            var bits = new BitArray(bytes);

            var values = new bool[bits.Count];
            bits.CopyTo(values, 0);
            return values[0..size];
        }

        public static bool SampleBernoulli(Random random, double p)
        {
            var d = random.NextDouble();
            return d < p;
        }

        public static int SampleDistribution(Random random, double[] distribution)
        {
            var d = random.NextDouble();

            var acc = 0.0d;
            for (int i = 0; i < distribution.Length; i++)
            {
                var p = distribution[i];

                if (double.IsNaN(p))
                {
                    continue;
                }

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

        public static T Choose<T>(Random random, List<T> items)
        {
            var i = random.Next(items.Count);
            return items[i];
        }

        // Fisher-Yates shuffle, inspired by SO answer at 
        // https://stackoverflow.com/questions/273313/randomize-a-listt
        public static void Shuffle<T>(Random random, T[] items)
        {
            var c = items.Length;
            while (c > 1)
            {
                c--;
                var i = random.Next(c + 1);
                var tmp = items[i];
                items[i] = items[c];
                items[c] = tmp;
            }
        }

    }
}
