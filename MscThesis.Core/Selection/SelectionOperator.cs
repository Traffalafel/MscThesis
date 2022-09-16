using MscThesis.Core.InstanceFormats;

namespace MscThesis.Core.Selection
{
    public abstract class SelectionOperator<T> where T : InstanceFormat
    {
        public abstract Population<T> Select(Population<T> population, FitnessFunction<T> fitnessFunction);
    }
}
