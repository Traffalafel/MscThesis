using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Framework.Factories.Expression;
using MscThesis.Framework.Factories.Parameters;
using MscThesis.Framework.Factories.Problem;
using MscThesis.Framework.Tests;
using MscThesis.Framework.Specification;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Framework.Factories
{
    public abstract class TestFactory<T> : ITestFactory where T : Instance
    {
        public ISet<string> Algorithms => _optimizers.Keys.ToHashSet();
        public ISet<string> Problems => _problems.Keys.ToHashSet();
        public ISet<string> Terminations => _terminations.Keys.ToHashSet();

        protected ParameterFactory _parameterFactory;

        public TestFactory()
        {
            var expressionFactory = new ExpressionFactory();
            _parameterFactory = new ParameterFactory(expressionFactory);
        }

        protected Dictionary<string, IOptimizerFactory<T>> _optimizers;
        protected Dictionary<string, IProblemFactory<T>> _problems;
        protected Dictionary<string, ITerminationFactory<T>> _terminations;

        protected IOptimizerFactory<T> GetOptimizerFactory(OptimizerSpecification spec)
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

        public ProblemDefinition GetProblemDefinition(ProblemSpecification spec)
        {
            var problemName = spec.Name;
            if (_problems.ContainsKey(problemName))
            {
                var factory = _problems[problemName];
                return factory.GetDefinition(spec);
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

        public ITest BuildTest(TestSpecification spec)
        {
            IEnumerable<double> variableValues = GenerateIterator(spec.VariableSteps);

            var numVariableValues = variableValues.Count();
            var sizeRuns = spec.NumRuns;
            var testRuns = sizeRuns * numVariableValues;
            var maxThreads = spec.MaxParallelization;

            var testBatchSize = (int)Math.Ceiling(maxThreads / testRuns);
            maxThreads /= testBatchSize;
            var sizeBatchSize = (int)Math.Ceiling(maxThreads / sizeRuns);
            maxThreads /= sizeBatchSize;
            var runBatchSize = (int)maxThreads;

            var generators = BuildRunGenerators(spec);
            var tests = new List<ITest>();
            foreach (var generator in generators)
            {
                var isSingleSize = numVariableValues == 1;
                var variableResults = new List<ITest>();
                foreach (var variableValue in variableValues)
                {
                    var problemSize = spec.ProblemSize;
                    VariableSpecification varSpec = null;
                    if (spec.Variable != Parameter.ProblemSize && spec.Variable != null)
                    {
                        varSpec = new VariableSpecification
                        {
                            Variable = spec.Variable.Value,
                            Value = variableValue
                        };
                    }
                    else
                    {
                        problemSize = Convert.ToInt32(variableValue);
                    }

                    var isSingleRun = spec.NumRuns == 1;
                    ITest run;
                    if (isSingleRun)
                    {
                        run = generator(problemSize.Value, isSingleRun && isSingleSize, varSpec);
                    }
                    else
                    {
                        var saveSeries = isSingleSize;
                        run = new MultipleRunsComposite(() => generator(problemSize.Value, saveSeries, varSpec), spec.NumRuns, runBatchSize, saveSeries, varSpec);
                    }
                    variableResults.Add(run);
                }

                ITest sizeResult;
                if (numVariableValues == 1)
                {
                    sizeResult = variableResults.First();
                }
                else
                {
                    sizeResult = new MultipleVariablesComposite(variableResults, sizeBatchSize, spec.Variable.Value);
                }
                tests.Add(sizeResult);
            }

            if (generators.Count() == 1)
            {
                return tests.First();
            }
            else
            {
                return new MultipleOptimizersComposite(tests, testBatchSize);
            }
        }

        private IEnumerable<double> GenerateIterator(StepsSpecification spec)
        {
            if (spec.Stop == null || spec.Step == null)
            {
                yield return spec.Start;
                yield break;
            }

            if (spec.Start < 0 || spec.Stop < 0)
            {
                throw new Exception("Start and stop must both be positive.");
            }
            if (spec.Stop < spec.Start)
            {
                throw new Exception("Start must be lower than stop.");
            }
            if (spec.Step < 0)
            {
                throw new Exception("Cannot have negative step size.");
            }
            if (spec.Step == 0 && spec.Start != spec.Stop)
            {
                throw new Exception("Start and stop must be equal for step size zero.");
            }

            if (spec.Step == 0)
            {
                yield return spec.Start;
                yield break;
            }

            var c = spec.Start;
            do
            {
                yield return c;
                c += spec.Step.Value;
            }
            while (c <= spec.Stop);
        }

        private IEnumerable<Func<int, bool, VariableSpecification, ITest>> BuildRunGenerators(TestSpecification spec)
        {
            var problemFactory = GetProblemFactory(spec.Problem);
            var buildProblemFunc = problemFactory.BuildProblem(spec.Problem);

            foreach (var optimizerSpec in spec.Optimizers)
            {
                var optimizerFactory = GetOptimizerFactory(optimizerSpec);
                var buildOptimizerFunc = optimizerFactory.BuildCreator(optimizerSpec);

                var buildTerminationsFunc = new Func<int, VariableSpecification, IEnumerable<TerminationCriterion>>((problemSize, varSpec) =>
                {
                    var terminations = new List<TerminationCriterion>();
                    foreach (var terminationSpec in spec.Terminations)
                    {
                        var terminationFactory = GetTerminationFactory(terminationSpec);
                        var creator = terminationFactory.BuildCriterion(terminationSpec, buildProblemFunc);
                        var termination = creator(problemSize, varSpec);
                        terminations.Add(termination);
                    }
                    return terminations;
                });

                yield return (int problemSize, bool saveSeries, VariableSpecification variableSpec) =>
                {
                    var problem = buildProblemFunc(problemSize, variableSpec);
                    var optimizer = buildOptimizerFunc(problem, variableSpec);
                    var terminations = buildTerminationsFunc(problemSize, variableSpec);

                    var variableValue = variableSpec?.Value ?? problemSize;

                    var run = optimizer.Run(problem);
                    foreach (var criterion in terminations)
                    {
                        run.AddTerminationCriterion(criterion);
                    }

                    return new SingleRun(run, () => problem.GetNumCalls(), problem.Comparison, optimizerSpec.Name, optimizer.StatisticsProperties, variableValue, saveSeries);
                };
            }
        }
    }
}
