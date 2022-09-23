﻿using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Factories;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using System;
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
            var factory = GetTestFactory(spec);
            var tests = factory.BuildTests(spec);
            return GatherResults(tests, spec.ProblemSizes, spec.NumRuns);
        }

        private ITestFactory<InstanceFormat> GetTestFactory(TestSpecification spec)
        {
            var problemName = spec.Problem.Name;
            foreach (var factory in _factories)
            {
                if (factory.Problems.Contains(problemName))
                {
                    return factory;
                }
            }
            throw new Exception("Problem not found");
        }

        private IResult<T> GatherResults<T>(IEnumerable<Test<T>> tests, IEnumerable<int> problemSizes, int numRuns) where T : InstanceFormat
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
