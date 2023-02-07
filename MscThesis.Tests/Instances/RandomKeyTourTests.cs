using MscThesis.Core.Formats;
using System;
using System.Linq;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class RandomKeyTourTests
    {
        [Fact]
        public void ReEncoding()
        {
            var numRuns = 20;

            foreach (var _ in Enumerable.Range(0, numRuns))
            {
                var random = new Random();
                var size = 100;
                var instance = RandomKeysTour.CreateUniform(random, size);

                var nodesPrev = RandomKeysTour.ToNodes(instance.Keys);
                instance.ReEncode(random);
                var nodesNew = RandomKeysTour.ToNodes(instance.Keys);

                for (int i = 0; i < size-1; i++)
                {
                    Assert.Equal(nodesNew[i], nodesPrev[i]);
                }
            }

        }

    }
}
