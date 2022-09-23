using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Specification
{
    public class TestSpecification
    {
        public int NumRuns { get; set; }
        public List<int> ProblemSizes { get; set; }

        public IEnumerable<OptimizerSpecification> Optimizers { get; set; }
        public ProblemSpecification Problem { get; set; }
    }
}
