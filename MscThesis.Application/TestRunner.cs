using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Results;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner
{
    public class TestRunner
    {
        public IDisplayableResult<BitString> TestMIMIC()
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
            var mimic = new MIMIC(problemSize, random, initialPopSize, selection);

            var oneMax = new OneMax();
            var results = mimic.GetResults(oneMax);
            
            var termination = new StagnationTermination<BitString>(epsilon, maxStagnatedIterations);
            results = termination.AddTerminationCriterion(results);

            return new RunResult<BitString>(results, oneMax, algorithmName);
        }

        public IDisplayableResult<T> SingleRun<T>(
            OptimizationHeuristic<T> heuristic,
            FitnessFunction<T> fitnessFunction,
            IEnumerable<TerminationCriterion<T>> terminationCriteria,
            string name
            ) where T : InstanceFormat
        {
            var results = heuristic.GetResults(fitnessFunction);
            var output = results;
            foreach (var criterion in terminationCriteria)
            {
                output = criterion.AddTerminationCriterion(output);
            }
            return new RunResult<T>(results, fitnessFunction, name);
        }

        public IEnumerable<string> GetAlgorithms()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetRequiredParameters()
        {
            throw new NotImplementedException();
        }

    }

    // TODO: 
        // Results of MULTIPLE runs of ONE algorithm on ONE problem of ONE size


}
