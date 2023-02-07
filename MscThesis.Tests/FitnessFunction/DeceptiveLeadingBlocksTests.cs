using MscThesis.Core.FitnessFunctions;
using Xunit;

namespace MscThesis.Tests.FitnessFunction
{
    public class DeceptiveLeadingBlocksTests
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

            var bs = Utils.ToBitString(bits);

            var actualFitness = func.ComputeFitness(bs);

            Assert.Equal(expectedFitness, actualFitness);
        }

    }
}
