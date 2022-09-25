﻿using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Termination
{
    public class StagnationFactory<T> : ITerminationFactory<T> where T : InstanceFormat
    {
        public IEnumerable<Parameter> RequiredParameters => new List<Parameter>
        {
            Parameter.MaxIterations,
            Parameter.Epsilon
        };

        public TerminationCriterion<T> BuildCriterion(TerminationSpecification spec)
        {
            var epsilon = spec.Parameters[Parameter.Epsilon];
            var maxStagnatedIterations = Convert.ToInt32(spec.Parameters[Parameter.MaxIterations]);
            return new StagnationTermination<T>(epsilon, maxStagnatedIterations);
        }
    }
}
