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
    public class TournamentSelectionTests
    {
        [Theory]
        [InlineData("0,1", 2, 2, "0,1,0,1", "1,1")]
        [InlineData("00,01,10,11", 2, 2, "0,1,2,3", "01,11")]
        [InlineData("00,01,10,11", 2, 2, "0,3,2,2", "11,10")]
        [InlineData("00,01,10,11", 2, 2, "3,1,2,0", "11,10")]
        [InlineData("00,01,10,11", 3, 3, "0,1,2,3,0,1,2,2,0", "10,11,10")]
        public void OneMax(string bitstrings, int numTournaments, int tournamentSize, string orderStr, string expected)
        {
            // Arrange
            var randomMock = new Mock<Random>();
            var order = orderStr.Split(',').Select(s => Convert.ToInt32(s)).ToList();
            var c = 0;
            randomMock.Setup(r => r.Next(It.IsAny<int>())).Returns(() => {
                c++;
                return order[c - 1];
            });
            var random = randomMock.Object;

            var fitness = new OneMax(3);
            var population = bitstrings.Split(',').Select(s => Utils.ToBitString(s)).ToList();
            var tournament = new TournamentSelection<BitString>(numTournaments, tournamentSize);

            // Act
            var populationNew = tournament.Select(random, population, fitness);
            var actual = string.Join(',', populationNew.Select(ind => ind.ToString()));

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
