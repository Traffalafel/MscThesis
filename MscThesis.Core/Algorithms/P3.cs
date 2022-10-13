using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    public class P3 : Optimizer<BitString>
    {
        private List<P3Level> _pyramid;
        private HashSet<string> _hashset;

        public P3(
            Random random,
            int problemSize
            ) : base(random, problemSize)
        {
            _hashset = new HashSet<string>();
            _pyramid = new List<P3Level>();
            _pyramid.Add(new P3Level(problemSize, random));
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override RunIteration<BitString> NextIteration(FitnessFunction<BitString> fitnessFunction)
        {
            var uniform = BitString.CreateUniform(_problemSize, _random);
            var individual = HillClimb(uniform, fitnessFunction, _random);

            if (!Exists(individual))
            {
                _pyramid[0].Add(individual);
                AddToHashset(individual);
            }

            for (int i = 0; i < _pyramid.Count; i++)
            {
                var fitnessPrev = individual.Fitness;

                _pyramid[i].Mix(individual, fitnessFunction);

                if (fitnessPrev >= individual.Fitness)
                {
                    continue;
                }
                if (Exists(individual))
                {
                    continue;
                }

                if (i == _pyramid.Count - 1)
                {
                    var levelNew = new P3Level(_problemSize, _random);
                    _pyramid.Add(levelNew);
                }
                _pyramid[i+1].Add(individual);
                AddToHashset(individual);
            }

            return new RunIteration<BitString>
            {
                Population = GetPopulation(),
                Statistics = new Dictionary<Property, double>()
            };
        }

        private Individual<BitString> HillClimb(BitString solution, FitnessFunction<BitString> fitnessFunction, Random random)
        {
            var fitness = fitnessFunction.ComputeFitness(solution);
            var individual = new IndividualImpl<BitString>(solution, fitness);

            var problemSize = solution.Values.Length;
            var options = Enumerable.Range(0, problemSize).ToList();

            var tried = new HashSet<int>();

            while (tried.Count < problemSize)
            {
                foreach (var index in RandomUtils.Shuffle(options, random))
                {
                    if (tried.Contains(index))
                    {
                        continue;
                    }

                    solution.Flip(index);
                    var fitnessNew = fitnessFunction.ComputeFitness(solution);

                    if (fitnessNew > fitness)
                    {
                        tried = new HashSet<int>();
                        fitness = fitnessNew;
                    }
                    else
                    {
                        solution.Flip(index);
                    }
                    tried.Add(index);
                }
            }

            return individual;
        }

        private void AddToHashset(Individual<BitString> individual)
        {
            _hashset.Add(individual.Value.ToString());
        }

        private bool Exists(Individual<BitString> individual)
        {
            return _hashset.Contains(individual.Value.ToString());
        }

        private Population<BitString> GetPopulation()
        {
            var individuals = _pyramid.Select(level => level.Population.Individuals).SelectMany(x => x);
            return new Population<BitString>(individuals);
        }

    }

    internal class P3Level
    {
        private Population<BitString> _population;

        private double[] _uniCounts;
        private double[,,] _jointCounts;

        private Random _random;
        private int _problemSize;
        private List<HashSet<int>> _clusters;

        public P3Level(int problemSize, Random random)
        {
            _problemSize = problemSize;
            _random = random;
            _population = new Population<BitString>();
            _uniCounts = new double[problemSize];
            _jointCounts = new double[problemSize, problemSize,4];
        }

        public Population<BitString> Population => _population;

        public void Add(Individual<BitString> individual)
        {
            _population.Add(individual);
            var populationSize = _population.Count();

            Utils.AddToUniCounts(_uniCounts, individual.Value);
            var uniFreqs = Utils.ComputeUniFrequencies(_uniCounts, populationSize);
            var uniEntropies = Utils.ComputeUniEntropies(uniFreqs);

            Utils.AddToJointCounts(_jointCounts, individual.Value);
            var jointFreqs = Utils.ComputeJointFrequencies(_jointCounts, populationSize);
            var jointEntropies = Utils.ComputeJointEntropies(jointFreqs);

            _clusters = Utils.BuildClusters(uniEntropies, jointEntropies);
        }

        public void Mix(Individual<BitString> individual, FitnessFunction<BitString> fitnessFunction)
        {
            foreach (var cluster in _clusters)
            {
                var other = RandomUtils.Choose(_population.Individuals, _random);
                var fitnessPrev = individual.Fitness;

                var solution = individual.Value;
                var prevValues = cluster.Select(c => (c, solution.Values[c])).ToList();

                var changed = false;
                foreach (var i in cluster)
                {
                    if (solution.Values[i] != other.Value.Values[i])
                    {
                        solution.Values[i] = other.Value.Values[i];
                        changed = true;
                    }
                }

                if (!changed)
                {
                    continue;
                }

                var fitnessNew = fitnessFunction.ComputeFitness(solution);
                if (fitnessPrev > fitnessNew)
                {
                    // Revert changes
                    foreach (var (i, val) in prevValues)
                    {
                        solution.Values[i] = val;
                    }
                }
                else
                {
                    individual.Fitness = fitnessNew;
                }
            }
        }

    }
}
