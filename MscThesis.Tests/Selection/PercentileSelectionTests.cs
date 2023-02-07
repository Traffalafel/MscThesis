using Moq;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Tests.FitnessFunction;
using System;
using System.Linq;
using Xunit;

namespace MscThesis.Tests.Selection
{
    public class PercentileSelectionTests
    {
        [Theory]
        [InlineData("0,1", 0.5, "1")]
        [InlineData("00,01,10,11", 0.75, "11,10,01")]
        [InlineData("00,01,10,11", 0.5, "11,10")]
        [InlineData("00,01,10,11", 0.25, "11")]
        [InlineData("00,01,10,11", 1.0, "11,10,01,00")]
        public void OneMax(string bitstrings, double percentile, string expected)
        {
            // Arrange
            var random = new Random();
            var population = bitstrings.Split(',').Select(s => Utils.ToBitString(s)).ToList();
            var problemSize = population[0].Size;
            var fitness = new OneMax(problemSize);
            var selection = new PercentileSelection<BitString>(percentile);

            // Act
            var populationNew = selection.Select(random, population, fitness);
            var actual = string.Join(',', populationNew.Select(ind => ind.ToString()));

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
