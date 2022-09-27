using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using System.Collections.Generic;
using System.Linq;

namespace MscThesis.Runner
{
    public class TestRunner
    {
        private List<ITestFactory<InstanceFormat>> _factories;

        public TestRunner()
        {
            _factories = new List<ITestFactory<InstanceFormat>>
            {
                new BitStringTestFactory(),
                new PermutationTestFactory()
            };
        }

        public IResult<InstanceFormat> Run(TestSpecification spec)
        {
            var problemName = spec.Problem.Name;
            var factory = GetTestFactory(problemName);
            var tests = factory.BuildTests(spec);
            return GatherResults(tests, spec.ProblemSizes, spec.NumRuns);
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

        public IEnumerable<Parameter> GetProblemParameters(string problemName)
        {
            foreach (var factory in _factories)
            {
                if (factory.Problems.Contains(problemName))
                {
                    return factory.GetProblemParameters(problemName);
                }
            }
            return new List<Parameter>();
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

        private ITestFactory<InstanceFormat> GetTestFactory(string problemName)
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


        public IResult<T> GatherResults<T>(IEnumerable<Test<T>> tests, IEnumerable<int> problemSizes, int numRuns) where T : InstanceFormat
        {
            var results = new List<IResult<T>>();
            foreach (var test in tests)
            {
                var sizeResults = new List<IResult<T>>();
                foreach (var size in problemSizes)
                {
                    var runResults = new List<IResult<T>>();
                    foreach (var _ in Enumerable.Range(0, numRuns))
                    {
                        var result = test.Run(size);
                        runResults.Add(result);
                    }

                    IResult<T> runResult;
                    if (numRuns == 1)
                    {
                        runResult = runResults.First();
                    }
                    else
                    {
                        runResult = new MeanResult<T>(runResults);
                    }
                    sizeResults.Add(runResult);
                }

                IResult<T> sizeResult;
                if (problemSizes.Count() == 1)
                {
                    sizeResult = sizeResults.First();
                }
                else
                {
                    sizeResult = new SeriesResult<T>(sizeResults);
                }
                results.Add(sizeResult);
            }

            if (tests.Count() == 1)
            {
                return results.First();
            }
            else
            {
                return new CollectionResult<T>(results);
            }
        }

    }

}
