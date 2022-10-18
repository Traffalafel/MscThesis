using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Factories.Expression;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Factories.Problem;
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

        protected IParameterFactory _parameterFactory;

        public TestCaseFactory()
        {
            var expressionFactory = new ExpressionFactory();
            _parameterFactory = new ParameterFactory(expressionFactory);
        }

        protected Dictionary<string, OptimizerFactory<T>> _optimizers;
        protected Dictionary<string, IProblemFactory<T>> _problems;
        protected Dictionary<string, ITerminationFactory<T>> _terminations;

        public IEnumerable<ITestCase<InstanceFormat>> BuildTestCases(TestSpecification spec)
        {
            foreach (var optimizerSpec in spec.Optimizers)
            {
                var optimizerFactory = GetOptimizerFactory(optimizerSpec);
                var buildOptimizerFunc = optimizerFactory.BuildCreator(optimizerSpec);

                var problemFactory = GetProblemFactory(spec.Problem);
                var buildProblemFunc = problemFactory.BuildProblem(spec.Problem);

                var buildTerminationsFunc = new Func<int, IEnumerable<TerminationCriterion<T>>>((size) =>
                {
                    var terminations = new List<TerminationCriterion<T>>();
                    foreach (var terminationSpec in spec.Terminations)
                    {
                        var terminationFactory = GetTerminationFactory(terminationSpec);
                        var creator = terminationFactory.BuildCriterion(terminationSpec, buildProblemFunc);
                        var termination = creator.Invoke(size);
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

                yield return new TestCase<T>(name, buildOptimizerFunc, buildProblemFunc, buildTerminationsFunc);
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
                return _optimizers[algorithmName].Parameters;
            }
            else
            {
                return new List<Parameter>();
            }
        }

        public ProblemDefinition GetProblemDefinition(string problemName)
        {
            if (_problems.ContainsKey(problemName))
            {
                var factory = _problems[problemName];
                return factory.Definition;
            }
            else
            {
                throw new Exception($"Problem \"{problemName}\" was not recognized");
            }
        }

        public ProblemInformation GetProblemInformation(ProblemSpecification spec)
        {
            var factory = GetProblemFactory(spec);
            return factory.GetInformation(spec);
        }

        public IEnumerable<Parameter> GetTerminationParameters(string terminationName)
        {
            if (_terminations.ContainsKey(terminationName))
            {
                return _terminations[terminationName].Parameters;
            }
            else
            {
                return new List<Parameter>();
            }
        }
    }
}
