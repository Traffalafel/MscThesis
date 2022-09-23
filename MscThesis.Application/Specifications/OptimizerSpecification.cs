using MscThesis.Core;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Specification
{
    public class OptimizerSpecification
    {
        public int? Seed { get; set; } = null;
        public string Name { get; set; }
        public string Algorithm { get; set; }
        public IDictionary<Parameter, double> Parameters { get; set; }
        public IEnumerable<TerminationSpecification> TerminationCriteria { get; set; }
    }
}
