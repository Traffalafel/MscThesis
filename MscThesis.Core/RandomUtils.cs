using MscThesis.Core.Formats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MscThesis.Core
{
    public static class RandomUtils
    {
        public static ThreadLocal<Random> BuildRandom()
        {
            return new ThreadLocal<Random>(() => new Random());
        }

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

        public static BitString SampleBitString(Random random, double[] probs)
        {
            var size = probs.Length;

            var s = new bool[size];
            for (int i = 0; i < size; i++)
            {
                s[i] = RandomUtils.SampleBernoulli(random, probs[i]);
            }
            return new BitString
            {
                Values = s
            };
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

        public static int[] SampleUnique(Random random, int numVals, int maxValue)
        {
            if (maxValue < numVals)
            {
                throw new Exception($"Cannot sample {numVals} unique values from [0;{maxValue-1}]");
            }

            var options = Enumerable.Range(0, maxValue).ToArray();
            var output = new int[numVals];

            var c = maxValue;
            for (int i = 0; i < numVals; i++)
            {
                var pos = random.Next(c);
                output[i] = options[pos];
                (options[c - 1], options[pos]) = (options[pos], options[c - 1]);
                c--;
            }

            return output;
        }

        // Code from SO answer @ https://stackoverflow.com/questions/218060/random-gaussian-variables
        public static double SampleStandard(Random random, double mean, double stdDeviation)
        {
            double u1 = 1.0 - random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            return mean + stdDeviation * randStdNormal; //random normal(mean,stdDev^2)
        }

    }
}
