using MscThesis.Core.Formats;
using System;

namespace MscThesis.Core.TerminationCriteria
{
    public class StagnationTermination<T> : TerminationCriterion<T> where T : InstanceFormat
    {
        private readonly double _epsilon;
        private readonly int _maxIterations;
        private double _bestFitnessPrev;
        private int _stagnatedIterations;

        protected override string Message => "stagnation";

        public StagnationTermination(double epsilon, int maxIterations)
        {
            _epsilon = epsilon;
            _maxIterations = maxIterations;
            _bestFitnessPrev = double.MaxValue;
            _stagnatedIterations = 0;
        }

        protected override bool ShouldTerminate(Population<T> pop)
        {
            var fittest = pop.Fittest;
            if (fittest == null)
            {
                throw new Exception("Fittest individual in population cannot be null");
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

            return _stagnatedIterations >= _maxIterations;
        }
    }
}
