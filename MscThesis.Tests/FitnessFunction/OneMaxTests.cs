using MscThesis.Core.FitnessFunctions;
using Xunit;

namespace MscThesis.Tests.FitnessFunction
{
    public class OneMaxTests
    {
        [Theory]
        [InlineData("1111", 4)]
        [InlineData("1100", 2)]
        [InlineData("1101", 3)]
        [InlineData("111100", 4)]
        [InlineData("111111", 6)]
        [InlineData("111101", 5)]
        [InlineData("111000", 3)]
        public void IsCorrect(string bits, int expectedFitness)
        {
            var size = bits.Length;
            var func = new OneMax(size);

            var bs = Utils.ToBitString(bits);

            var actualFitness = func.ComputeFitness(bs);

            Assert.Equal(expectedFitness, actualFitness);
        }

    }
}
