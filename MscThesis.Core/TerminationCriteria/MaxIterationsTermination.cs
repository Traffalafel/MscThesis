using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Core.TerminationCriteria
{
    public class MaxIterationsTermination<T> : TerminationCriterion<T> where T : InstanceFormat
    {
        private int _numPrevIterations;
        private int _maxIterations;

        public MaxIterationsTermination(int maxIterations)
        {
            _numPrevIterations = 0;
            _maxIterations = maxIterations;
        }

        protected override bool ShouldTerminate(Population<T> pop)
        {
            if (_numPrevIterations >= _maxIterations)
            {
                return true;
            }
            else
            {
                _numPrevIterations++;
                return false;
            }
        }
    }
}
