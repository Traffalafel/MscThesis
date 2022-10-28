using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms
{
    public class P3 : Optimizer<BitString>
    {
        private FitnessFunction<BitString> _fitnessFunction;
        private List<P3Level> _pyramid;
        private HashSet<string> _hashset;

        public P3(
            int problemSize,
            FitnessFunction<BitString> fitnessFunction
            ) : base(problemSize, fitnessFunction.ComparisonStrategy)
        {
            _fitnessFunction = fitnessFunction;
            _hashset = new HashSet<string>();
            _pyramid = new List<P3Level>();
        }

        protected override void Initialize(FitnessFunction<BitString> fitnessFunction)
        {
            base.Initialize(fitnessFunction);

            _pyramid.Add(new P3Level(_random.Value, _problemSize, _fitnessFunction));
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override RunIteration<BitString> NextIteration(FitnessFunction<BitString> fitnessFunction)
        {
            var uniform = BitString.CreateUniform(_random.Value, _problemSize);
            var individual = HillClimb(_random.Value, uniform, fitnessFunction);

            if (!Exists(individual))
            {
                _pyramid[0].Add(individual);
                AddToHashset(individual);
            }

            for (int i = 0; i < _pyramid.Count; i++)
            {
                var fitnessPrev = individual.Fitness;

                _pyramid[i].Mix(individual);

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
                    var levelNew = new P3Level(_random.Value, _problemSize, _fitnessFunction);
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

        private Individual<BitString> HillClimb(Random random, BitString solution, FitnessFunction<BitString> fitnessFunction)
        {
            var fitness = fitnessFunction.ComputeFitness(solution);

            var problemSize = solution.Values.Length;
            var options = Enumerable.Range(0, problemSize).ToArray();

            var tried = new HashSet<int>();

            while (tried.Count < problemSize)
            {
                RandomUtils.Shuffle(random, options);
                foreach (var index in options)
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

            return new IndividualImpl<BitString>(solution, fitness);
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
            return new Population<BitString>(individuals, _comparisonStrategy);
        }

    }
}
