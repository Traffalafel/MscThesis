using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MscThesis.Runner.Specification
{
    public class TestTerminationSpecification
    {
        public string Criterion { get; set; }
        public IDictionary<Parameter, double> Parameters { get; set; }
    }
}
