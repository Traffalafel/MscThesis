using MscThesis.Core.Algorithms.BitStrings;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    internal class P3Level : PyramidLevel<BitString>
    {
        private FitnessFunction<BitString> _fitnessFunction;
        private double[] _uniCounts;
        private double[,,] _jointCounts;

        public P3Level(Random random, int problemSize, FitnessFunction<BitString> fitnessFunction) : base(random, fitnessFunction.Comparison)
        {
            _fitnessFunction = fitnessFunction;
            _uniCounts = new double[problemSize];
            _jointCounts = new double[problemSize, problemSize,4];
        }

        protected override bool Mix(BitString donor, BitString dest, HashSet<int> cluster)
        {
            var fitnessPrev = dest.Fitness;

            var prevValues = cluster.Select(c => (c, dest.Values[c])).ToList();

            var changed = false;
            foreach (var i in cluster)
            {
                if (dest.Values[i] != donor.Values[i])
                {
                    dest.Values[i] = donor.Values[i];
                    changed = true;
                }
            }

            if (!changed)
            {
                return false;
            }

            var fitnessNew = _fitnessFunction.ComputeFitness(dest);
            if (fitnessPrev > fitnessNew)
            {
                // Revert changes
                foreach (var (i, val) in prevValues)
                {
                    dest.Values[i] = val;
                }
            }
            else
            {
                dest.Fitness = fitnessNew;
            }
            return true;
        }

        protected override void RecomputeClusters(BitString individual)
        {
            var populationSize = _population.Count();

            BitStringEntropyUtils.AddToUniCounts(_uniCounts, individual);
            var uniFreqs = BitStringEntropyUtils.ComputeUniFrequencies(_uniCounts, populationSize);
            var uniEntropies = BitStringEntropyUtils.ComputeUniEntropies(uniFreqs);

            BitStringEntropyUtils.AddToJointCounts(_jointCounts, individual);
            var jointFreqs = BitStringEntropyUtils.ComputeJointFrequencies(_jointCounts, populationSize);
            var jointEntropies = BitStringEntropyUtils.ComputeJointEntropies(jointFreqs, uniFreqs);

            var distanceFunc = ClusteringUtils.GetEntropyDistanceFunc(jointEntropies, uniEntropies);
            _clusters = ClusteringUtils.BuildClusters(_fitnessFunction.Size, distanceFunc);
        }
    }
}
