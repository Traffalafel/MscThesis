using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    internal static class ClusteringUtils
    {

        internal static List<HashSet<int>> BuildClusters(int problemSize, Func<int, int, double> distanceFunc)
        {
            var clusters = Enumerable.Range(0, problemSize).Select(c => new HashSet<int> { c }).ToList();
            var Dpositions = Enumerable.Range(0, problemSize).ToList();

            var unmerged = Enumerable.Range(0, problemSize).ToHashSet();
            var useful = new List<HashSet<int>>(clusters);

            var D = new double[problemSize, problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = i; j < problemSize; j++)
                {
                    D[i, j] = distanceFunc(i, j);
                    D[j, i] = D[i, j];
                }
            }

            while (unmerged.Count > 1)
            {
                var pairs = GetPairs(unmerged);
                var (dist, pair) = pairs.Select(p => (D[Dpositions[p.C1], Dpositions[p.C2]], p)).Min();
                var C1 = pair.C1;
                var C2 = pair.C2;

                var cluster1 = clusters[C1];
                var cluster2 = clusters[C2];
                var clusterNew = cluster1.Union(cluster2).ToHashSet();

                unmerged.Remove(C1);
                unmerged.Remove(C2);

                var mergedPos = Dpositions[C1];
                var nanPos = Dpositions[C2];

                foreach (var remainingIdx in unmerged)
                {
                    var remainingCluster = clusters[remainingIdx];
                    var distanceNew = UPGMA(cluster1, cluster2, distanceFunc);
                    var remainingPos = Dpositions[remainingIdx];

                    D[mergedPos, remainingPos] = distanceNew;
                    D[remainingPos, mergedPos] = distanceNew;
                    D[nanPos, remainingPos] = double.NaN;
                    D[remainingPos, nanPos] = double.NaN;
                }

                if (dist == 0)
                {
                    useful.Remove(cluster1);
                    useful.Remove(cluster2);
                }

                var idxNew = clusters.Count;
                Dpositions.Add(mergedPos);
                clusters.Add(clusterNew);
                useful.Add(clusterNew);
                unmerged.Add(idxNew);
            }

            return useful.OrderBy(c => c.Count).SkipLast(1).ToList();
        }

        internal static Func<int, int, double> GetEntropyDistanceFunc(double[,] jointEntropies, double[] uniEntropies)
        {
            return (c1, c2) => 2 - (uniEntropies[c1] + uniEntropies[c2]) / jointEntropies[c1, c2];
        }

        internal static Func<int, int, double> GetPermutationDistanceFunc(double[,] delta1, double[,] delta2)
        {
            return (c1, c2) => 1 - delta1[c1, c2] * delta2[c1, c2];
        }

        private static IEnumerable<(T C1, T C2)> GetPairs<T>(IEnumerable<T> values)
        {
            return values.Select((C1, i) =>
            {
                return values.Skip(i + 1).Select(C2 => (C1, C2));
            }).SelectMany(x => x);
        }

        private static double UPGMA(HashSet<int> cluster1, HashSet<int> cluster2, Func<int, int, double> distanceFunc)
        {
            var sum = 0.0d;
            foreach (var c1 in cluster1)
            {
                foreach (var c2 in cluster2)
                {
                    sum += distanceFunc(c1, c2);
                }
            }
            return (1.0d / (cluster1.Count * cluster2.Count)) * sum;
        }

    }
}
