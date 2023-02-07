using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Selection
{
    public class TournamentSelection<T> : ISelectionOperator<T> where T : Instance
    {
        private readonly int _numTournaments;
        private readonly int _tournamentSize;

        public TournamentSelection(int numTournaments, int tournamentSize)
        {
            _numTournaments = numTournaments;
            _tournamentSize = tournamentSize;
        }

        public List<T> Select(Random random, List<T> population, FitnessFunction<T> fitnessFunction)
        {
            var comparison = fitnessFunction.Comparison;
            var output = new List<T>();

            for (int i = 0; i < _numTournaments; i++)
            {
                var sample = Enumerable.Range(0, _tournamentSize).Select(_ =>
                {
                    return RandomUtils.Choose(random, population);
                });
                var fittest = sample.Aggregate((i1, i2) => {
                    i1.Fitness ??= fitnessFunction.ComputeFitness(i1);
                    i2.Fitness ??= fitnessFunction.ComputeFitness(i2);
                    return comparison.IsFitter(i1, i2) ? i1 : i2;
                });
                output.Add(fittest);
            }

            return output;
        }
    }
}
