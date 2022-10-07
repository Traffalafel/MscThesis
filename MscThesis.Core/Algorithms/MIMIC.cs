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
        private readonly int _initialPopulationSize;

        public override ISet<Property> StatisticsProperties
        {
            get => new HashSet<Property>
            {
                Property.MinEntropy
            };
        }

        public MIMIC(
            Random random,
            int initialPopulationSize, 
            ISelectionOperator<BitString> selectionOperator) : base(random)
        {
            _selectionOperator = selectionOperator;
            _initialPopulationSize = initialPopulationSize;
        }

        protected override Population<BitString> Initialize(int problemSize)
        {
            // Initialize population uniformly
            var population = new Population<BitString>();
            for (int i = 0; i < _initialPopulationSize; i++)
            {
                var bs = BitString.CreateUniform(problemSize, _random);
                population.Add(new IndividualImpl<BitString>(bs));
            }
            return population;
        }

        protected override RunIteration<BitString> NextIteration(Population<BitString> population, FitnessFunction<BitString> fitnessFunction)
        {
            var values = population.GetValues();
            var problemSize = population.ProblemSize;

            // Compute univariate empirical entropies
            var up = ComputeUnivariateProbabilities(values);
            var uniEntropies = new double[problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                uniEntropies[i] = UnivariateEntropy(up[i]);
            }

            // Compute joint empirical entropies
            var jointProbs = ComputeJointProbabilities(values);
            var jointEntropies = new double[problemSize, problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = i + 1; j < problemSize; j++)
                {
                    var jp = jointProbs[i, j];
                    jointEntropies[i, j] = -jp.Sum(p =>
                    {
                        if (p <= 0 || p >= 1)
                        {
                            return 0.0;
                        }
                        else
                        {
                            return p * Math.Log2(p);
                        }
                    });
                }
            }

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
            for (int i = 0; i < _initialPopulationSize; i++)
            {
                var vals = new bool[problemSize];

                // Sample the first variable
                var first = ordering[0];
                var probFirst = up[first];
                probFirst = ApplyMargins(probFirst, minProb, maxProb);
                vals[first] = Sampling.SampleBit(probFirst, _random);

                // Sample the rest
                for (int k = 1; k < problemSize; k++)
                {
                    var position = ordering[k];
                    var prev = ordering[k - 1];
                    var prevVal = vals[prev];

                    double[] joint;
                    if (position < prev)
                    {
                        joint = jointProbs[position, prev];
                    }
                    else
                    {
                        joint = jointProbs[prev, position];
                    }

                    double p;
                    if (prevVal)
                    {
                        p = joint[3] / up[prev];
                    }
                    else
                    {
                        p = joint[2] / (1 - up[prev]);
                    }
                    p = ApplyMargins(p, minProb, maxProb);
                    vals[position] = Sampling.SampleBit(p, _random);
                }

                var bs = new BitString { Values = vals };
                population.Add(new IndividualImpl<BitString>(bs));
            }

            foreach (var individual in population)
            {
                if (individual.Fitness != null)
                {
                    continue;
                }

                individual.Fitness = fitnessFunction.ComputeFitness(individual.Value);
            }

            population = _selectionOperator.Select(population, fitnessFunction);

            var jointIndices = Enumerable.Range(0, problemSize).Select(i => Enumerable.Range(i + 1, problemSize - i - 1).Select(j => (i, j))).SelectMany(x => x);
            var minJointEntropy = jointIndices.Select(idxs => jointEntropies[idxs.i, idxs.j]).Min();

            var stats = new Dictionary<Property, double>()
            {
                { Property.MinEntropy, minJointEntropy }
            };

            return new RunIteration<BitString>
            {
                Population = population,
                Statistics = stats
            };
        }

        private double[] ComputeUnivariateProbabilities(IEnumerable<BitString> instances)
        {
            var populationSize = instances.Count();
            if (populationSize == 0)
            {
                return new double[0];
            }

            var problemSize = instances.First().Values.Length;

            var oneCounts = new int[problemSize];
            foreach (var instance in instances)
            {
                var vals = instance.Values;
                for (int i = 0; i < problemSize; i++)
                {
                    if (vals[i]) oneCounts[i]++;
                }
            }

            var probabilities = new double[problemSize];
            for (var i = 0; i < problemSize; i++)
            {
                var p = (double)oneCounts[i] / populationSize;
                probabilities[i] = p;
            }

            return probabilities;
        }

        private double ApplyMargins(double p, double min, double max)
        {
            return Math.Min(max, Math.Max(min, p));
        }

        private double[,][] ComputeJointProbabilities(IEnumerable<BitString> instances)
        {
            var populationSize = instances.Count();
            if (populationSize == 0)
            {
                return new double[0,0][];
            }

            var problemSize = instances.First().Values.Length;

            var counts = new int[problemSize,problemSize,4];
            foreach (var instance in instances)
            {
                var vals = instance.Values;
                for (int i = 0; i < problemSize; i++)
                {
                    for (int j = i+1; j < problemSize; j++)
                    {
                        if (!vals[i] && !vals[j]) counts[i,j,0]++;
                        if (!vals[i] && vals[j]) counts[i,j,1]++;
                        if (vals[i] && !vals[j]) counts[i,j,2]++;
                        if (vals[i] && vals[j]) counts[i,j,3]++;
                    }
                }
            }

            var probabilities = new double[problemSize,problemSize][];
            for (var i = 0; i < problemSize; i++)
            {
                for (var j = i+1; j < problemSize; j++)
                {
                    probabilities[i, j] = new double[4];
                    for (var k = 0; k < 4; k++)
                    {
                        var p = (double)counts[i, j, k] / populationSize;
                        probabilities[i,j][k] = p;
                    }
                }
            }

            return probabilities;
        }

        private double UnivariateEntropy(double p)
        {
            if (p <= 0)
            {
                return 0.0;
            }
            if (p >= 1)
            {
                return 0.0;
            }
            else
            {
                return -p * Math.Log2(p) - (1-p) * Math.Log2(1 - p);
            }
        }

    }
}
