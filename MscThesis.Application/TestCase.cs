using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Runner
{

    // TestCase = ONE configuration of ONE algorithm on ONE problem
    internal class TestCase<T> where T : InstanceFormat
    {
        public int NumRuns { get; }

        public RunResult<T> NewRun()
        {
            throw new NotImplementedException();
        }

        // TODO
        // Represents entirely configured combination of 
            // Algorithm
            // Problem
            // Termination criteria
        // Generates 
    }
}
