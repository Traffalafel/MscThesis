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
                Values = Sampling.SampleBitStringUniformly(size, random)
            };
        }

        public override int GetSize()
        {
            return Values.Length;
        }

        public override string ToString()
        {
            var s = Values.Select(b => Convert.ToInt32(b)).Select(i => i.ToString()).ToArray();
            return string.Join("", s);
        }
    }

}
