using System;
using System.Collections;
using System.Linq;

namespace MscThesis.Core.Formats
{
    public class BitString : InstanceFormat
    {
        public bool[] Values { get; set; }

        public static BitString GenerateUniformly(int size)
        {
            var rnd = new Random();

            var numBytes = (size / 8) + 1;
            var bytes = new byte[numBytes];
            rnd.NextBytes(bytes);
            var bits = new BitArray(bytes);

            var values = new bool[bits.Count];
            bits.CopyTo(values, 0);

            return new BitString
            {
                Values = values[0..size]
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
