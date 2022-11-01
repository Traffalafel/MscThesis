using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories;
using MscThesis.Runner.Factories.Problem;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TspLibNet;

namespace MscThesis.Runner
{
    public class TestProvider
    {
        private List<ITestCaseFactory<InstanceFormat>> _factories;

        public TestProvider(Settings settings)
        {
            Console.WriteLine($"CWD: {Directory.GetCurrentDirectory()}");// todo: RM
            Console.WriteLine($"TSPdirpath: {settings.TSPLibDirectoryPath}");

            var tspLib = new TspLib95(settings.TSPLibDirectoryPath);

            _factories = new List<ITestCaseFactory<InstanceFormat>>
            {
                new BitStringTestCaseFactory(),
                new TourTestCaseFactory(tspLib)
            };
        }

        public ITest<InstanceFormat> Build(TestSpecification spec)
        {
            var problemName = spec.Problem.Name;
            var factory = GetTestFactory(problemName);
            var tests = factory.BuildTestCases(spec);

            var problemDef = factory.GetProblemDefinition(spec.Problem);
            List<int> problemSizes;
            if (problemDef.CustomSizesAllowed)
            {
                problemSizes = spec.ProblemSizes;
            }
            else
            {
                problemSizes = new List<int> { problemDef.ProblemSize.Value };
            }

            return GatherResults(tests, problemSizes, spec.NumRuns, spec.MaxParallelization);
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
                var isSingleSize = problemSizes.Count() == 1;
                var sizeResults = new List<ITest<T>>();
                foreach (var size in problemSizes)
                {
                    var isSingleRun = numRuns == 1;
                    ITest<T> run;
                    if (isSingleRun)
                    {
                        run = test.CreateRun(size, isSingleRun && isSingleSize);
                    }
                    else
                    {
                        run = new MultipleRunsComposite<T>(test, size, numRuns, runBatchSize, isSingleSize);
                    }
                    sizeResults.Add(run);
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
