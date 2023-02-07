using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Termination
{
    public class GlobalOptimumFactory<T> : ITerminationFactory<T> where T : Instance
    {
        public IEnumerable<Parameter> Parameters => new List<Parameter>();

        public Func<int, VariableSpecification, ITerminationCriterion> BuildCriterion(TerminationSpecification _, Func<int, VariableSpecification, FitnessFunction<T>> buildProblemFunc)
        {
            return (size, varSpec) =>
            {
                var fitnessFunction = buildProblemFunc(size, varSpec);
                if (!fitnessFunction.Optimum(size).HasValue)
                {
                    throw new Exception("Fitness function must have known global optimum");
                }

                return new OptimumReached(fitnessFunction.Optimum(size).Value);
            };

        }
    }
}
