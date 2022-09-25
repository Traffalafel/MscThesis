using MscThesis.Core;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public interface IParameterProvider
    {
        public IEnumerable<Parameter> RequiredParameters { get; }
    }
}
