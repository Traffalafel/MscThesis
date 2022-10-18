using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core.Algorithms
{
    internal static class PermutationUtils
    {
        internal static double[] ComputeUniEntropies(Population<Permutation> population)
        {
            if (population.Size == 0)
            {
                return new double[0];
            }

            var counts = GetUniCounts(population);
            var freqs = ComputeUniFrequencies(counts, population.Size);
            return ComputeUniEntropies(freqs);
        }

        internal static double[] ComputeUniEntropies(double[,] frequencies)
        {
            var problemSize = frequencies.GetLength(0);
            var entropies = new double[problemSize];
            for (int position = 0; position < problemSize; position++)
            {
                for (int i = 0; i < problemSize; i++) {
                    var p = frequencies[position,i];
                    if (p <= 0 || p >= 1)
                    {
                        continue;
                    }
                    else
                    {
                        entropies[position] += -p * Math.Log2(p) - (1 - p) * Math.Log2(1 - p);
                    }
                }
            }
            return entropies;
        }

        internal static double[,] ComputeJointEntropies(Population<Permutation> population)
        {
            if (population.Size == 0)
            {
                return new double[0,0];
            }

            var counts = GetJointCounts(population);
            return ComputeJointEntropies(counts, population.Size);
        }

        internal static double[,] ComputeJointEntropies(double[,,,] counts, int populationSize)
        {
            var problemSize = counts.GetLength(0);
            var entropies = new double[problemSize, problemSize];
            for (int pos1 = 0; pos1 < problemSize; pos1++)
            {
                for (int pos2 = 1; pos1 < problemSize; pos1++)
                {
                    for (int val1 = 0; pos1 < problemSize; pos1++)
                    {
                        for (int val2 = 0; pos1 < problemSize; pos1++)
                        {
                            var count = counts[pos1, pos2, val1, val2];
                            if (count <= 0 || count >= populationSize)
                            {
                                continue;
                            }
                            else
                            {
                                var p = count / populationSize;
                                entropies[pos1, pos2] -= p * Math.Log2(p);
                            }
                        }
                    }
                    entropies[pos2, pos1] = entropies[pos1, pos2];
                }
            }

            return entropies;
        }

        internal static double[,] ComputeUniFrequencies(double[,] counts, int populationSize)
        {
            if (populationSize == 0)
            {
                throw new Exception();
            }

            var problemSize = counts.GetLength(0);

            var freqs = new double[problemSize,problemSize];
            for (var position = 0; position < problemSize; position++)
            {
                for (var value = 0; value < problemSize; value++)
                {
                    freqs[position, value] = counts[position, value] / populationSize;
                }
            }
            return freqs;
        }

        internal static double[,] GetUniCounts(Population<Permutation> population)
        {
            var problemSize = population.ProblemSize;
            var counts = new double[problemSize,problemSize];
            foreach (var individual in population)
            {
                AddToUniCounts(counts, individual.Value);
            }
            return counts;
        }

        internal static double[,,,] GetJointCounts(Population<Permutation> population)
        {
            var problemSize = population.ProblemSize;
            var counts = new double[problemSize, problemSize, problemSize, problemSize];
            foreach (var individual in population)
            {
                AddToJointCounts(counts, individual.Value);
            }
            return counts;
        }

        internal static void AddToUniCounts(double[,] counts, Permutation instance)
        {
            var position = 0;
            foreach (var value in instance.Values)
            {
                counts[position++, value]++;
            }
        }

        internal static void AddToJointCounts(double[,,,] counts, Permutation instance)
        {
            var problemSize = counts.GetLength(0);

            var values = instance.Values;
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = i + 1; j < problemSize; j++)
                {
                    counts[i, j, values[i], values[j]]++;
                }
            }
        }

    }
}
