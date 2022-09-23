using MscThesis.Core.Formats;

namespace MscThesis.Core.FitnessFunctions
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

        public void Reset()
        {
            _numCalls = 0;
        }
    }
}
