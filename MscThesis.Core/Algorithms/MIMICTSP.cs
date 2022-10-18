using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    public class MIMICTSP : Optimizer<Permutation>
    {
        private readonly ISelectionOperator<Permutation> _selectionOperator;
        private readonly int _populationSize;

        private Population<Permutation> _population;

        public MIMICTSP(
            Random random,
            int problemSize,
            int populationSize,
            ISelectionOperator<Permutation> selectionOperator) : base(random, problemSize)
        {
            _selectionOperator = selectionOperator;
            _populationSize = populationSize;

            // Initialize population uniformly
            _population = new Population<Permutation>();
            for (int i = 0; i < _populationSize; i++)
            {
                var permutation = Permutation.CreateUniform(random, problemSize);
                _population.Add(new IndividualImpl<Permutation>(permutation));
            }
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>
        {
            Property.AvgEntropy
        };

        protected override RunIteration<Permutation> NextIteration(FitnessFunction<Permutation> fitnessFunction)
        {
            var problemSize = _population.ProblemSize;

            var uniCounts = PermutationUtils.GetUniCounts(_population);
            var uniFreqs = PermutationUtils.ComputeUniFrequencies(uniCounts, _population.Size);
            var uniEntropies = PermutationUtils.ComputeUniEntropies(_population);

            var jointCounts = PermutationUtils.GetJointCounts(_population);
            var jointEntropies = PermutationUtils.ComputeJointEntropies(jointCounts, _population.Size);

            var ordering = MIMICUtils.GetOrdering(problemSize, uniEntropies, jointEntropies);

            // Sample solutions from model
            var minProb = 1.0d / problemSize;
            var maxProb = 1.0d - minProb;
            for (int i = 0; i < _populationSize; i++)
            {
                var values = new int[problemSize];
                var chosen = new HashSet<int>();

                // Sample the first variable
                var first = ordering[0];
                var distributionFirst = uniFreqs.GetRow(first);
                ApplyMargins(distributionFirst, minProb, maxProb);
                var valueFirst = RandomUtils.SampleDistribution(_random, distributionFirst);
                values[first] = valueFirst;
                chosen.Add(valueFirst);

                // Sample the rest
                for (int k = 1; k < problemSize; k++)
                {
                    var position = ordering[k];
                    var positionPrev = ordering[k - 1];
                    var valuePrev = values[positionPrev];

                    var distribution = jointCounts.GetLastDimension(positionPrev, valuePrev, position);
                    ToDistribution(distribution, _populationSize);
                    MarginalizeRemaining(distribution, chosen);
                    ApplyMargins(distribution, minProb, maxProb);
                    var valueNew = RandomUtils.SampleDistribution(_random, distribution);
                    values[position] = valueNew;
                    chosen.Add(valueNew);
                }

                var instance = new Permutation(values);
                _population.Add(new IndividualImpl<Permutation>(instance));
            }

            foreach (var individual in _population)
            {
                if (individual.Fitness != null)
                {
                    continue;
                }

                individual.Fitness = fitnessFunction.ComputeFitness(individual.Value);
            }

            _population = _selectionOperator.Select(_population, fitnessFunction);

            var jointIndices = Enumerable.Range(0, problemSize).Select(i => Enumerable.Range(i + 1, problemSize - i - 1).Select(j => (i, j))).SelectMany(x => x);
            var avgJointEntropy = jointIndices.Select(idxs => jointEntropies[idxs.i, idxs.j]).Sum() / (problemSize * (problemSize + 1)) / 2;

            var stats = new Dictionary<Property, double>()
            {
                { Property.AvgEntropy, avgJointEntropy }
            };

            return new RunIteration<Permutation>
            {
                Population = _population,
                Statistics = stats
            };
        }

        private void ToDistribution(double[] counts, int populationSize)
        {
            for (int i = 0; i < counts.Length; i++)
            {
                counts[i] /= populationSize;
            }
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

        private void ApplyMargins(double[] distribution, double min, double max)
        {
            var size = distribution.Length;

            // Set all above or below to min or max
            var modified = new HashSet<int>();
            for (int i = 0; i < size; i++)
            {
                var p = distribution[i];
                if (p < min || p > max)
                {
                    distribution[i] = Math.Min(max, Math.Max(min, p));
                    modified.Add(i);
                }
            }

            var remaining = Enumerable.Range(0, size).ToHashSet();
            remaining.ExceptWith(modified);
            Normalize(distribution, remaining);
        }

        private void Normalize(double[] distribution, HashSet<int> positions)
        {
            var ps = Take(distribution, positions);
            if (ps.Any(p => p == double.NaN))
            {
                throw new Exception();
            }
            var sum = ps.Sum();

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
