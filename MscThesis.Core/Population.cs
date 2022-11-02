using MscThesis.Core.Formats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core
{
    public class Population<T> : IEnumerable<Individual<T>> where T : InstanceFormat
    {
        private List<Individual<T>> _individuals;
        private Individual<T> _fittest;
        private FitnessComparison _comparisonStrategy;

        public List<Individual<T>> Individuals => _individuals;
        public Individual<T> Fittest => _fittest;

        public int ProblemSize { get
            {
                var first = _individuals.FirstOrDefault();
                return first != null ? first.Size : 0;
            } 
        }

        public int Size { get
            {
                return _individuals.Count;
            } 
        }

        public Population(FitnessComparison comparisonStrategy)
        {
            _individuals = new List<Individual<T>>();
            _comparisonStrategy = comparisonStrategy;
        }

        public Population(IEnumerable<Individual<T>> individuals, FitnessComparison comparisonStrategy)
        {
            _individuals = individuals.ToList();
            _comparisonStrategy = comparisonStrategy;

            var fittest = individuals.Aggregate((i1, i2) => comparisonStrategy.IsFitter(i1, i2) ? i1 : i2);
            _fittest = fittest;
        }

        public void Add(Individual<T> individual)
        {
            _individuals.Add(individual);

            if (_fittest == null)
            {
                _fittest = individual;
                return;
            }
            if (_comparisonStrategy.IsFitter(individual, _fittest))
            {
                _fittest = individual;
                return;
            }
        }

        public IEnumerable<T> GetValues()
        {
            return _individuals.Select(p => p.Value);
        }

        public IEnumerator<Individual<T>> GetEnumerator()
        {
            foreach (var individual in _individuals)
            {
                yield return individual;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public double GetAverageFitness()
        {
            if (_individuals.Any(ind => ind.Fitness == null))
            {
                throw new Exception();
            }

            return _individuals.Sum(ind => ind.Fitness.Value) / _individuals.Count;
        }

    }
}
