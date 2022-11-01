using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;

namespace MscThesis.Core.Selection
{
    public interface ISelectionOperator<T> where T : InstanceFormat
    {
        public Population<T> Select(Random random, Population<T> population, FitnessFunction<T> fitnessFunction);
    }
}
