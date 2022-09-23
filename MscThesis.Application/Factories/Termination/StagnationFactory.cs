using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;
using System;

namespace MscThesis.Runner.Factories.Termination
{
    public class StagnationFactory<T> : TerminationFactory<T> where T : InstanceFormat
    {
        public TerminationCriterion<T> BuildCriterion(TerminationSpecification spec)
        {
            var epsilon = spec.Parameters[Parameter.Epsilon];
            var maxStagnatedIterations = Convert.ToInt32(spec.Parameters[Parameter.MaxStagnatedIterations]);
            return new StagnationTermination<T>(epsilon, maxStagnatedIterations);
        }
    }
}
