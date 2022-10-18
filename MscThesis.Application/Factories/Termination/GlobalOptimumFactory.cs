using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Termination
{
    public class GlobalOptimumFactory<T> : ITerminationFactory<T> where T : InstanceFormat
    {
        public IEnumerable<Parameter> Parameters => new List<Parameter>();

        public Func<int, TerminationCriterion<T>> BuildCriterion(TerminationSpecification _, Func<int, FitnessFunction<T>> buildProblemFunc)
        {
            return (size) =>
            {
                var fitnessFunction = buildProblemFunc(size);
                if (!fitnessFunction.Optimum.HasValue)
                {
                    throw new Exception("Fitness function must have known global optimum");
                }

                return new OptimumReached<T>(fitnessFunction.Optimum.Value);
            };

        }
    }
}
