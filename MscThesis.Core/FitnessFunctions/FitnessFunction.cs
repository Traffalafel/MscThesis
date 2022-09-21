using MscThesis.Core.Formats;

namespace MscThesis.Core
{
    public abstract class FitnessFunction<T> where T : InstanceFormat
    {
        private int _numCalls = 0;
        protected abstract double Compute(T instance);

        public double ComputeFitness(T instance)
        {
            _numCalls++;
            return Compute(instance);
        }

        public int GetNumCalls()
        {
            return _numCalls;
        }
    }
}
