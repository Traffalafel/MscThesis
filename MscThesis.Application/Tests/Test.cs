using MscThesis.Core;
using MscThesis.Core.Formats;

namespace MscThesis.Runner.Results
{
    public abstract class Test<T> where T : InstanceFormat
    {
        protected bool _isTerminated;
        protected ObservableValue<Individual<T>> _fittest = new ObservableValue<Individual<T>>();

        public bool IsTerminated => _isTerminated;
        public IObservableValue<Individual<T>> Fittest => _fittest;

        protected void TryUpdateFittest(Individual<T> other)
        {
            // Overwrite fittest if better
            if (CompareIndividuals(_fittest.Value, other) < 0)
            {
                _fittest.Value = other;
            }
        }

        // Returns
        // 0 if they are equal
        // >0 if i1 is larger than i2
        // <0 if i2 is larger than i1
        public int CompareIndividuals(Individual<T> i1, Individual<T> i2)
        {
            if (i1 == null && i2 == null)
            {
                return 0;
            }
            if (i1 != null && i2 == null)
            {
                return 1;
            }
            if (i1 == null && i2 != null)
            {
                return -1;
            }

            if (i1.Fitness == null && i2.Fitness == null)
            {
                return 0;
            }
            if (i1.Fitness != null && i2.Fitness == null)
            {
                return 1;
            }
            if (i1.Fitness == null && i2.Fitness != null)
            {
                return -1;
            }

            var diff = i1.Fitness.Value - i2.Fitness.Value;
            return diff == 0.0d ? 0 : diff > 0 ? 1 : -1;
        }
    }
}
