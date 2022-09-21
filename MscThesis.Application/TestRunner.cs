using MscThesis.Application.Results;
using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Core.TerminationCriteria;
using System;
using System.Collections.Generic;

namespace MscThesis.Application
{
    public class TestRunner
    {
        public RunResult<BitString> TestMIMIC()
        {
            var problemSize = 50;
            var initialPopSize = 100;
            var quartile = 0.5d;
            var maxStagnatedIterations = 5;
            var epsilon = 10E-6;
            var algorithmName = "MIMIC1";

            var gapSize = 8;

            var selection = new QuartileSelection<BitString>(quartile);
            var mimic = new MIMIC(problemSize, initialPopSize, selection);

            var oneMax = new OneMax();
            var results = mimic.GetResults(oneMax);
            
            var termination = new StagnationTermination<BitString>(epsilon, maxStagnatedIterations);
            results = termination.AddTerminationCriterion(results);

            return new RunResult<BitString>(results, oneMax, algorithmName);
        }
 
    }

    // TODO: 
        // Results of MULTIPLE runs of ONE algorithm on ONE problem of ONE size


}
