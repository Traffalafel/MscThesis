using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MscThesis.Core
{
    public class MIMIC : OptimizationHeuristic<BitString>
    {
        private int _initialPopSize;
        private double _quartile;

        public MIMIC(int intitialPopSize, double quartile)
        {
            _initialPopSize = intitialPopSize;
            _quartile = quartile;
        }

        public override BitString Optimize(FitnessFunction<BitString> function, int problemSize)
        {
            // Initialize population uniformly
            var population = new List<Individual<BitString>>();
            for (int i = 0; i < _initialPopSize; i++)
            {
                var bs = BitString.GenerateUniformly(problemSize);
                population.Add(new Individual<BitString>(bs));
            }

            var c = 1000;
            
            // While stopping criterion not met
            while (c > 0)
            {
                var values = population.Select(p => p.Value);

                // Compute univariate empirical entropies
                var up = ComputeUnivariateProbabilities(values);
                var uniEntropies = new double[problemSize];
                for (int i = 0; i < problemSize; i++)
                {
                    uniEntropies[i] = ComputeEntropy(up[i]);
                }

                // Compute joint empirical entropies
                var jointProbs = ComputeJointProbabilities(values);
                var jointEntropies = new double[problemSize,problemSize];
                for (int i = 0; i < problemSize; i++)
                {
                    for (int j = 0; j < problemSize; j++)
                    {
                        var jp = jointProbs[i,j];
                        var entropy =
                            - ComputeJointEntropyTerm(jp[0], up[j])
                            - ComputeJointEntropyTerm(jp[1], (1-up[j]))
                            - ComputeJointEntropyTerm(jp[2], up[j])
                            - ComputeJointEntropyTerm(jp[3], (1-up[j]));
                        jointEntropies[i,j] = entropy;
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
                    var (_, iMin) = remaining.Select(i => (jointEntropies[i, posPrev], i)).Min();

                    ordering[pos] = iMin;
                    remaining.Remove(iMin);
                }

                // Sample solutions from model
                for (int i = 0; i < _initialPopSize; i++)
                {
                    var vals = new bool[problemSize];

                    // Sample the first variable
                    var first = ordering[0];
                    var probFirst = up[first];
                    vals[first] = Utils.SampleBit(probFirst);

                    // Sample the rest
                    for (int k = 1; k < problemSize; k++)
                    {
                        var position = ordering[k];
                        var prev = ordering[k - 1];
                        var prevVal = vals[prev];
                        var joint = jointProbs[position, prev];

                        double p;
                        if (prevVal)
                        {
                            p = joint[3] / up[prev];
                        }
                        else
                        {
                            p = joint[2] / (1 - up[prev]);
                        }
                        vals[position] = Utils.SampleBit(p);
                    }

                    var bs = new BitString { Values = vals };
                    population.Add(new Individual<BitString>(bs));
                }

                foreach (var individual in population)
                {
                    if (individual.Fitness != null)
                    {
                        continue;
                    }

                    individual.Fitness = function.ComputeFitness(individual.Value);
                }

                var populationSize = population.Count();
                var targetSize = Convert.ToInt32(Math.Round(populationSize * _quartile));
                population = population.OrderByDescending(i => i.Fitness).Take(targetSize).ToList();

                c--;
            }

            return population.First().Value;
        }

        public double ComputeJointEntropyTerm(double pXY, double pX)
        {
            if (pXY == 0 || pX == 0)
            {
                return 0;
            }
            return pXY * Math.Log2(pXY / pX);
        }

        public double[] ComputeUnivariateProbabilities(IEnumerable<BitString> instances)
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
            //var minProb = 1.0d / problemSize;
            //var maxProb = 1.0d - minProb;
            for (var i = 0; i < problemSize; i++)
            {
                var p = (double)oneCounts[i] / populationSize;
                probabilities[i] = p;
                //probabilities[i] = Math.Min(maxProb, Math.Max(minProb, p));
            }

            return probabilities;
        }

        public double[,][] ComputeJointProbabilities(IEnumerable<BitString> instances)
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
                    for (int j = 0; j < problemSize; j++)
                    {
                        if (!vals[i] && !vals[j]) counts[i,j,0]++;
                        if (!vals[i] && vals[j]) counts[i,j,1]++;
                        if (vals[i] && !vals[j]) counts[i,j,2]++;
                        if (vals[i] && vals[j]) counts[i,j,3]++;
                    }
                }
            }

            //var minProb = 1.0d / Math.Pow(problemSize, 2.0d);
            //var maxProb = 1.0d - minProb;
            var probabilities = new double[problemSize,problemSize][];
            for (var i = 0; i < problemSize; i++)
            {
                for (var j = 0; j < problemSize; j++)
                {
                    probabilities[i, j] = new double[4];
                    //var marginNet = 0.0d;
                    //var affected = new HashSet<int>(Enumerable.Range(0, 4));
                    for (var k = 0; k < 4; k++)
                    {
                        var p = (double)counts[i, j, k] / populationSize;
                        //if (p < minProb)
                        //{
                        //    marginNet -= minProb - p;
                        //    p = minProb;
                        //    affected.Remove(k);
                        //}
                        //if (p > maxProb)
                        //{
                        //    marginNet += p - maxProb;
                        //    p = maxProb;
                        //    affected.Remove(k);
                        //}
                        probabilities[i,j][k] = p;
                    }

                    //if (affected.Count < 4)
                    //{
                    //    foreach (var k in affected)
                    //    {
                    //        var prev = probabilities[i, j][k];
                    //        probabilities[i, j][k] *= 1 + marginNet;
                    //    }
                    //}

                }
            }

            return probabilities;
        }

        public double ComputeEntropy(double p)
        {
            return -p * Math.Log2(p) - (1-p) * Math.Log2(1 - p);
        }


    }
}
