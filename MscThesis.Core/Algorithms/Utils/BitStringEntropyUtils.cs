using MscThesis.Core.Formats;
using System;
using System.Linq;

namespace MscThesis.Core.Algorithms.BitStrings
{
    internal static class BitStringEntropyUtils
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

        internal static double[,,] GetJointCounts(Population<BitString> population, int[] positions)
        {
            var values = population.GetValues();
            var numPositions = positions.Length;

            var counts = new double[numPositions, numPositions, 4];
            foreach (var instance in values)
            {
                AddToJointCounts(counts, instance, positions);
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

        internal static void AddToJointCounts(double[,,] counts, BitString instance, int[] positions)
        {
            var vals = instance.Values;
            for (int i = 0; i < positions.Length; i++)
            {
                for (int j = i+1; j < positions.Length; j++)
                {
                    var iPos = positions[i];
                    var jPos = positions[j];

                    if (!vals[iPos] && !vals[jPos]) counts[i, j, 0]++;
                    if (!vals[iPos] && vals[jPos]) counts[i, j, 1]++;
                    if (vals[iPos] && !vals[jPos]) counts[i, j, 2]++;
                    if (vals[iPos] && vals[jPos]) counts[i, j, 3]++;
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

    }
}
