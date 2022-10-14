using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MscThesis.Core.Algorithms
{
    public class GOMEA : Optimizer<BitString>
    {
        private readonly ISelectionOperator<BitString> _selectionOperator;
        private readonly int _populationSize;

        private Population<BitString> _population;

        public GOMEA(
            Random random, 
            int problemSize, 
            int populationSize,
            ISelectionOperator<BitString> selectionOperator
            ) : base(random, problemSize)
        {
            _selectionOperator = selectionOperator;
            _populationSize = populationSize;

            // Initialize population uniformly
            _population = new Population<BitString>();
            for (int i = 0; i < _populationSize; i++)
            {
                var bs = BitString.CreateUniform(problemSize, _random);
                _population.Add(new IndividualImpl<BitString>(bs));
            }
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override void Initialize(FitnessFunction<BitString> fitnessFunction)
        {
            foreach (var individual in _population)
            {
                individual.Fitness = fitnessFunction.ComputeFitness(individual.Value);
            }
            _population = _selectionOperator.Select(_population, fitnessFunction);
        }

        protected override RunIteration<BitString> NextIteration(FitnessFunction<BitString> fitnessFunction)
        {
            var uniEntropies = Utils.ComputeUniEntropies(_population);
            var jointEntropies = Utils.ComputeJointEntropies(_population);
            var clusters = Utils.BuildClusters(uniEntropies, jointEntropies);

            var output = new Population<BitString>();

            foreach (var individual in _population)
            {
                var b = Clone(individual.Value);
                var o = Clone(individual.Value);

                var oFitness = individual.Fitness.Value;
                var bFitness = individual.Fitness.Value;

                foreach (var cluster in clusters)
                {
                    var p = RandomUtils.Choose(_population.Individuals, _random).Value;

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

            _population = _selectionOperator.Select(output, fitnessFunction);
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
