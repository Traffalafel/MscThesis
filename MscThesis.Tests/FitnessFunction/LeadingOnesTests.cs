using MscThesis.Core.FitnessFunctions;
using Xunit;

namespace MscThesis.Tests.FitnessFunction
{
    public class LeadingOnesTests
    {
        [Theory]
        [InlineData("1111", 4)]
        [InlineData("1100", 2)]
        [InlineData("1101", 2)]
        [InlineData("111100", 4)]
        [InlineData("111111", 6)]
        [InlineData("111101", 4)]
        [InlineData("111000", 3)]
        public void IsCorrect(string bits, int expectedFitness)
        {
            var size = bits.Length;
            var func = new LeadingOnes(size);

            var bs = Utils.ToBitString(bits);

            var actualFitness = func.ComputeFitness(bs);

            Assert.Equal(expectedFitness, actualFitness);
        }

    }
}
