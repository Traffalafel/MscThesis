using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TspLibNet.Tours;

namespace MscThesis.Core.Formats
{
    public class Tour : InstanceFormat, IEnumerable<int>, ITour
    {
        private int[] _values;

        public Tour(int[] values)
        {
            _values = values;
        }

        public int[] Values => _values;

        public static Tour CreateUniform(Random random, int size)
        {
            var indices = Enumerable.Range(0, size - 1).ToArray();
            RandomUtils.Shuffle(random, indices);
            return new Tour(indices);
        }

        public override int Size => _values.Count();

        public string Name => "name";

        public string Comment => "comment";

        public int Dimension => Size + 1;

        public List<int> Nodes => _values.Select(val => val+2).Prepend(1).ToList(); 

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
