using MscThesis.Core.Formats;
using System;

namespace MscThesis.Core.TerminationCriteria
{
    public class StagnationTermination<T> : TerminationCriterion<T> where T : InstanceFormat
    {
        private readonly double _epsilon;
        private readonly int _maxIterations;
        private double? _bestFitnessGlobal;
        private int _stagnatedIterations;
        private FitnessComparison _comparison;

        public override Property Reason => Property.Stagnation;

        public StagnationTermination(double epsilon, int maxIterations, FitnessComparison comparison)
        {
            _epsilon = epsilon;
            _maxIterations = maxIterations;
            _comparison = comparison;
            _bestFitnessGlobal = null;
            _stagnatedIterations = 0;
        }

        public override bool ShouldTerminate(Population<T> pop)
        {
            var fittest = pop.Fittest;
            if (fittest == null)
            {
                throw new Exception("Fittest individual in population cannot be null");
            }
            var bestFitness = fittest.Fitness ?? double.MinValue;

            if (_bestFitnessGlobal == null)
            {
                _bestFitnessGlobal = bestFitness;
                return false;
            }

            var isImprovement = _comparison.IsFitter(bestFitness, _bestFitnessGlobal.Value);
            if (!isImprovement || Math.Abs(_bestFitnessGlobal.Value - bestFitness) < _epsilon)
            {
                _stagnatedIterations++;
            }
            else
            {
                _stagnatedIterations = 0;
            }
            
            if (isImprovement)
            {
                _bestFitnessGlobal = bestFitness;
            }

            return _stagnatedIterations >= _maxIterations;
        }
    }
}
