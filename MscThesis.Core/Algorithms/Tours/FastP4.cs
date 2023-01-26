using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms.Tours
{
    public class FastP4 : Optimizer<Tour>
    {
        private List<P4Level> _pyramid = new List<P4Level>();
        private FitnessFunction<Tour> _fitnessFunction;
        private (double, double)[] _rescalingIntervals;
        private readonly int _numFreeNodes;
        private readonly int _sheddingInterval;
        private int _sheddingCounter;

        private Population<Tour> Population
        {
            get
            {
                var individuals = _pyramid.Select(level => level.Population.Individuals).SelectMany(x => x);
                return new Population<Tour>(individuals, _comparisonStrategy);
            }
        }

        public FastP4(int problemSize, int sheddingInterval, FitnessFunction<Tour> fitnessFunction) : base(problemSize, fitnessFunction.Comparison)
        {
            _fitnessFunction = fitnessFunction;
            _numFreeNodes = _problemSize - 1;
            _sheddingInterval = sheddingInterval;
            _sheddingCounter = 0;
            _rescalingIntervals = RandomKeysUtils.ComputeRescalingIntervals(_numFreeNodes);
        }

        protected override void Initialize(FitnessFunction<Tour> _)
        {
            AddNewLevel();

            var individual = GenerateUniform();
            _pyramid[0].Add(individual);
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override RunIteration NextIteration(FitnessFunction<Tour> fitnessFunction)
        {
            foreach (var level in _pyramid)
            {
                foreach (var ind in level.Population)
                {
                    ind.Value.ReEncode(_random.Value);
                }
            }

            var individual = GenerateUniform();

            for (int i = 0; i < _pyramid.Count; i++)
            {
                var fitnessPrev = individual.Fitness.Value;
                _pyramid[i].Mix(individual);
                var fitnessNew = individual.Fitness.Value;

                if (!_fitnessFunction.Comparison.IsFitter(fitnessNew, fitnessPrev))
                {
                    continue;
                }

                // Add to next level of pyramid
                if (i == _pyramid.Count - 1)
                {
                    var levelNew = new P4Level(_random.Value, _numFreeNodes, fitnessFunction, _rescalingIntervals);
                    _pyramid.Add(levelNew);
                }
                _pyramid[i + 1].Add(individual);
            }

            if (!_pyramid.Any())
            {
                AddNewLevel();
                _pyramid[0].Add(individual);
            }

            var result = new RunIteration
            {
                Population = Population,
                Statistics = new Dictionary<Property, double>()
            };

            _sheddingCounter++;
            if (_sheddingCounter == _sheddingInterval)
            {
                _pyramid.RemoveAt(0);
                _sheddingCounter = 0;
            }

            return result;
        }

        private Individual<RandomKeysTour> GenerateUniform()
        {
            var uniform = RandomKeysTour.CreateUniform(_random.Value, _problemSize);
            var initialFitness = _fitnessFunction.ComputeFitness(uniform);
            return new IndividualImpl<RandomKeysTour>(uniform, initialFitness);
        }

        private void AddNewLevel()
        {
            _pyramid.Add(new P4Level(_random.Value, _numFreeNodes, _fitnessFunction, _rescalingIntervals));
        }
    }
}
