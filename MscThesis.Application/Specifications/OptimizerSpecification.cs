using MscThesis.Core;
using System.Collections.Generic;

namespace MscThesis.Runner.Specification
{
    public class OptimizerSpecification
    {
        public int? Seed { get; set; } = null;
        public string Name { get; set; }
        public string Algorithm { get; set; }
        public IDictionary<Parameter, string> Parameters { get; set; }

        public OptimizerSpecification()
        {
            Name = "<OptimizerName>";
            Algorithm = "<AlgorithmName>";
            Parameters = new Dictionary<Parameter, string>();
        }
    }
}
