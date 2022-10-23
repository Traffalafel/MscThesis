using MscThesis.Core;
using System.Collections.Generic;

namespace MscThesis.Runner
{
    public class ProblemDefinition
    {
        public bool CustomSizesAllowed { get; set; }
        public int? ProblemSize { get; set; }
        public IEnumerable<Parameter> ExpressionParameters { get; set;  }
        public IDictionary<Parameter, IEnumerable<string>> OptionParameters { get; set; }
    }
}
