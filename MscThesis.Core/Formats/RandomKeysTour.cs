using System;
using System.Collections.Generic;
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
            var keys = Enumerable.Range(0, size - 1).Select(_ => random.NextDouble()).ToArray();
            return new RandomKeysTour(keys);
        }

        // Create permutation from keys sort order
        private static int[] ToNodes(double[] keys)
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
    }
}
