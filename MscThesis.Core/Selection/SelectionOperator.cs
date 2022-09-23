using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;

namespace MscThesis.Core.Selection
{
    public interface ISelectionOperator<T> where T : InstanceFormat
    {
        public Population<T> Select(Population<T> population, FitnessFunction<T> fitnessFunction);
    }
}
