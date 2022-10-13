using System;
using System.Collections.Generic;
using System.Linq;
using MscThesis.Core.Selection;
using MscThesis.Core.Formats;
using MscThesis.Core.FitnessFunctions;

namespace MscThesis.Core.Algorithms
{
    public class MIMIC : Optimizer<BitString>
    {        
        private readonly ISelectionOperator<BitString> _selectionOperator;
        private readonly int _populationSize;

        private Population<BitString> _population;

        public override ISet<Property> StatisticsProperties
        {
            get => new HashSet<Property>
            {
                Property.AvgEntropy
            };
        }

        public MIMIC(
            Random random,
            int problemSize,
            int populationSize, 
            ISelectionOperator<BitString> selectionOperator) : base(random, problemSize)
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

        protected override RunIteration<BitString> NextIteration(FitnessFunction<BitString> fitnessFunction)
        {
            var problemSize = _population.ProblemSize;

            var uniCounts = Utils.GetUniCounts(_population);
            var uniFreqs = Utils.ComputeUniFrequencies(uniCounts, _population.Size);
            var uniEntropies = Utils.ComputeUniEntropies(_population);

            var jointCounts = Utils.GetJointCounts(_population);
            var jointFreqs = Utils.ComputeJointFrequencies(jointCounts, _population.Size);
            var jointEntropies = Utils.ComputeJointEntropies(jointFreqs);

            var remaining = new HashSet<int>(Enumerable.Range(0, problemSize));
            var ordering = new int[problemSize];

            // Find lowest univariate entropy and set to start of chain
            var (_, minIdx) = uniEntropies.Select((e, i) => (e, i)).Min();
            ordering[0] = minIdx;
            remaining.Remove(minIdx);

            // Find lowest pairwise entropies and build chain
            for (int pos = 1; pos < problemSize; pos++)
            {
                var posPrev = ordering[pos - 1];
                var (_, iMin) = remaining.Select(i =>
                {
                    if (i < posPrev)
                    {
                        return (jointEntropies[i, posPrev], i);
                    }
                    else
                    {
                        return (jointEntropies[posPrev, i], i);
                    }
                }).Min();

                ordering[pos] = iMin;
                remaining.Remove(iMin);
            }

            var minProb = 1.0d / problemSize;
            var maxProb = 1.0d - minProb;

            // Sample solutions from model
            for (int i = 0; i < _populationSize; i++)
            {
                var vals = new bool[problemSize];

                // Sample the first variable
                var first = ordering[0];
                var probFirst = uniFreqs[first];
                probFirst = ApplyMargins(probFirst, minProb, maxProb);
                vals[first] = RandomUtils.SampleBit(probFirst, _random);

                // Sample the rest
                for (int k = 1; k < problemSize; k++)
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
                    vals[position] = RandomUtils.SampleBit(p, _random);
                }

                var bs = new BitString { Values = vals };
                _population.Add(new IndividualImpl<BitString>(bs));
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
            var avgJointEntropy = jointIndices.Select(idxs => jointEntropies[idxs.i, idxs.j]).Sum() / (problemSize*(problemSize+1))/2;

            var stats = new Dictionary<Property, double>()
            {
                { Property.AvgEntropy, avgJointEntropy }
            };

            return new RunIteration<BitString>
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
