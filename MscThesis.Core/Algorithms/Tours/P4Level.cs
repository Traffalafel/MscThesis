using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core.Algorithms.Tours
{
    internal class P4Level : PyramidLevel<RandomKeysTour>
    {
        private int _problemSize;
        private FitnessFunction<Tour> _fitnessFunction;

        private (double, double)[] _rescalingIntervals;

        private double[,] _delta1Sums;
        private double[,] _delta2Sums;

        internal P4Level(Random random, int problemSize, FitnessFunction<Tour> fitnessFunction, (double, double)[] rescalingIntervals) : base(random, fitnessFunction.Comparison)
        {
            _random = random;
            _fitnessFunction = fitnessFunction;
            _problemSize = problemSize;
            _population = new Population<RandomKeysTour>(fitnessFunction.Comparison);
            _delta1Sums = new double[problemSize, problemSize];
            _delta2Sums = new double[problemSize, problemSize];
            _rescalingIntervals = rescalingIntervals;
        }

        protected override bool Mix(RandomKeysTour donor, RandomKeysTour dest, HashSet<int> cluster)
        {
            return RandomKeysUtils.OptimalMixing(_random, donor, dest, cluster, _fitnessFunction, _rescalingIntervals);
        }

        protected override void RecomputeClusters(RandomKeysTour individual)
        {
            RandomKeysUtils.AddToDelta1Sums(_delta1Sums, individual);
            RandomKeysUtils.AddToDelta2Sums(_delta2Sums, individual);
            var delta1 = RandomKeysUtils.ComputeDelta1(_delta1Sums, _population.Size);
            var delta2 = RandomKeysUtils.ComputeDelta2(_delta2Sums, _population.Size);
            var distanceFunc = ClusteringUtils.GetPermutationDistanceFunc(delta1, delta2);
            _clusters = ClusteringUtils.BuildClusters(_problemSize, distanceFunc);
        }
    }
}
