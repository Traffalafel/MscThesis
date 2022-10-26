using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MscThesis.Core.Algorithms
{
    internal static class MIMICUtils
    {
        internal static int[] GetOrdering(int problemSize, double[] uniEntropies, double[,] jointEntropies)
        {
            var remaining = new HashSet<int>(Enumerable.Range(0, problemSize));
            var ordering = new int[problemSize];

            // Find lowest univariate entropy and set to start of chain
            var (_, minIdx) = uniEntropies.Select((e, i) => (e, i)).Min();
            ordering[0] = minIdx;
            remaining.Remove(minIdx);

            // Find lowest pairwise entropies and build chain
            for (int pos = 1; pos < problemSize; pos++)
            {
                var posPrev = ordering[pos - 1];
                var (_, iMin) = remaining.Select(i =>
                {
                    if (i < posPrev)
                    {
                        return (jointEntropies[i, posPrev], i);
                    }
                    else
                    {
                        return (jointEntropies[posPrev, i], i);
                    }
                }).Min();

                ordering[pos] = iMin;
                remaining.Remove(iMin);
            }

            return ordering;
        }
    }
}
