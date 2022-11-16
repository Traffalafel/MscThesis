using MscThesis.Core.Algorithms.BitStrings;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using System.Collections.Generic;

namespace MscThesis.Core.Algorithms
{
    public class GOMEA : Optimizer<BitString>
    {
        private readonly ISelectionOperator<BitString> _selectionOperator;
        private readonly int _populationSize;

        private Population<BitString> _population;

        public GOMEA(
            int problemSize,
            FitnessComparison comparisonStrategy,
            int populationSize,
            ISelectionOperator<BitString> selectionOperator
            ) : base(problemSize, comparisonStrategy)
        {
            _selectionOperator = selectionOperator;
            _populationSize = populationSize;
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override void Initialize(FitnessFunction<BitString> fitnessFunction)
        {
            base.Initialize(fitnessFunction);

            // Initialize population uniformly
            _population = new Population<BitString>(_comparisonStrategy);
            for (int i = 0; i < _populationSize; i++)
            {
                var bs = BitString.CreateUniform(_random.Value, _problemSize);
                _population.Add(new IndividualImpl<BitString>(bs));
            }

            foreach (var individual in _population)
            {
                individual.Fitness = fitnessFunction.ComputeFitness(individual.Value);
            }
            _population = _selectionOperator.Select(_random.Value, _population, fitnessFunction);
        }

        protected override RunIteration<BitString> NextIteration(FitnessFunction<BitString> fitnessFunction)
        {
            var uniEntropies = BitStringEntropyUtils.ComputeUniEntropies(_population);
            var jointEntropies = BitStringEntropyUtils.ComputeJointEntropies(_population);
            var clusters = ClusteringUtils.BuildClusters(uniEntropies, jointEntropies);

            var output = new Population<BitString>(_comparisonStrategy);

            foreach (var individual in _population)
            {
                var b = Clone(individual.Value);
                var o = Clone(individual.Value);

                var oFitness = individual.Fitness.Value;
                var bFitness = individual.Fitness.Value;

                foreach (var cluster in clusters)
                {
                    var p = RandomUtils.Choose(_random.Value, _population.Individuals).Value;

                    Copy(p, o, cluster);

                    if (Equals(o, b, cluster))
                    {
                        continue;
                    }

                    var fitnessNew = fitnessFunction.ComputeFitness(o);

                    if (fitnessNew > bFitness)
                    {
                        Copy(o, b, cluster);
                        oFitness = fitnessNew;
                        bFitness = fitnessNew;
                    }
                    else
                    {
                        Copy(b, o, cluster);
                    }
                }

                var mixed = new IndividualImpl<BitString>(o, oFitness);
                output.Add(mixed);
            }

            _population = _selectionOperator.Select(_random.Value, output, fitnessFunction);
            return new RunIteration<BitString>(_population);
        }

        private bool Equals(BitString bs1, BitString bs2, IEnumerable<int> indices)
        {
            foreach (var index in indices)
            {
                if (bs1.Values[index] != bs2.Values[index])
                {
                    return false;
                }
            }
            return true;
        }

        private void Copy(BitString source, BitString dest, IEnumerable<int> indices)
        {
            foreach (var index in indices)
            {
                dest.Values[index] = source.Values[index];
            }
        }

        private BitString Clone(BitString values)
        {
            var size = values.Size;
            var clone = new bool[size];
            for (int i = 0; i < size; i++)
            {
                clone[i] = values.Values[i];
            }
            return new BitString
            {
                Values = clone
            };
        }

    }
}
