using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core
{
    public abstract class FitnessFunction<T> where T : InstanceFormat
    {
        private int _numCalls = 0;

        public abstract double ComputeFitness(T instance);

        public int GetNumCalls()
        {
            return _numCalls;
        }
    }
}
