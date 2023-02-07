using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    internal static class TourEntropyUtils
    {
        internal static double[] ComputeUniEntropies(IEnumerable<Tour> population, int problemSize)
        {
            var populationSize = population.Count();
            if (populationSize == 0)
            {
                return new double[0];
            }

            var counts = GetUniCounts(population, problemSize);
            var freqs = ComputeUniFrequencies(counts, populationSize);
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

        internal static double[,] ComputeJointEntropies(IEnumerable<Tour> population, int problemSize)
        {
            var populationSize = population.Count();
            if (populationSize == 0)
            {
                return new double[0,0];
            }

            var counts = GetJointCounts(population, problemSize);
            var freqs = ComputeJointFrequencies(counts, populationSize);
            return ComputeJointEntropies(freqs);
        }

        internal static double[,,,] ComputeJointFrequencies(double[,,,] counts, int populationSize)
        {
            var numPositions = counts.GetLength(0);
            var numOptions = counts.GetLength(3);

            var freqs = new double[numPositions, numPositions, numOptions, numOptions];
            for (var i = 0; i < numPositions; i++)
                for (var j = i + 1; j < numPositions; j++)
                    for (var k = 0; k < numOptions; k++)
                        for (var l = 0; l < numOptions; l++)
                        {
                            freqs[i, j, k, l] = counts[i, j, k, l] / populationSize;
                        }

            return freqs;
        }

        internal static double[,] ComputeJointEntropies(double[,,,] freqs)
        {
            var problemSize = freqs.GetLength(0);
            var entropies = new double[problemSize, problemSize];
            for (int pos1 = 0; pos1 < problemSize; pos1++)
            {
                for (int pos2 = pos1 + 1; pos2 < problemSize; pos2++)
                {
                    for (int val1 = 0; val1 < problemSize; val1++)
                    {
                        for (int val2 = 0; val2 < problemSize; val2++)
                        {
                            var p = freqs[pos1, pos2, val1, val2];
                            if (p <= 0 || p >= 1.0)
                            {
                                continue;
                            }
                            else
                            {
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

        internal static double[,] GetUniCounts(IEnumerable<Tour> population, int problemSize)
        {
            var counts = new double[problemSize,problemSize];
            foreach (var individual in population)
            {
                AddToUniCounts(counts, individual);
            }
            return counts;
        }

        internal static double[,,,] GetJointCounts(IEnumerable<Tour> population, int problemSize)
        {
            var counts = new double[problemSize, problemSize, problemSize, problemSize];
            foreach (var individual in population)
            {
                AddToJointCounts(counts, individual);
            }
            return counts;
        }

        internal static double[,,,] GetJointCounts(IEnumerable<Tour> population, int[] positions, int problemSize)
        {
            var numPositions = positions.Length;
            var counts = new double[numPositions, numPositions, problemSize, problemSize];
            foreach (var individual in population)
            {
                AddToJointCounts(counts, individual, positions);
            }
            return counts;
        }

        internal static void AddToUniCounts(double[,] counts, Tour instance)
        {
            var position = 0;
            foreach (var value in instance.Values)
            {
                counts[position++, value]++;
            }
        }

        internal static void AddToJointCounts(double[,,,] counts, Tour instance)
        {
            var problemSize = counts.GetLength(0);

            var values = instance.Values;
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = i + 1; j < problemSize; j++)
                {
                    counts[i, j, values[i], values[j]]++;
                    counts[j, i, values[j], values[i]]++;
                }
            }
        }

        internal static void AddToJointCounts(double[,,,] counts, Tour instance, int[] positions)
        {
            var values = instance.Values;

            for (int i = 0; i < positions.Length; i++)
            {
                for (int j = i + 1; j < positions.Length; j++)
                {
                    var iPos = positions[i];
                    var jPos = positions[j];

                    counts[i, j, values[iPos], values[jPos]]++;
                    counts[j, i, values[jPos], values[iPos]]++;
                }
            }
        }

    }
}
