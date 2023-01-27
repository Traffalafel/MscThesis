using MscThesis.Core.Formats;
using System;

namespace MscThesis.Core.FitnessFunctions
{
    public abstract class FitnessFunction<T> where T : Instance
    {
        private int _numCalls = 0;

        protected int _size;

        public FitnessFunction(int size)
        {
            _size = size;
        }

        protected abstract double Compute(T instance);
        public abstract double? Optimum(int problemSize);
        public abstract FitnessComparison Comparison { get; }

        public int Size => _size;
        public Type InstanceType => typeof(T);

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
