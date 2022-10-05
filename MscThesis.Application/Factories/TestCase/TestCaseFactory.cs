﻿using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Runner.Factories
{
    public abstract class TestCaseFactory<T> : ITestCaseFactory<T> where T : InstanceFormat
    {
        public ISet<string> Algorithms => _optimizers.Keys.ToHashSet();
        public ISet<string> Problems => _problems.Keys.ToHashSet();
        public ISet<string> Terminations => _terminations.Keys.ToHashSet();

        protected Dictionary<string, OptimizerFactory<T>> _optimizers;
        protected Dictionary<string, IProblemFactory<T>> _problems;
        protected Dictionary<string, ITerminationFactory<T>> _terminations;

        public IEnumerable<ITestCase<InstanceFormat>> BuildTestCases(TestSpecification spec)
        {
            foreach (var optimizerSpec in spec.Optimizers)
            {
                var buildOptimizer = new Func<Optimizer<T>>(() =>
                {
                    var optimizerFactory = GetOptimizerFactory(optimizerSpec);
                    return optimizerFactory.BuildOptimizer(optimizerSpec);
                });

                var buildProblem = new Func<FitnessFunction<T>>(() =>
                {
                    var problemFactory = GetProblemFactory(spec.Problem);
                    return problemFactory.BuildProblem(spec.Problem);
                });

                var buildTerminations = new Func<IEnumerable<TerminationCriterion<T>>>(() =>
                {
                    var terminations = new List<TerminationCriterion<T>>();
                    foreach (var terminationSpec in spec.Terminations)
                    {
                        var terminationFactory = GetTerminationFactory(terminationSpec);
                        var termination = terminationFactory.BuildCriterion(terminationSpec);
                        terminations.Add(termination);
                    }
                    return terminations;
                });

                string name;
                if (!string.IsNullOrWhiteSpace(optimizerSpec.Name))
                {
                    name = optimizerSpec.Name;
                }
                else
                {
                    var paramStrings = optimizerSpec.Parameters.Select(kv => $"{kv.Key}:{kv.Value}");
                    var algoName = optimizerSpec.Algorithm;
                    name = $"{algoName}_{string.Join('_', paramStrings)}";
                }

                yield return new TestCase<T>(name, buildOptimizer, buildProblem, buildTerminations); ;
            }
        }

        protected OptimizerFactory<T> GetOptimizerFactory(OptimizerSpecification spec)
        {
            return _optimizers[spec.Algorithm];
        }

        protected IProblemFactory<T> GetProblemFactory(ProblemSpecification spec)
        {
            return _problems[spec.Name];
        }

        protected ITerminationFactory<T> GetTerminationFactory(TerminationSpecification spec)
        {
            return _terminations[spec.Name];
        }

        public IEnumerable<Parameter> GetAlgorithmParameters(string algorithmName)
        {
            if (_optimizers.ContainsKey(algorithmName))
            {
                return _optimizers[algorithmName].RequiredParameters;
            }
            else
            {
                return new List<Parameter>();
            }
        }

        public IEnumerable<Parameter> GetProblemParameters(string problemName)
        {
            if (_problems.ContainsKey(problemName))
            {
                return _problems[problemName].RequiredParameters;
            }
            else
            {
                return new List<Parameter>();
            }
        }

        public IEnumerable<Parameter> GetTerminationParameters(string terminationName)
        {
            if (_terminations.ContainsKey(terminationName))
            {
                return _terminations[terminationName].RequiredParameters;
            }
            else
            {
                return new List<Parameter>();
            }
        }
    }
}
