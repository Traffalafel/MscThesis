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

        public P4(int problemSize, FitnessFunction<Tour> fitnessFunction) : base(problemSize, fitnessFunction.Comparison)
        {
            _fitnessFunction = fitnessFunction;
            _numFreeNodes = _problemSize - 1;
            _rescalingIntervals = RandomKeysUtils.ComputeRescalingIntervals(_numFreeNodes);
        }

        protected override void Initialize(FitnessFunction<Tour> fitnessFunction)
        {
            _pyramid.Add(new P4Level(_random.Value, _numFreeNodes, fitnessFunction, _rescalingIntervals));

            var uniform = RandomKeysTour.CreateUniform(_random.Value, _problemSize);
            uniform.Fitness = fitnessFunction.ComputeFitness(uniform);
            _pyramid[0].Add(uniform);
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override RunIteration NextIteration(FitnessFunction<Tour> fitnessFunction)
        {
            foreach (var level in _pyramid)
            {
                foreach (var ind in level.Population)
                {
                    ind.ReEncode(_random.Value);
                }
            }

            var instance = RandomKeysTour.CreateUniform(_random.Value, _problemSize);
            instance.Fitness = fitnessFunction.ComputeFitness(instance);

            for (int i = 0; i < _pyramid.Count; i++)
            {
                var fitnessPrev = instance.Fitness.Value;
                _pyramid[i].Mix(instance);
                var fitnessNew = instance.Fitness.Value;

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
                _pyramid[i + 1].Add(instance);
            }

            return new RunIteration
            {
                Population = Population,
                Statistics = new Dictionary<Property, double>()
            };
        }

    }
}
