using MscThesis.Core.Formats;
using System;
using System.Linq;

namespace MscThesis.Core.Selection
{
    public class QuartileSelectionOperator<T> : SelectionOperator<T> where T : InstanceFormat
    {
        private double _quartile;

        public QuartileSelectionOperator(double quartile)
        {
            _quartile = quartile;
        }

        public override Population<T> Select(Population<T> population, FitnessFunction<T> fitnessFunction)
        {
            var targetSize = Convert.ToInt32(Math.Round(population.Size * _quartile));
            var newIndividuals = population.OrderByDescending(i => i.Fitness).Take(targetSize);
            return new Population<T>(newIndividuals);
        }
    }
}
