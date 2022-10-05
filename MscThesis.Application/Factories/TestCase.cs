using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Results;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    // 1 optimizer on 1 problem
    public class TestCase<T> : ITestCase<T> where T : InstanceFormat
    {
        private readonly string _name;
        private readonly Func<Optimizer<T>> _buildOptimizer;
        private readonly Func<FitnessFunction<T>> _buildProblem;
        private readonly Func<IEnumerable<TerminationCriterion<T>>> _buildTerminations;

        public TestCase(string name, Func<Optimizer<T>> buildOptimizer, Func<FitnessFunction<T>> buildProblem, Func<IEnumerable<TerminationCriterion<T>>> buildTerminations)
        {
            _name = name;
            _buildOptimizer = buildOptimizer;
            _buildProblem = buildProblem;
            _buildTerminations = buildTerminations;
        }

        public ITest<T> CreateRun(int size)
        {
            var optimizer = _buildOptimizer.Invoke();
            var problem = _buildProblem.Invoke();
            var terminations = _buildTerminations.Invoke();

            var iterations = optimizer.Run(problem, size);
            foreach (var criterion in terminations)
            {
                iterations = criterion.AddTerminationCriterion(iterations);
            }
            return new TestRun<T>(iterations, problem, _name, optimizer.StatisticsProperties, size);
        }
    }
}
