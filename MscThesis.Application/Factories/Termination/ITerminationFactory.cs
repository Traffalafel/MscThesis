using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;
using System;

namespace MscThesis.Runner.Factories
{
    public interface ITerminationFactory<T> : IParameterProvider where T : InstanceFormat
    {
        public Func<int, TerminationCriterion<T>> BuildCriterion(TerminationSpecification spec);
    }

}
