using System;
using System.Collections;
using System.Linq;

namespace MscThesis.Core.Formats
{
    public class BitString : InstanceFormat
    {
        public bool[] Values { get; set; }

        public static BitString CreateUniform(int size, Random random)
        {
            return new BitString
            {
                Values = RandomUtils.SampleBitStringUniformly(size, random)
            };
        }

        public override int GetSize()
        {
            return Values.Length;
        }

        public void Flip(int i)
        {
            Values[i] = !Values[i];
        }

        public override string ToString()
        {
            var s = Values.Select(b => Convert.ToInt32(b)).Select(i => i.ToString()).ToArray();
            return string.Join("", s);
        }
    }

}
