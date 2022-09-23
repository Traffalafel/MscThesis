using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Runner
{
    // 1 optimizer on 1 problem
    public class TestImpl<T> : Test<T> where T : InstanceFormat
    {
        private readonly string _name;
        private readonly Optimizer<T> _optimizer;
        private readonly FitnessFunction<T> _fitnessfunction;
        private readonly IEnumerable<TerminationCriterion<T>> _terminationCriteria;

        public TestImpl(string name, Optimizer<T> optimizer, FitnessFunction<T> fitnessFunction, IEnumerable<TerminationCriterion<T>> terminationCriteria)
        {
            _name = name;
            _optimizer = optimizer;
            _fitnessfunction = fitnessFunction;
            _terminationCriteria = terminationCriteria;
        }

        public IResult<T> Run(int size)
        {
            _fitnessfunction.Reset();
            var iterations = _optimizer.Run(_fitnessfunction, size);
            foreach (var criterion in _terminationCriteria)
            {
                iterations = criterion.AddTerminationCriterion(iterations);
            }
            return new RunResult<T>(iterations, _fitnessfunction, _name);
        }
    }
}
