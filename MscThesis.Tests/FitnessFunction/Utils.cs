using MscThesis.Core.Formats;

namespace MscThesis.Tests.FitnessFunction
{
    internal static class Utils
    {
        internal static BitString ToBitString(string bits)
        {
            var output = new bool[bits.Length];
            for (int i = 0; i < bits.Length; i++)
            {
                output[i] = bits[i] == '1';
            }
            return new BitString
            {
                Values = output
            };
        }
    }
}
