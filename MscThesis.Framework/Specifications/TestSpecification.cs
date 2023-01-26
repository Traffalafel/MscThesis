using MscThesis.Core;
using System;
using System.Collections.Generic;

namespace MscThesis.Framework.Specification
{
    public class TestSpecification
    {
        public int NumRuns { get; set; }
        
        public Parameter? Variable { get; set; }
        public StepsSpecification VariableSteps { get; set; }

        public int? ProblemSize { get; set; }
        public double MaxParallelization { get; set; }
        public ProblemSpecification Problem { get; set; }
        public List<OptimizerSpecification> Optimizers { get; set; }
        public List<TerminationSpecification> Terminations { get; set; }

        public TestSpecification()
        {
            NumRuns = 1;
            Optimizers = new List<OptimizerSpecification>();
            Problem = new ProblemSpecification();
            Terminations = new List<TerminationSpecification>();
        }
    }

    public class StepsSpecification
    {
        public double Start { get; set; }
        public double? Stop { get; set; }
        public double? Step { get; set; }
    }
}
