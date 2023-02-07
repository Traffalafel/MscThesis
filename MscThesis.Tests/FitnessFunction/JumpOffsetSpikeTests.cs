using MscThesis.Core.FitnessFunctions;
using Xunit;

namespace MscThesis.Tests.FitnessFunction
{
    public class JumpOffsetSpikeTests
    {
        [Theory]
        [InlineData("00000000", 2, 2)]
        [InlineData("11111111", 2, 10)]
        [InlineData("11111110", 2, 11)]
        public void IsCorrect(string bits, int gapSize, int expectedFitness)
        {
            var size = bits.Length;
            var func = new JumpOffsetSpike(size, gapSize);

            var bs = Utils.ToBitString(bits);

            var actualFitness = func.ComputeFitness(bs);

            Assert.Equal(expectedFitness, actualFitness);
        }

    }
}
