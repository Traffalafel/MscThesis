﻿using MscThesis.Core;
using System.Collections.Generic;

namespace MscThesis.Runner.Specification
{
    public class ProblemSpecification
    {
        public string Name { get; set; }
        public IDictionary<Parameter, string> Parameters { get; set; }

        public ProblemSpecification()
        {
            Name = "<ProblemName>";
            Parameters = new Dictionary<Parameter, string>();
        }
    }
}
