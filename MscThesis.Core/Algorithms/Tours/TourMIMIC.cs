﻿using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    public class TourMIMIC : Optimizer<Tour>
    {
        private readonly ISelectionOperator<Tour> _selectionOperator;
        private readonly int _populationSize;
        private List<Tour> _population;

        public TourMIMIC(
            int problemSize,
            int populationSize,
            ISelectionOperator<Tour> selectionOperator) : base(problemSize)
        {
            _selectionOperator = selectionOperator;
            _populationSize = populationSize;
        }

        protected override void Initialize(FitnessFunction<Tour> _)
        {
            // Initialize population uniformly
            _population = new List<Tour>();
            for (int i = 0; i < _populationSize; i++)
            {
                var permutation = Tour.CreateUniform(_random.Value, _problemSize);
                _population.Add(permutation);
            }
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>
        {
            Property.AvgEntropy
        };

        protected override RunIteration NextIteration(FitnessFunction<Tour> fitnessFunction)
        {
            var numFreeNodes = _problemSize - 1;

            var uniCounts = TourEntropyUtils.GetUniCounts(_population, numFreeNodes);
            var uniFreqs = TourEntropyUtils.ComputeUniFrequencies(uniCounts, _populationSize);
            var uniEntropies = TourEntropyUtils.ComputeUniEntropies(_population, numFreeNodes);

            var jointCounts = TourEntropyUtils.GetJointCounts(_population, numFreeNodes);
            var jointFreqs = TourEntropyUtils.ComputeJointFrequencies(jointCounts, _populationSize);
            var jointEntropies = TourEntropyUtils.ComputeJointEntropies(jointFreqs);

            var ordering = MIMICUtils.GetOrdering(numFreeNodes, uniEntropies, jointEntropies);

            // Sample solutions from model
            var minProb = 1.0d / (numFreeNodes*numFreeNodes);
            var maxProb = 1.0d - minProb;
            for (int i = 0; i < _populationSize; i++)
            {
                var values = new int[numFreeNodes];
                var chosen = new HashSet<int>();

                // Sample the first variable
                var first = ordering[0];
                var distributionFirst = uniFreqs.GetRow(first);
                ApplyMargins(distributionFirst, minProb, maxProb);
                var valueFirst = RandomUtils.SampleDistribution(_random.Value, distributionFirst);
                values[first] = valueFirst;
                chosen.Add(valueFirst);

                // Sample the rest
                for (int k = 1; k < numFreeNodes; k++)
                {
                    var position = ordering[k];
                    var positionPrev = ordering[k - 1];
                    var valuePrev = values[positionPrev];

                    var distribution = jointFreqs.GetLastDimension(positionPrev, position, valuePrev);
                    MarginalizeRemaining(distribution, chosen);
                    ApplyMargins(distribution, minProb, maxProb);
                    var valueNew = RandomUtils.SampleDistribution(_random.Value, distribution);
                    values[position] = valueNew;
                    chosen.Add(valueNew);
                }

                var instance = new Tour(values);
                _population.Add(instance);
            }

            foreach (var individual in _population)
            {
                if (individual.Fitness != null)
                {
                    continue;
                }

                individual.Fitness = fitnessFunction.ComputeFitness(individual);
            }

            _population = _selectionOperator.Select(_random.Value, _population, fitnessFunction).ToList();
            
            var jointIndices = Enumerable.Range(0, numFreeNodes).Select(i => Enumerable.Range(i + 1, numFreeNodes - i - 1).Select(j => (i, j))).SelectMany(x => x);
            var avgJointEntropy = jointIndices.Select(idxs => jointEntropies[idxs.i, idxs.j]).Sum() / (numFreeNodes * (_problemSize + 1)) / 2;

            var stats = new Dictionary<Property, double>()
            {
                { Property.AvgEntropy, avgJointEntropy }
            };

            return new RunIteration
            {
                Population = _population,
                Statistics = stats
            };
        }

        private void MarginalizeRemaining(double[] distribution, HashSet<int> chosen)
        {
            var size = distribution.Length;
            var remaining = Enumerable.Range(0, size).ToHashSet();
            remaining.ExceptWith(chosen);

            foreach (var i in chosen)
            {
                distribution[i] = double.NaN;
            }

            Normalize(distribution, remaining);
        }

        // Set all above or below to min or max and normalize afterwards
        private void ApplyMargins(double[] distribution, double min, double max)
        {
            var size = distribution.Length;

            var nan = new HashSet<int>();
            var modified = new HashSet<int>();
            var notModified = new HashSet<int>();
            for (int i = 0; i < size; i++)
            {
                var p = distribution[i];
                if (double.IsNaN(p))
                {
                    nan.Add(i);
                }
                else if (p < min || p > max)
                {
                    distribution[i] = Math.Min(max, Math.Max(min, p));
                    modified.Add(i);
                }
                else
                {
                    notModified.Add(i);
                }
            }

            if (notModified.Count > 0)
            {
                Normalize(distribution, notModified);
            }
            else
            {
                Normalize(distribution, modified);
            }
        }

        private void Normalize(double[] distribution, HashSet<int> positions)
        {
            var ps = Take(distribution, positions);
            if (ps.Any(p => double.IsNaN(p)))
            {
                throw new Exception();
            }
            var sum = ps.Sum();

            if (sum == 0)
            {
                return;
            }

            foreach (var position in positions)
            {
                distribution[position] /= sum;
            }
        }

        private IEnumerable<T> Take<T>(T[] array, IEnumerable<int> positions)
        {
            foreach (var position in positions)
            {
                yield return array[position];
            }
        }
    }
}
