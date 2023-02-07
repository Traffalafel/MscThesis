using MscThesis.Core.Algorithms.BitStrings;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    public class GOMEA : Optimizer<BitString>
    {
        private readonly ISelectionOperator<BitString> _selectionOperator;
        private readonly int _populationSize;
        private List<BitString> _population;

        public GOMEA(
            int problemSize,
            int populationSize,
            ISelectionOperator<BitString> selectionOperator
            ) : base(problemSize)
        {
            _selectionOperator = selectionOperator;
            _populationSize = populationSize;
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override void Initialize(FitnessFunction<BitString> fitnessFunction)
        {
            // Initialize population uniformly
            _population = new List<BitString>();
            for (int i = 0; i < _populationSize; i++)
            {
                var bs = BitString.CreateUniform(_random.Value, _problemSize);
                _population.Add(bs);
            }

            foreach (var individual in _population)
            {
                individual.Fitness = fitnessFunction.ComputeFitness(individual);
            }
            _population = _selectionOperator.Select(_random.Value, _population, fitnessFunction).ToList();
        }

        protected override RunIteration NextIteration(FitnessFunction<BitString> fitnessFunction)
        {
            var uniEntropies = BitStringEntropyUtils.ComputeUniEntropies(_population, _problemSize);
            var jointEntropies = BitStringEntropyUtils.ComputeJointEntropies(_population, _problemSize);

            var distanceFunc = ClusteringUtils.GetEntropyDistanceFunc(jointEntropies, uniEntropies);
            var clusters = ClusteringUtils.BuildClusters(_problemSize, distanceFunc);

            var output = new List<BitString>();
            foreach (var individual in _population)
            {
                var b = Clone(individual);
                var o = Clone(individual);

                var oFitness = individual.Fitness.Value;
                var bFitness = individual.Fitness.Value;

                foreach (var cluster in clusters)
                {
                    var p = RandomUtils.Choose(_random.Value, _population);

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
                o.Fitness = oFitness;
                output.Add(o);
            }

            _population = _selectionOperator.Select(_random.Value, output, fitnessFunction).ToList();
            return new RunIteration
            {
                Population = _population
            };
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
                Values = clone,
                Fitness = values.Fitness
            };
        }

    }
}
