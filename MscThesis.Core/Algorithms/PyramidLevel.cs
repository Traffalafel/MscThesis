using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core.Algorithms
{
    public abstract class PyramidLevel<T> where T : Instance
    {
        protected Random _random;
        protected List<T> _population;
        protected List<HashSet<int>> _clusters;

        public List<T> Population => _population;

        public PyramidLevel(Random random)
        {
            _random = random;
            _population = new List<T>();
        }

        public void Add(T individual)
        {
            _population.Add(individual);
            RecomputeClusters(individual);
        }

        public void Mix(T individual)
        {
            var populationSize = _population.Count;
            foreach (var cluster in _clusters)
            {
                var numRemaining = populationSize;
                var different = false;
                do
                {
                    var idx = _random.Next(numRemaining);
                    var donor = _population[idx];

                    different = Mix(donor, individual, cluster);

                    Swap(_population, idx, numRemaining - 1);
                    numRemaining--;
                }
                while (numRemaining > 0 && !different);
            }
        }

        protected abstract void RecomputeClusters(T individual);
        protected abstract bool Mix(T donor, T dest, HashSet<int> cluster);

        private static void Swap<_>(List<_> list, int a, int b)
        {
            var tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }

    }
}
