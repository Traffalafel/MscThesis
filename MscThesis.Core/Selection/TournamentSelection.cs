﻿using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Linq;

namespace MscThesis.Core.Selection
{
    public class TournamentSelection<T> : ISelectionOperator<T> where T : InstanceFormat
    {
        private readonly int _numTournaments;
        private readonly int _tournamentSize;

        public TournamentSelection(int numTournaments, int tournamentSize)
        {
            _numTournaments = numTournaments;
            _tournamentSize = tournamentSize;
        }

        public Population<T> Select(Random random, Population<T> population, FitnessFunction<T> problem)
        {
            var output = new Population<T>(problem.Comparison);

            for (int i = 0; i < _numTournaments; i++)
            {
                var sample = Enumerable.Range(0, _tournamentSize).Select(_ =>
                {
                    return RandomUtils.Choose(random, population.Individuals);
                });
                var fittest = sample.Aggregate((i1, i2) => (i1.Fitness ?? double.MinValue) > (i2.Fitness ?? double.MinValue) ? i1 : i2);
                output.Add(fittest);
            }

            return output;
        }
    }
}
