using MscThesis.Core.Formats;
using System;
using System.Linq;

namespace MscThesis.Core.Selection
{
    public class QuartileSelection<T> : ISelectionOperator<T> where T : InstanceFormat
    {
        private double _quartile;

        public QuartileSelection(double quartile)
        {
            _quartile = quartile;
        }

        public Population<T> Select(Population<T> population, FitnessFunction<T> fitnessFunction)
        {
            var targetSize = Convert.ToInt32(Math.Ceiling(population.NumIndividuals * _quartile));
            var newIndividuals = population.OrderByDescending(i => i.Fitness).Take(targetSize);
            return new Population<T>(newIndividuals);
        }
    }
}
