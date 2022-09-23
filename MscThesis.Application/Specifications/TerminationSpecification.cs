using MscThesis.Core;
using System.Collections.Generic;

namespace MscThesis.Runner.Specification
{
    public class TerminationSpecification
    {
        public string Name { get; set; }
        public IDictionary<Parameter, double> Parameters { get; set; }
    }
}
