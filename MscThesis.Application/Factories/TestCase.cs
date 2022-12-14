using MscThesis.Core;
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
        private readonly Func<FitnessFunction<T>, VariableSpecification, Optimizer<T>> _buildOptimizer;
        private readonly Func<int, VariableSpecification, FitnessFunction<T>> _buildProblem;
        private readonly Func<int, VariableSpecification, IEnumerable<TerminationCriterion<T>>> _buildTerminations;

        public TestCase(string name, Func<FitnessFunction<T>, VariableSpecification, Optimizer<T>> buildOptimizer, Func<int, VariableSpecification, FitnessFunction<T>> buildProblem, Func<int, VariableSpecification, IEnumerable<TerminationCriterion<T>>> buildTerminations)
        {
            _name = name;
            _buildOptimizer = buildOptimizer;
            _buildProblem = buildProblem;
            _buildTerminations = buildTerminations;
        }

        public ITest<T> CreateRun(int problemSize, bool saveSeries, VariableSpecification variableSpec)
        {
            var problem = _buildProblem(problemSize, variableSpec);

            var optimizer = _buildOptimizer(problem, variableSpec);
            
            var terminations = _buildTerminations(problemSize, variableSpec);

            var variableValue = variableSpec?.Value ?? problemSize;

            var run = optimizer.Run(problem);
            foreach (var criterion in terminations)
            {
                run.AddTerminationCriterion(criterion);
            }
            return new SingleRun<T>(run, problem, _name, optimizer.StatisticsProperties, variableValue, saveSeries);
        }
    }

    public class VariableSpecification
    {
        public VariableSpecification() { }

        public Parameter Variable { get; set; }
        public double Value { get; set; } 
    }
}
