using MscThesis.Core.Algorithms.BitStrings;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    public class FastTourMIMIC : Optimizer<Tour>
    {
        private readonly ISelectionOperator<Tour> _selectionOperator;
        private readonly int _populationSize;
        private readonly int _numSampledPositions;

        private Population<Tour> _population;

        private double[,,,] _jointFreqs;
        private double[,] _jointEntropies;

        public FastTourMIMIC(
            int problemSize,
            FitnessComparison comparisonStrategy,
            int populationSize,
            int numSampledPositions,
            ISelectionOperator<Tour> selectionOperator) : base(problemSize, comparisonStrategy)
        {
            _selectionOperator = selectionOperator;
            _populationSize = populationSize;
            _numSampledPositions = numSampledPositions;
        }

        protected override void Initialize(FitnessFunction<Tour> fitnessFunction)
        {
            // Initialize population uniformly
            _population = new Population<Tour>(_comparisonStrategy);
            for (int i = 0; i < _populationSize; i++)
            {
                var permutation = Tour.CreateUniform(_random.Value, _problemSize);
                _population.Add(permutation);
            }

            _jointFreqs = TourEntropyUtils.GetJointCounts(_population);
            _jointEntropies = TourEntropyUtils.ComputeJointEntropies(_jointFreqs);
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>
        {
            Property.AvgEntropy
        };

        protected override RunIteration NextIteration(FitnessFunction<Tour> fitnessFunction)
        {
            var problemSize = _population.ProblemSize;

            var uniCounts = TourEntropyUtils.GetUniCounts(_population);
            var uniFreqs = TourEntropyUtils.ComputeUniFrequencies(uniCounts, _population.Size);
            var uniEntropies = TourEntropyUtils.ComputeUniEntropies(_population);

            var toSample = Math.Min(_numSampledPositions, problemSize);
            var samplePositions = RandomUtils.SampleUnique(_random.Value, toSample, problemSize);
            var jointCountsSample = TourEntropyUtils.GetJointCounts(_population, samplePositions);
            var jointFreqsSample = TourEntropyUtils.ComputeJointFrequencies(jointCountsSample, _populationSize);
            var jointEntropySample = TourEntropyUtils.ComputeJointEntropies(jointFreqsSample);
            for (int i = 0; i < samplePositions.Length; i++)
            {
                for (int j = i + 1; j < samplePositions.Length; j++)
                {
                    var iPos = samplePositions[i];
                    var jPos = samplePositions[j];
                    if (iPos > jPos)
                    {
                        (iPos, jPos) = (jPos, iPos);
                    }

                    for (int k = 0; k < problemSize; k++)
                    {
                        for (int l = 0; l < problemSize; l++)
                        {
                            _jointFreqs[iPos, jPos, k, l] = jointFreqsSample[i, j, k, l];
                            _jointEntropies[iPos, jPos] = jointEntropySample[i, j];
                        }
                    }
                }
            }

            var ordering = MIMICUtils.GetOrdering(problemSize, uniEntropies, _jointEntropies);

            // Sample solutions from model
            var minProb = 1.0d / (problemSize*problemSize);
            var maxProb = 1.0d - minProb;
            for (int i = 0; i < _populationSize; i++)
            {
                var values = new int[problemSize];
                var chosen = new HashSet<int>();

                // Sample the first variable
                var first = ordering[0];
                var distributionFirst = uniFreqs.GetRow(first);
                ApplyMargins(distributionFirst, minProb, maxProb);
                var valueFirst = RandomUtils.SampleDistribution(_random.Value, distributionFirst);
                values[first] = valueFirst;
                chosen.Add(valueFirst);

                // Sample the rest
                for (int k = 1; k < problemSize; k++)
                {
                    var position = ordering[k];
                    var positionPrev = ordering[k - 1];
                    var valuePrev = values[positionPrev];

                    var distribution = _jointFreqs.GetLastDimension(positionPrev, position, valuePrev);
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

            _population = _selectionOperator.Select(_random.Value, _population, fitnessFunction);

            var jointIndices = Enumerable.Range(0, problemSize).Select(i => Enumerable.Range(i + 1, problemSize - i - 1).Select(j => (i, j))).SelectMany(x => x);
            var avgJointEntropy = jointIndices.Select(idxs => _jointEntropies[idxs.i, idxs.j]).Sum() / (problemSize * (problemSize + 1)) / 2;

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
