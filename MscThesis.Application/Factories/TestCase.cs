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
        private readonly Func<FitnessFunction<T>, Optimizer<T>> _buildOptimizer;
        private readonly Func<int, FitnessFunction<T>> _buildProblem;
        private readonly Func<int, IEnumerable<TerminationCriterion<T>>> _buildTerminations;

        public TestCase(string name, Func<FitnessFunction<T>, Optimizer<T>> buildOptimizer, Func<int, FitnessFunction<T>> buildProblem, Func<int, IEnumerable<TerminationCriterion<T>>> buildTerminations)
        {
            _name = name;
            _buildOptimizer = buildOptimizer;
            _buildProblem = buildProblem;
            _buildTerminations = buildTerminations;
        }

        public ITest<T> CreateRun(int size, bool saveSeries)
        {
            var problem = _buildProblem.Invoke(size);
            var optimizer = _buildOptimizer.Invoke(problem);
            var terminations = _buildTerminations.Invoke(size);

            var run = optimizer.Run(problem);
            foreach (var criterion in terminations)
            {
                run.AddTerminationCriterion(criterion);
            }
            return new SingleRun<T>(run, problem, _name, optimizer.StatisticsProperties, size, saveSeries);
        }
    }
}
