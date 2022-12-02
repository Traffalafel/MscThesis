using System;
using System.Collections;
using System.Linq;

namespace MscThesis.Core.Formats
{
    public class BitString : InstanceFormat
    {
        public bool[] Values { get; set; }

        public static BitString CreateUniform(Random random, int size)
        {
            return new BitString
            {
                Values = RandomUtils.SampleBitStringUniformly(random, size)
            };
        }

        public override int Size => Values.Length;

        public void Flip(int i)
        {
            Values[i] = !Values[i];
        }

        public override string ToString()
        {
            if (Values == null)
            {
                return string.Empty;
            }

            var s = Values.Select(b => Convert.ToInt32(b)).Select(i => i.ToString()).ToArray();
            return string.Join("", s);
        }

        public static InstanceFormat FromString(string str)
        {
            var values = str.Select(c => c == '1').ToArray();
            return new BitString
            {
                Values = values
            };
        }
    }

}
