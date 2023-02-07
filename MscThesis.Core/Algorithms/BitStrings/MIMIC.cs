using System;
using System.Collections.Generic;
using System.Linq;
using MscThesis.Core.Selection;
using MscThesis.Core.Formats;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Algorithms.BitStrings;

namespace MscThesis.Core.Algorithms
{
    public class MIMIC : Optimizer<BitString>
    {
        private readonly ISelectionOperator<BitString> _selectionOperator;
        private readonly int _populationSize;
        private List<BitString> _population;

        public override ISet<Property> StatisticsProperties => new HashSet<Property>
        {
            Property.AvgEntropy
        };

        public MIMIC(
            int problemSize,
            int populationSize,
            ISelectionOperator<BitString> selectionOperator) : base(problemSize)
        {
            _selectionOperator = selectionOperator;
            _populationSize = populationSize;
        }

        protected override void Initialize(FitnessFunction<BitString> fitnessFunction)
        {
            // Initialize population uniformly
            _population = new List<BitString>();
            for (int i = 0; i < _populationSize; i++)
            {
                var bs = BitString.CreateUniform(_random.Value, _problemSize);
                _population.Add(bs);
            }
        }

        protected override RunIteration NextIteration(FitnessFunction<BitString> fitnessFunction)
        {
            var uniCounts = BitStringEntropyUtils.GetUniCounts(_population, _problemSize);
            var uniFreqs = BitStringEntropyUtils.ComputeUniFrequencies(uniCounts, _populationSize);
            var uniEntropies = BitStringEntropyUtils.ComputeUniEntropies(_population, _problemSize);

            var jointCounts = BitStringEntropyUtils.GetJointCounts(_population, _problemSize);
            var jointFreqs = BitStringEntropyUtils.ComputeJointFrequencies(jointCounts, _populationSize);
            var jointEntropies = BitStringEntropyUtils.ComputeJointEntropies(jointFreqs, uniFreqs);

            var ordering = MIMICUtils.GetOrdering(_problemSize, uniEntropies, jointEntropies);

            // Sample solutions from model
            var minProb = 1.0d / _problemSize;
            var maxProb = 1.0d - minProb;
            for (int i = 0; i < _populationSize; i++)
            {
                var vals = new bool[_problemSize];

                // Sample the first variable
                var first = ordering[0];
                var probFirst = uniFreqs[first];
                probFirst = ApplyMargins(probFirst, minProb, maxProb);
                vals[first] = RandomUtils.SampleBernoulli(_random.Value, probFirst);

                // Sample the rest
                for (int k = 1; k < _problemSize; k++)
                {
                    var position = ordering[k];
                    var prev = ordering[k - 1];
                    var prevVal = vals[prev];

                    double[] joint;
                    if (position < prev)
                    {
                        joint = jointFreqs[position, prev];
                    }
                    else
                    {
                        joint = jointFreqs[prev, position];
                    }

                    double p;
                    if (prevVal)
                    {
                        p = joint[3] / uniFreqs[prev];
                    }
                    else
                    {
                        p = joint[2] / (1 - uniFreqs[prev]);
                    }
                    p = ApplyMargins(p, minProb, maxProb);
                    vals[position] = RandomUtils.SampleBernoulli(_random.Value, p);
                }

                var bs = new BitString { Values = vals };
                _population.Add(bs);
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

            var jointIndices = Enumerable.Range(0, _problemSize).Select(i => Enumerable.Range(i + 1, _problemSize - i - 1).Select(j => (i, j))).SelectMany(x => x);
            var avgJointEntropy = jointIndices.Select(idxs => jointEntropies[idxs.i, idxs.j]).Sum() / (_problemSize * (_problemSize + 1)) / 2;

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

        private double ApplyMargins(double p, double min, double max)
        {
            return Math.Min(max, Math.Max(min, p));
        }

    }
}
