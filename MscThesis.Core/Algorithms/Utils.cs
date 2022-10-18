using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    internal static class Utils
    {
        internal static double[] ComputeUniEntropies(Population<BitString> population)
        {
            if (population.Size == 0)
            {
                return new double[0];
            }

            var counts = GetUniCounts(population);
            var freqs = ComputeUniFrequencies(counts, population.Size);
            return ComputeUniEntropies(freqs);
        }

        internal static double[] ComputeUniEntropies(double[] uniFrequencies)
        {
            var problemSize = uniFrequencies.GetLength(0);
            var entropies = new double[problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                var p = uniFrequencies[i];
                if (p <= 0 || p >= 1)
                {
                    continue;
                }
                else
                {
                    entropies[i] = -p * Math.Log2(p) - (1 - p) * Math.Log2(1 - p);
                }
            }
            return entropies;
        }

        internal static double[,] ComputeJointEntropies(Population<BitString> population)
        {
            if (population.Size == 0)
            {
                return new double[0, 0];
            }

            var counts = GetJointCounts(population);
            var freqs = ComputeJointFrequencies(counts, population.Size);
            return ComputeJointEntropies(freqs);
        }

        internal static double[,] ComputeJointEntropies(double[,][] jointFrequencies)
        {
            var problemSize = jointFrequencies.GetLength(0);

            var jointEntropies = new double[problemSize, problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = i + 1; j < problemSize; j++)
                {
                    var jp = jointFrequencies[i, j];
                    jointEntropies[i, j] = -jp.Sum(p =>
                    {
                        if (p <= 0 || p >= 1)
                        {
                            return 0.0;
                        }
                        else
                        {
                            return p * Math.Log2(p);
                        }
                    });

                    jointEntropies[j, i] = jointEntropies[i, j];
                }
            }

            return jointEntropies;
        }

        internal static double[] GetUniCounts(Population<BitString> population)
        {
            var values = population.GetValues();
            var problemSize = population.ProblemSize;

            var counts = new double[problemSize];
            foreach (var instance in values)
            {
                AddToUniCounts(counts, instance);
            }
            return counts;
        }

        internal static void AddToUniCounts(double[] counts, BitString instance)
        {
            var problemSize = counts.GetLength(0);

            var vals = instance.Values;
            for (int i = 0; i < problemSize; i++)
            {
                if (vals[i]) counts[i]++;
            }
        }

        internal static double[,,] GetJointCounts(Population<BitString> population)
        {
            var values = population.GetValues();
            var problemSize = population.ProblemSize;

            var counts = new double[problemSize, problemSize, 4];
            foreach (var instance in values)
            {
                AddToJointCounts(counts, instance);
            }
            return counts;
        }

        internal static void AddToJointCounts(double[,,] counts, BitString instance)
        {
            var problemSize = counts.GetLength(0);

            var vals = instance.Values;
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = i + 1; j < problemSize; j++)
                {
                    if (!vals[i] && !vals[j]) counts[i, j, 0]++;
                    if (!vals[i] && vals[j]) counts[i, j, 1]++;
                    if (vals[i] && !vals[j]) counts[i, j, 2]++;
                    if (vals[i] && vals[j]) counts[i, j, 3]++;
                }
            }
        }


        internal static double[] ComputeUniFrequencies(double[] counts, int populationSize)
        {
            var problemSize = counts.GetLength(0);

            var probabilities = new double[problemSize];
            for (var i = 0; i < problemSize; i++)
            {
                var p = counts[i] / populationSize;
                probabilities[i] = p;
            }

            return probabilities;
        }

        internal static double[,][] ComputeJointFrequencies(double[,,] counts, int populationSize)
        {
            var problemSize = counts.GetLength(0);

            var freqs = new double[problemSize, problemSize][];
            for (var i = 0; i < problemSize; i++)
            {
                for (var j = i + 1; j < problemSize; j++)
                {
                    freqs[i, j] = new double[4];
                    for (var k = 0; k < 4; k++)
                    {
                        var p = counts[i, j, k] / populationSize;
                        freqs[i, j][k] = p;
                    }
                }
            }

            return freqs;
        }

        internal static List<HashSet<int>> BuildClusters(double[] uniEntropies, double[,] jointEntropies)
        {
            var problemSize = uniEntropies.GetLength(0);

            var Dsum = new double[problemSize, problemSize];
            var D = new double[problemSize, problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = i; j < problemSize; j++)
                {
                    Dsum[i, j] = 2 - (uniEntropies[i] + uniEntropies[j]) / jointEntropies[i, j];
                    Dsum[j, i] = Dsum[i, j];

                    D[i, j] = Dsum[j, i];
                    D[j, i] = Dsum[i, j];
                }
            }
            var sizes = new int[problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                sizes[i] = 1;
            }

            // Re-build clusters
            var clusters = Enumerable.Range(0, problemSize).Select(c => new HashSet<int> { c }).ToList();
            var unmerged = Enumerable.Range(0, problemSize).Select(c => (c, d: c)).ToHashSet();
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

        private static void Merge(double[,] D, double[,] Dsum, int[] sizes, IEnumerable<int> remaining, int c1, int c2)
        {
            // Merge c2 into c1 and overwrite c2 with NaN
            var problemSize = D.GetLength(0);

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
