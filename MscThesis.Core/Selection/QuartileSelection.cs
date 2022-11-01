using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MscThesis.Core.Selection
{
    public class QuartileSelection<T> : ISelectionOperator<T> where T : InstanceFormat
    {
        private double _quartile;
        private SelectionMethod _method;

        public QuartileSelection(double quartile, SelectionMethod method)
        {
            _quartile = quartile;
            _method = method;
        }

        public Population<T> Select(Random _, Population<T> population, FitnessFunction<T> fitnessFunction)
        {
            var targetSize = Convert.ToInt32(Math.Ceiling(population.Size * _quartile));

            IEnumerable<Individual<T>> ordered;
            if (_method == SelectionMethod.Maximize)
            {
                ordered = population.OrderByDescending(i => i.Fitness);
            }
            else
            {
                ordered = population.OrderBy(i => i.Fitness);
            }
            var newIndividuals = ordered.Take(targetSize);

            return new Population<T>(newIndividuals, fitnessFunction.ComparisonStrategy);
        }
    }
}
