using System;
using System.Collections;
using System.Collections.Generic;

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

        public static bool SampleBit(Random random, double p)
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
                if (distribution[i] == double.NaN)
                {
                    continue;
                }

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

        public static T Choose<T>(Random random, List<T> items)
        {
            var i = random.Next(items.Count);
            return items[i];
        }

        public static IEnumerable<T> Shuffle<T>(Random random, T[] items)
        {
            var remaining = items.Length;
            while (remaining > 0)
            {
                var i = random.Next(remaining);
                yield return items[i];
                remaining--;
            }
        }

    }
}
