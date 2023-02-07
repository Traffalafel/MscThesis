using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Core.Selection
{
    public class PercentileSelection<T> : ISelectionOperator<T> where T : Instance
    {
        private double _percentile;

        public PercentileSelection(double percentile)
        {
            _percentile = percentile;
        }

        public List<T> Select(Random _, List<T> population, FitnessFunction<T> fitnessFunction)
        {
            var targetSize = Convert.ToInt32(Math.Ceiling(population.Count() * _percentile));
            foreach (var individual in population)
            {
                individual.Fitness ??= fitnessFunction.ComputeFitness(individual);
            }
            var ordered = population.OrderByDescending(i => i.Fitness, fitnessFunction.Comparison);
            return ordered.Take(targetSize).ToList();
        }
    }
}
