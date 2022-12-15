using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using TspLibNet;

namespace MscThesis.Runner
{
    public class TestProvider
    {
        private List<ITestCaseFactory<InstanceFormat>> _factories;

        public TestProvider(string tspLibPath)
        {
            var tspLib = new TspLib95(tspLibPath);

            _factories = new List<ITestCaseFactory<InstanceFormat>>
            {
                new BitStringTestCaseFactory(),
                new TourTestCaseFactory(tspLib)
            };
        }

        public ITest<InstanceFormat> Build(TestSpecification spec)
        {
            var variable = spec.Variable ?? Parameter.ProblemSize;

            var problemName = spec.Problem.Name;
            var factory = GetTestFactory(problemName);
            IEnumerable<ITestCase<InstanceFormat>> tests;
            tests = factory.BuildTestCases(spec);

            var problemDef = factory.GetProblemDefinition(spec.Problem);
            if (!problemDef.CustomSizesAllowed)
            {
                if (variable == Parameter.ProblemSize)
                {
                    spec.ProblemSizes = new List<int> { problemDef.ProblemSize.Value };
                }
                else
                {
                    spec.ProblemSize = problemDef.ProblemSize;
                }
            }

            IEnumerable<double> variableValues;
            if (spec.ProblemSizes != null)
            {
                variableValues = spec.ProblemSizes.Select(size => Convert.ToDouble(size));
            }
            else if (spec.VariableValue != null)
            {
                variableValues = new List<double> { spec.VariableValue.Value };
            }
            else
            {
                if (spec.VariableValues != null)
                {
                    variableValues = spec.VariableValues;
                }
                else
                {
                    variableValues = GenerateIterator(spec.VariableSteps);
                }
            }

            return GatherResults(tests, variable, variableValues, spec.NumRuns, spec.MaxParallelization, spec.ProblemSize);
        }

        public List<string> GetProblemNames()
        {
            var names = new List<string>();
            foreach (var factory in _factories)
            {
                names.AddRange(factory.Problems);
            }
            return names;
        }

        public ProblemDefinition GetProblemDefinition(ProblemSpecification spec)
        {
            var problemName = spec.Name;
            foreach (var factory in _factories)
            {
                if (factory.Problems.Contains(problemName))
                {
                    return factory.GetProblemDefinition(spec);
                }
            }
            return new ProblemDefinition
            {
                CustomSizesAllowed = false,
                ExpressionParameters = new List<Parameter>(),
                OptionParameters = new Dictionary<Parameter, IEnumerable<string>>()
            };
        }

        public ProblemInformation GetProblemInformation(ProblemSpecification spec)
        {
            try
            {
                var factory = GetTestFactory(spec.Name);
                return factory.GetProblemInformation(spec);
            }
            catch
            {
                return new ProblemInformation();
            }
        }

        public List<string> GetTerminationNames(string problemName)
        {
            var factory = GetTestFactory(problemName);
            if (factory != null)
            {
                return factory.Terminations.ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public List<string> GetAlgorithmNames(string problemName)
        {
            var factory = GetTestFactory(problemName);
            if (factory != null)
            {
                return factory.Algorithms.ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public IEnumerable<Parameter> GetAlgorithmParameters(string algorithmName)
        {
            foreach (var factory in _factories)
            {
                if (factory.Algorithms.Contains(algorithmName))
                {
                    return factory.GetAlgorithmParameters(algorithmName);
                }
            }
            return new List<Parameter>();
        }

        public IEnumerable<Parameter> GetTerminationParameters(string terminationName)
        {
            foreach (var factory in _factories)
            {
                if (factory.Terminations.Contains(terminationName))
                {
                    return factory.GetTerminationParameters(terminationName);
                }
            }
            return new List<Parameter>();
        }

        private ITestCaseFactory<InstanceFormat> GetTestFactory(string problemName)
        {
            foreach (var factory in _factories)
            {
                if (factory.Problems.Contains(problemName))
                {
                    return factory;
                }
            }
            return null;
        }

        private ITest<T> GatherResults<T>(IEnumerable<ITestCase<T>> tests, Parameter variable, IEnumerable<double> variableSizes, int numRuns, double maxParallelization, int? problemSize) where T : InstanceFormat
        {
            var numSizes = variableSizes.Count();
            var sizeRuns = numRuns;
            var testRuns = sizeRuns * numSizes;

            var testBatchSize = (int)Math.Ceiling(maxParallelization / testRuns);
            maxParallelization /= testBatchSize;

            var sizeBatchSize = (int)Math.Ceiling(maxParallelization / sizeRuns);
            maxParallelization /= sizeBatchSize;

            var runBatchSize = (int)maxParallelization;

            var results = new List<ITest<T>>();
            foreach (var test in tests)
            {
                var isSingleSize = variableSizes.Count() == 1;
                var sizeResults = new List<ITest<T>>();
                foreach (var size in variableSizes)
                {
                    VariableSpecification varSpec = null;
                    if (variable != Parameter.ProblemSize)
                    {
                        varSpec = new VariableSpecification
                        {
                            Variable = variable,
                            Value = size
                        };
                    }
                    else
                    {
                        problemSize = Convert.ToInt32(size);
                    }

                    var isSingleRun = numRuns == 1;
                    ITest<T> run;
                    if (isSingleRun)
                    {
                        run = test.CreateRun(problemSize.Value, isSingleRun && isSingleSize, varSpec);
                    }
                    else
                    {
                        run = new MultipleRunsComposite<T>(test, problemSize.Value, numRuns, runBatchSize, isSingleSize, varSpec);
                    }
                    sizeResults.Add(run);
                }

                ITest<T> sizeResult;
                if (variableSizes.Count() == 1)
                {
                    sizeResult = sizeResults.First();
                }
                else
                {
                    sizeResult = new MultipleVariablesComposite<T>(sizeResults, sizeBatchSize, variable);
                }
                results.Add(sizeResult);
            }

            if (tests.Count() == 1)
            {
                return results.First();
            }
            else
            {
                return new MultipleOptimizersComposite<T>(results, testBatchSize);
            }
        }

        private IEnumerable<double> GenerateIterator(StepsSpecification spec)
        {
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
                c += spec.Step;
            }
            while (c <= spec.Stop);
        }

    }

}
