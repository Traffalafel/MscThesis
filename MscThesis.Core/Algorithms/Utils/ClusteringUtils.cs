using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    internal static class ClusteringUtils
    {

        internal static List<HashSet<int>> BuildClusters(double[] uniEntropies, double[,] jointEntropies)
        {
            var problemSize = uniEntropies.GetLength(0);

            var D = new double[problemSize, problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = i; j < problemSize; j++)
                {
                    D[i, j] = 2 - (uniEntropies[i] + uniEntropies[j]) / jointEntropies[i, j];
                    D[j, i] = D[i, j];
                }
            }

            return BuildClusters(D);
        }

        // Builds clusters minimizing distance matrix D
        // This overwrites D
        internal static List<HashSet<int>> BuildClusters(double[,] D)
        {
            var dim = D.GetLength(0);
            if (dim != D.GetLength(1))
            {
                throw new Exception("D must be square");
            }

            var Dsum = new double[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = i; j < dim; j++)
                {
                    Dsum[i, j] = D[i, j];
                    Dsum[j, i] = D[i, j];
                }
            }

            var sizes = new int[dim];
            for (int i = 0; i < dim; i++)
            {
                sizes[i] = 1;
            }

            // Re-build clusters
            var clusters = Enumerable.Range(0, dim).Select(c => new HashSet<int> { c }).ToList();
            var unmerged = Enumerable.Range(0, dim).Select(c => (c, d: c)).ToHashSet();
            var useful = new HashSet<HashSet<int>>(clusters);
            while (unmerged.Count > 1)
            {
                var pairs = GetPairs(unmerged);

                var (dist, pair) = pairs.Select(p => (Dsum[p.C1.d, p.C2.d], p)).Min();
                var C1 = pair.C1;
                var C2 = pair.C2;

                var remaining = unmerged.Select(x => x.d);
                Merge(D, Dsum, sizes, remaining, C1.d, C2.d);
                unmerged.Remove(C1);
                unmerged.Remove(C2);

                var cluster1 = clusters[C1.c];
                var cluster2 = clusters[C2.c];

                var clusterNew = cluster1.Union(cluster2).ToHashSet();
                clusters.Add(clusterNew);
                unmerged.Add((clusters.Count - 1, C1.d));
                useful.Add(clusterNew);

                if (dist == 0)
                {
                    useful.Remove(cluster1);
                    useful.Remove(cluster2);
                }
            }
            return useful.OrderBy(c => c.Count).SkipLast(1).ToList();
        }

        // Merge c2 into c1 and overwrite c2 with NaN
        private static void Merge(double[,] D, double[,] Dsum, int[] sizes, IEnumerable<int> remaining, int c1, int c2)
        {
            var sizeNew = sizes[c1] + sizes[c2];
            sizes[c1] = sizeNew;
            sizes[c2] = 0;

            foreach (var i in remaining)
            {
                if (sizes[i] == 0)
                {
                    continue;
                }

                Dsum[i, c1] = Dsum[i, c1] + Dsum[i, c2];
                Dsum[i, c2] = double.NaN;
                Dsum[c1, i] = Dsum[i, c1];
                Dsum[c2, i] = Dsum[i, c2];

                D[i, c1] = (1 / (sizeNew * sizes[i])) * Dsum[i, c1];
                D[i, c2] = double.NaN;
                D[c1, i] = D[i, c1];
                D[c2, i] = D[i, c2];
            }
        }

        private static IEnumerable<(T C1, T C2)> GetPairs<T>(IEnumerable<T> values)
        {
            return values.Select((C1, i) =>
            {
                return values.Skip(i + 1).Select(C2 => (C1, C2));
            }).SelectMany(x => x);
        }
    }
}
