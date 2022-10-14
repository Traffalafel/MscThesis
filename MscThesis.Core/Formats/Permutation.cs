using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MscThesis.Core.Formats
{
    public class Permutation : InstanceFormat, IEnumerable<int>
    {
        private IEnumerable<int> _tour;

        public Permutation(IEnumerable<int> tour)
        {
            _tour = tour;
        }

        public override int Size => _tour.Count();

        public override string ToString()
        {
            return string.Join(',', _tour);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _tour.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
