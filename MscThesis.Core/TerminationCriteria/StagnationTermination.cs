using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.TerminationCriteria
{
    public class StagnationTermination : ITerminationCriterion
    {
        private readonly double _epsilon;
        private readonly int _maxIterations;
        private double? _bestFitnessGlobal;
        private int _stagnatedIterations;
        private FitnessComparison _comparison;

        public Property Reason => Property.Stagnation;

        public StagnationTermination(double epsilon, int maxIterations, FitnessComparison comparison)
        {
            _epsilon = epsilon;
            _maxIterations = maxIterations;
            _comparison = comparison;
            _bestFitnessGlobal = null;
            _stagnatedIterations = 0;
        }

        public bool ShouldTerminate(IEnumerable<double?> fitnesses)
        {
            var bestFitness = _comparison.GetFittest(fitnesses);

            if (_bestFitnessGlobal == null)
            {
                _bestFitnessGlobal = bestFitness;
                return false;
            }

            var isImprovement = _comparison.IsFitter(bestFitness, _bestFitnessGlobal.Value);
            if (!isImprovement || Math.Abs(_bestFitnessGlobal.Value - bestFitness.Value) < _epsilon)
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
