using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MscThesis.Core.Formats
{
    public class Permutation : InstanceFormat, IEnumerable<int>
    {
        private int[] _values;

        public Permutation(int[] values)
        {
            _values = values;
        }

        public int[] Values => _values;

        public static Permutation CreateUniform(Random random, int size)
        {
            var indices = Enumerable.Range(0, size).ToArray();
            var shuffled = RandomUtils.Shuffle(random, indices).ToArray();
            return new Permutation(shuffled);
        }

        public override int Size => _values.Count();

        public override string ToString()
        {
            return string.Join(',', _values);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _values.Cast<int>().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
