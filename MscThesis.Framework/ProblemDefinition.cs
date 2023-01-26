using MscThesis.Core;
using System.Collections.Generic;

namespace MscThesis.Framework
{
    public class ProblemDefinition
    {
        public bool CustomSizesAllowed { get; set; } = true;
        public int? ProblemSize { get; set; }
        public IEnumerable<Parameter> ExpressionParameters { get; set;  }
        public IDictionary<Parameter, IEnumerable<string>> OptionParameters { get; set; }
    }
}
