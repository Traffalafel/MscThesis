using MscThesis.Core;
using System.Collections.Generic;

namespace MscThesis.Framework.Specification
{
    public class OptimizerSpecification
    {
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
