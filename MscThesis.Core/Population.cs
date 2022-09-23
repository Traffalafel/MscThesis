using MscThesis.Core.Formats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core
{
    public class Population<T> : IEnumerable<IndividualImpl<T>> where T : InstanceFormat
    {
        private List<IndividualImpl<T>> _individuals;

        public int ProblemSize { get
            {
                var first = _individuals.FirstOrDefault();
                return first != null ? first.Size : 0;
            } 
        }

        public int NumIndividuals { get
            {
                return _individuals.Count;
            } 
        }

        public Population()
        {
            _individuals = new List<IndividualImpl<T>>();
        }

        public Population(IEnumerable<IndividualImpl<T>> individuals)
        {
            _individuals = individuals.ToList();
        }

        public void Add(IndividualImpl<T> individual)
        {
            _individuals.Add(individual);
        }

        public IEnumerable<T> GetValues()
        {
            return _individuals.Select(p => p.Value);
        }

        public IndividualImpl<T> GetFittest()
        {
            return _individuals.OrderByDescending(p => p.Fitness ?? double.MinValue).FirstOrDefault();
        }

        public IEnumerator<IndividualImpl<T>> GetEnumerator()
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
