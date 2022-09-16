using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core.TerminationCriterion
{
    public class StagnationTerminationCriterion<T> : TerminationCriterion<T> where T : InstanceFormat
    {
        private readonly double _epsilon;
        private readonly int _maxIterations;
        private double _bestFitnessPrev;
        private int _stagnatedIterations;

        public StagnationTerminationCriterion(double epsilon, int maxIterations)
        {
            _epsilon = epsilon;
            _maxIterations = maxIterations;
            _bestFitnessPrev = double.MaxValue;
            _stagnatedIterations = 0;
        }

        public override void Iteration(Population<T> pop)
        {
            var fittest = pop.GetFittest();
            if (fittest == null)
            {
                return;
            }
            var bestFitness = fittest.Fitness ?? double.MinValue;

            var diff = Math.Abs(_bestFitnessPrev - bestFitness);
            if (diff < _epsilon)
            {
                _stagnatedIterations++;
            }
            else
            {
                _stagnatedIterations = 0;
            }
            _bestFitnessPrev = bestFitness;
        }

        public override bool ShouldTerminate()
        {
            return _stagnatedIterations >= _maxIterations;
        }
    }
}
