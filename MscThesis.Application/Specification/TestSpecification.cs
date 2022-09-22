using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Specification
{
    public class TestSpecification
    {
        public int? Seed { get; set; } = null;
        public int NumRuns { get; set; }
        public List<int> ProblemSizes { get; set; }

        public IEnumerable<TestOptimizerSpecification> Optimizers { get; set; }
        public IEnumerable<TestTerminationSpecification> TerminationCriteria { get; set; }
        public TestProblemSpecification Problem { get; set; }
    }
}
