using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Algorithms.Tours
{
    public class TourGOMEA : Optimizer<Tour>
    {
        private readonly int _numFreeNodes;
        private readonly int _populationSize;
        private readonly int _tournamentSize = 2;
        private (double, double)[] _rescalingIntervals;
        private List<RandomKeysTour> _population;

        public TourGOMEA(
            int problemSize, 
            int populationSize
            ) : base(problemSize)
        {
            _numFreeNodes = problemSize - 1;
            _populationSize = populationSize;
            _rescalingIntervals = RandomKeysUtils.ComputeRescalingIntervals(_numFreeNodes);
        }

        public override ISet<Property> StatisticsProperties => new HashSet<Property>();

        protected override void Initialize(FitnessFunction<Tour> fitnessFunction)
        {
            // Initialize population uniformly
            _population = new List<RandomKeysTour>();
            for (int i = 0; i < _populationSize; i++)
            {
                var uniform = RandomKeysTour.CreateUniform(_random.Value, _problemSize);
                uniform.Fitness = fitnessFunction.ComputeFitness(uniform);
                _population.Add(uniform);
            }

            _population = TournamentSelection(_random.Value, _population, fitnessFunction);
        }

        protected override RunIteration NextIteration(FitnessFunction<Tour> fitnessFunction)
        {
            foreach (var ind in _population)
            {
                ind.ReEncode(_random.Value);
            }

            var clusters = ComputeClusters(_population);

            foreach (var individual in _population)
            {
                foreach (var cluster in clusters)
                {
                    var donor = RandomUtils.Choose(_random.Value, _population);
                    RandomKeysUtils.OptimalMixing(_random.Value, donor, individual, cluster, fitnessFunction, _rescalingIntervals);
                }
            }

            _population = TournamentSelection(_random.Value, _population, fitnessFunction);
            return new RunIteration 
            {
                Population = _population 
            };
        }

        private List<HashSet<int>> ComputeClusters(List<RandomKeysTour> population)
        {
            var delta1 = RandomKeysUtils.ComputeDelta1(_numFreeNodes, population);
            var delta2 = RandomKeysUtils.ComputeDelta2(_numFreeNodes, population);
            var distanceFunc = ClusteringUtils.GetPermutationDistanceFunc(delta1, delta2);
            return ClusteringUtils.BuildClusters(_numFreeNodes, distanceFunc);
        }

        private List<RandomKeysTour> TournamentSelection(Random random, List<RandomKeysTour> population, FitnessFunction<Tour> fitnessFunction)
        {
            var comparison = fitnessFunction.Comparison;
            var output = new List<RandomKeysTour>();
            var numTournaments = population.Count;

            for (int i = 0; i < numTournaments; i++)
            {
                var sample = Enumerable.Range(0, _tournamentSize).Select(_ =>
                {
                    return RandomUtils.Choose(random, population);
                });
                var fittest = sample.Aggregate((i1, i2) => comparison.IsFitter(i1, i2) ? i1 : i2);
                output.Add(fittest);
            }

            return output;
        }

    }
}
