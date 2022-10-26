using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms.Tours
{
    public class P4 : Optimizer<Tour>
    {
        private List<P4Level> _pyramid = new List<P4Level>();
        private FitnessFunction<Tour> _fitnessFunction;
        private (double, double)[] _rescalingIntervals;
        private int _numFreeNodes;
        private Population<Tour> Population
        {
            get
            {
                var individuals = _pyramid.Select(level => level.Population.Individuals).SelectMany(x => x);
                return new Population<Tour>(individuals, _comparisonStrategy);
            }
        }

        public P4(Random random, int problemSize, FitnessFunction<Tour> fitnessFunction) : base(random, problemSize, fitnessFunction.ComparisonStrategy)
        {
            _fitnessFunction = fitnessFunction;
            _numFreeNodes = _problemSize - 1;
            _rescalingIntervals = ComputeRescalingIntervals(_numFreeNodes);
            _pyramid.Add(new P4Level(random, _numFreeNodes, fitnessFunction, _rescalingIntervals));

            var uniform = RandomKeysTour.CreateUniform(_random, _problemSize);
            var initialFitness = fitnessFunction.ComputeFitness(uniform);
            var individual = new IndividualImpl<RandomKeysTour>(uniform, initialFitness);
            _pyramid[0].Add(individual);
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override RunIteration<Tour> NextIteration(FitnessFunction<Tour> fitnessFunction)
        {
            foreach (var level in _pyramid)
            {
                foreach (var ind in level.Population)
                {
                    ind.Value.ReEncode(_random);
                }
            }

            var uniform = RandomKeysTour.CreateUniform(_random, _problemSize);
            var initialFitness = fitnessFunction.ComputeFitness(uniform);
            var individual = new IndividualImpl<RandomKeysTour>(uniform, initialFitness);

            for (int i = 0; i < _pyramid.Count; i++)
            {
                var fitnessPrev = individual.Fitness.Value;
                _pyramid[i].Mix(individual);
                var fitnessNew = individual.Fitness.Value;

                if (!_fitnessFunction.ComparisonStrategy.IsFitter(fitnessNew, fitnessPrev))
                {
                    continue;
                }

                // Add to next level of pyramid
                if (i == _pyramid.Count - 1)
                {
                    var levelNew = new P4Level(_random, _numFreeNodes, fitnessFunction, _rescalingIntervals);
                    _pyramid.Add(levelNew);
                }
                _pyramid[i + 1].Add(individual);
            }

            return new RunIteration<Tour>
            {
                Population = Population,
                Statistics = new Dictionary<Property, double>()
            };
        }

        private (double, double)[] ComputeRescalingIntervals(int size)
        {
            var intervalRange = 1.0d / size;
            return Enumerable.Range(0, size)
                             .Select(i => i * intervalRange)
                             .Select(start => (start, start + intervalRange))
                             .ToArray();
        }
    }
}
