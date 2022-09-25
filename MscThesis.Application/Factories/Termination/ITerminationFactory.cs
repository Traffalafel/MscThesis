using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;

namespace MscThesis.Runner.Factories
{
    public interface ITerminationFactory<T> : IParameterProvider where T : InstanceFormat
    {
        public TerminationCriterion<T> BuildCriterion(TerminationSpecification spec);
    }

}
