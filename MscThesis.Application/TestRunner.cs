using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MscThesis.Runner
{
    public class TestRunner
    {
        public IResult<BitString> TestMIMIC()
        {
            var problemSize = 50;
            var initialPopSize = 100;
            var quartile = 0.5d;
            var maxStagnatedIterations = 5;
            var epsilon = 10E-6;
            var algorithmName = "MIMIC1";

            var gapSize = 8;
            var seed = 420;

            var random = new Random(420);

            var selection = new QuartileSelection<BitString>(quartile);
            var mimic = new MIMIC(random, initialPopSize, selection);

            var oneMax = new OneMax();
            var results = mimic.Run(oneMax, problemSize);
            
            var termination = new StagnationTermination<BitString>(epsilon, maxStagnatedIterations);
            results = termination.AddTerminationCriterion(results);

            return new RunResult<BitString>(results, oneMax, algorithmName);
        }

        public IResult<InstanceFormat> BuildAndRun(TestSpecification spec)
        {
            var tests = Build(spec);
            return RunTests(tests, spec.ProblemSizes, spec.NumRuns);
        }

        public IResult<T> RunTests<T>(IEnumerable<Test<T>> tests, IEnumerable<int> problemSizes, int numRuns) where T : InstanceFormat
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

        private IEnumerable<Test<InstanceFormat>> Build(TestSpecification spec)
        {
            FitnessFunction<InstanceFormat> fitnessFunction;
            if (spec.Problem.Name == "OneMax")
            {
                fitnessFunction = new OneMax();
            }
            else
            {
                throw new Exception("");
            }

            var mimic = new MIMIC(new Random(), 0, new QuartileSelection<BitString>(0.4));
            var criteria = new List<TerminationCriterion<BitString>>();
            return new Test<BitString>("", mimic, fitnessFunction, criteria);

            throw new NotImplementedException();
        }

    }

}
