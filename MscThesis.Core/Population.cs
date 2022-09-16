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

        public int Size { get
            {
                return _individuals.Count;
            } 
        }

        public Population()
        {
            _individuals = new List<Individual<T>>();
        }

        public Population(IEnumerable<Individual<T>> individuals)
        {
            _individuals = individuals.ToList();
        }

        public void Add(Individual<T> individual)
        {
            _individuals.Add(individual);
        }

        public IEnumerable<T> GetValues()
        {
            return _individuals.Select(p => p.Value);
        }

        public Individual<T> GetFittest()
        {
            return _individuals.OrderByDescending(p => p.Fitness ?? double.MinValue).FirstOrDefault();
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
    }
}
