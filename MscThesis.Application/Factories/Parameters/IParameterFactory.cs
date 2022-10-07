using MscThesis.Core;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Parameters
{
    public interface IParameterFactory
    {
        public Func<Parameter, int, double> BuildParameters(IDictionary<Parameter, string> spec);
    }
}
