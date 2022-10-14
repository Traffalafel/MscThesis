﻿using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories;
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

        public TestProvider(Settings settings)
        {
            var tspLib = new TspLib95(settings.TSPLibDirectoryPath);

            _factories = new List<ITestCaseFactory<InstanceFormat>>
            {
                new BitStringTestCaseFactory(),
                new PermutationTestCaseFactory(tspLib)
            };
        }

        public ITest<InstanceFormat> Run(TestSpecification spec)
        {
            var problemName = spec.Problem.Name;
            var factory = GetTestFactory(problemName);
            var tests = factory.BuildTestCases(spec);
            return GatherResults(tests, spec.ProblemSizes, spec.NumRuns, spec.MaxParallelization);
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

        public ProblemDefinition GetProblemParameters(string problemName)
        {
            foreach (var factory in _factories)
            {
                if (factory.Problems.Contains(problemName))
                {
                    return factory.GetProblemDefinition(problemName);
                }
            }
            throw new Exception($"Problem \"{problemName}\" is not recognized");
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

        private ITest<T> GatherResults<T>(IEnumerable<ITestCase<T>> tests, IEnumerable<int> problemSizes, int numRuns, double maxParallelization) where T : InstanceFormat
        {
            var numSizes = problemSizes.Count();
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
                var sizeResults = new List<ITest<T>>();
                foreach (var size in problemSizes)
                {
                    var runResults = new List<ITest<T>>();
                    foreach (var _ in Enumerable.Range(0, numRuns))
                    {
                        var result = test.CreateRun(size);
                        runResults.Add(result);
                    }

                    ITest<T> runResult;
                    if (numRuns == 1)
                    {
                        runResult = runResults.First();
                    }
                    else
                    {
                        runResult = new MultipleRunsComposite<T>(runResults, runBatchSize);
                    }
                    sizeResults.Add(runResult);
                }

                ITest<T> sizeResult;
                if (problemSizes.Count() == 1)
                {
                    sizeResult = sizeResults.First();
                }
                else
                {
                    sizeResult = new MultipleSizesComposite<T>(sizeResults, sizeBatchSize, Property.ProblemSize);
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

    }

}
