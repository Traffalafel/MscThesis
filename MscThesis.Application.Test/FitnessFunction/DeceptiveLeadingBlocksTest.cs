using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using Xunit;

namespace MscThesis.Application.Test.FitnessFunction
{
    public class DeceptiveLeadingBlocksTest
    {
        [Theory]
        [InlineData("1111", 2, 4)]
        [InlineData("1100", 2, 3)]
        [InlineData("1101", 2, 2)]
        [InlineData("111100", 2, 5)]
        [InlineData("111111", 3, 6)]
        [InlineData("111101", 3, 3)]
        [InlineData("111000", 3, 4)]
        [InlineData("11111111111100001111", 4, 13)]
        public void IsCorrect(string bits, int blockSize, int expectedFitness)
        {
            var size = bits.Length;
            var func = new DeceptiveLeadingBlocks(size, blockSize);

            var bs = ToBitString(bits);

            var actualFitness = func.ComputeFitness(bs);

            Assert.Equal(expectedFitness, actualFitness);
        }

        private BitString ToBitString(string bits)
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
