using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core.Algorithms
{
    public abstract class PyramidLevel<T> where T : InstanceFormat
    {
        protected Random _random;
        protected Population<T> _population;
        protected List<HashSet<int>> _clusters;

        public Population<T> Population => _population;

        public PyramidLevel(Random random, FitnessComparisonStrategy comparisonStrategy)
        {
            _random = random;
            _population = new Population<T>(comparisonStrategy);
        }

        public void Add(Individual<T> individual)
        {
            _population.Add(individual);
            RecomputeClusters(individual);
        }

        public void Mix(Individual<T> individual)
        {
            foreach (var cluster in _clusters)
            {
                var numRemaining = _population.Size;
                var different = false;
                do
                {
                    var idx = _random.Next(numRemaining);
                    var donor = _population.Individuals[idx];

                    different = Mix(donor, individual, cluster);

                    Swap(_population.Individuals, idx, numRemaining - 1);
                    numRemaining--;
                }
                while (numRemaining > 0 && !different);
            }
        }

        protected abstract void RecomputeClusters(Individual<T> individual);
        protected abstract bool Mix(Individual<T> donor, Individual<T> dest, HashSet<int> cluster);

        private static void Swap<_>(List<_> list, int a, int b)
        {
            var tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }

    }
}
