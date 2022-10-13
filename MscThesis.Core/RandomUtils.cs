using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MscThesis.Core
{
    public static class RandomUtils
    {
        public static bool[] SampleBitStringUniformly(int size, Random random)
        {
            var numBytes = (size / 8) + 1;
            var bytes = new byte[numBytes];
            random.NextBytes(bytes);
            var bits = new BitArray(bytes);

            var values = new bool[bits.Count];
            bits.CopyTo(values, 0);
            return values[0..size];
        }

        public static bool SampleBit(double p, Random random)
        {
            var d = random.NextDouble();
            return d < p;
        }

        public static int SampleDistribution(double[] distribution, Random random)
        {
            var d = random.NextDouble();

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

        public static T Choose<T>(List<T> items, Random random)
        {
            var i = random.Next(items.Count);
            return items[i];
        }

        public static IEnumerable<T> Shuffle<T>(List<T> items, Random random)
        {
            var remaining = items.Count;
            while (remaining > 0)
            {
                var i = random.Next(remaining);
                yield return items[i];
                remaining--;
            }
        }

    }
}
