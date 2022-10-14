using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public interface ITerminationFactory<T> where T : InstanceFormat
    {
        public IEnumerable<Parameter> Parameters { get; }
        public Func<int, TerminationCriterion<T>> BuildCriterion(TerminationSpecification spec);
    }

}
