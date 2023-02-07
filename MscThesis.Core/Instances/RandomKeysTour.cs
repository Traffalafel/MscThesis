using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MscThesis.Core.Formats
{
    public class RandomKeysTour : Tour
    {
        private double[] _keys;

        public double[] Keys => _keys;

        public RandomKeysTour(double[] keys) : base(ToNodes(keys))
        {
            _keys = keys;
        }

        public static new RandomKeysTour CreateUniform(Random random, int size)
        {
            var keys = GenerateRandomKeys(random, size-1);
            return new RandomKeysTour(keys);
        }

        // Create permutation from keys sort order
        public static int[] ToNodes(double[] keys)
        {
            var order = keys.Select((val, idx) => (val, idx))
                            .OrderBy(x => x.val)
                            .Select((x, sortPos) => (x.idx, sortPos));

            var nodes = new int[keys.Length];
            foreach (var (idx,sortPos) in order)
            {
                nodes[idx] = sortPos;
            }
            return nodes;
        }

        // Perform random rescaling
        public void Rescale(Random random, HashSet<int> cluster, (double lo, double hi)[] intervals)
        {
            var intervalIdx = random.Next(intervals.Length);
            var interval = intervals[intervalIdx];

            double min = 0.0;
            double max = 1.0;
            foreach (var position in cluster)
            {
                var value = _keys[position];
                if (value < min)
                {
                    min = value;
                }
                if (value > max)
                {
                    max = value;
                }
            }

            if (min == max)
            {
                foreach (var position in cluster)
                {
                    _keys[position] = interval.lo;
                }
            }
            else
            {
                var range = max - min;
                var numIntervalsScale = 1.0d / intervals.Length;
                foreach (var position in cluster)
                {
                    _keys[position] = (_keys[position] - min) / range * numIntervalsScale + interval.lo;
                }
            }

            _values = ToNodes(_keys);
        }

        public void ReEncode(Random random)
        {
            var newKeysSorted = GenerateRandomKeys(random, _keys.Length).OrderBy(x => x).ToArray();
            for (int i = 0; i < _keys.Length; i++)
            {
                _keys[i] = newKeysSorted[_values[i]];
            }
        }

        private static double[] GenerateRandomKeys(Random random, int size)
        {
            return Enumerable.Range(0, size).Select(_ => random.NextDouble()).ToArray();
        }

    }
}
