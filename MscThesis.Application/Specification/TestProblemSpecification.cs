using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MscThesis.Runner.Specification
{
    public class TestProblemSpecification
    {
        public string Name { get; set; }
        public IDictionary<Parameter, double> Parameters { get; set; }
    }
}
