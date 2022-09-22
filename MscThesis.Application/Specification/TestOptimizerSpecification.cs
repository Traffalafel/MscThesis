using MscThesis.Core;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Specification
{
    public class TestOptimizerSpecification
    {
        public string Algorithm { get; set; }
        public IDictionary<Parameter, double> Parameters { get; set; }

        private string GetName()
        {
            throw new NotImplementedException();
        }
    }
}
