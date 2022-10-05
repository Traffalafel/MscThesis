using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Specification
{
    public class TestSpecification
    {
        public int NumRuns { get; set; }
        public List<int> ProblemSizes { get; set; }
        public double MaxParallelization { get; set; }
        public ProblemSpecification Problem { get; set; }
        public List<OptimizerSpecification> Optimizers { get; set; }
        public List<TerminationSpecification> Terminations { get; set; }

        public TestSpecification()
        {
            NumRuns = 1;
            ProblemSizes = new List<int>();
            Optimizers = new List<OptimizerSpecification>();
            Problem = new ProblemSpecification();
            Terminations = new List<TerminationSpecification>();
        }
    }
}
