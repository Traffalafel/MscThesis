﻿using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MscThesis.Core.Algorithms.Tours
{
    internal static class RandomKeysUtils
    {
        private static double _rescalingProb = 0.1d;

        // Copy values over from donor
        internal static bool OptimalMixing(Random random, RandomKeysTour donor, RandomKeysTour dest, HashSet<int> cluster, FitnessFunction<Tour> fitnessFunction, (double, double)[] rescalingIntervals)
        {
            var donorInstance = donor;
            var destInstance = dest;

            var prevKeys = Copy(donorInstance.Keys, destInstance.Keys, cluster);

            // Rescaling
            if (random.NextDouble() <= _rescalingProb)
            {
                destInstance.Rescale(random, cluster, rescalingIntervals);
            }

            var fitnessPrev = dest.Fitness.Value;
            var fitnessNew = fitnessFunction.ComputeFitness(destInstance);

            if (fitnessFunction.Comparison.IsFitter(fitnessNew, fitnessPrev))
            {
                dest.Fitness = fitnessNew;
                return true;
            }
            else
            {
                Revert(destInstance.Keys, prevKeys);
                return false;
            }
        }

        internal static (double, double)[] ComputeRescalingIntervals(int size)
        {
            var intervalRange = 1.0d / size;
            return Enumerable.Range(0, size)
                             .Select(i => i * intervalRange)
                             .Select(start => (start, start + intervalRange))
                             .ToArray();
        }

        internal static void Revert(double[] keys, List<(int idx, double prevKey)> prev)
        {
            foreach (var (idx, prevKey) in prev)
            {
                keys[idx] = prevKey;
            }
        }

        internal static List<(int idx, double prevKey)> Copy(double[] donor, double[] dest, HashSet<int> indices)
        {
            var prev = new List<(int, double)>();
            foreach (var index in indices)
            {
                prev.Add((index, dest[index]));
                dest[index] = donor[index];
            }
            return prev;
        }

        internal static double[,] ComputeDelta1(int numFreeNodes, IEnumerable<RandomKeysTour> population)
        {
            var delta1Sums = new double[numFreeNodes, numFreeNodes];
            foreach (var individual in population)
            {
                AddToDelta1Sums(delta1Sums, individual);
            }

            return ComputeDelta1(delta1Sums, population.Count());
        }

        internal static double[,] ComputeDelta1(double[,] delta1Sums, int populationSize)
        {
            var numFreeNodes = delta1Sums.GetLength(0);

            var delta1 = new double[numFreeNodes, numFreeNodes];
            for (int i = 0; i < numFreeNodes; i++)
            {
                for (int j = i + 1; j < numFreeNodes; j++)
                {
                    var p_ij = (1.0d / populationSize) * delta1Sums[i, j];
                    delta1[i, j] = 1.0d + (p_ij * Math.Log2(p_ij)) * ((1.0d - p_ij) * Math.Log2(1.0d - p_ij));
                    delta1[j, i] = delta1[i, j];
                }
            }
            return delta1;
        }


        internal static void AddToDelta1Sums(double[,] delta1Sums, RandomKeysTour tour)
        {
            var size = delta1Sums.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var r_i = tour.Keys[i];
                    var r_j = tour.Keys[j];
                    if (r_i < r_j)
                    {
                        delta1Sums[i, j]++;
                    }
                }
            }
        }

        internal static double[,] ComputeDelta2(int numFreeNodes, IEnumerable<RandomKeysTour> population)
        {
            var delta2Sums = new double[numFreeNodes, numFreeNodes];
            foreach (var individual in population)
            {
                AddToDelta1Sums(delta2Sums, individual);
            }

            return ComputeDelta2(delta2Sums, population.Count());
        }

        internal static double[,] ComputeDelta2(double[,] delta2Sums, int populationSize)
        {
            var numFreeNodes = delta2Sums.GetLength(0);

            var delta2 = new double[numFreeNodes, numFreeNodes];
            for (int i = 0; i < numFreeNodes; i++)
            {
                for (int j = i + 1; j < numFreeNodes; j++)
                {
                    delta2[i, j] = 1.0d - (1.0d / populationSize) * delta2Sums[i, j];
                    delta2[j, i] = delta2[i, j];
                }
            }
            return delta2;
        }


        internal static void AddToDelta2Sums(double[,] delta1Sums, RandomKeysTour tour)
        {
            var size = delta1Sums.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var diff = tour.Keys[i] - tour.Keys[j];
                    delta1Sums[i, j] = diff * diff;
                }
            }
        }

    }
}
