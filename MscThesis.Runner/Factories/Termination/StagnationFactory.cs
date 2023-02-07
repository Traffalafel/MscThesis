using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Termination
{
    public class StagnationFactory<T> : ITerminationFactory<T> where T : Instance
    {
        private ParameterFactory _parameterFactory;

        public StagnationFactory(ParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter>
        {
            Parameter.MaxIterations,
            Parameter.Epsilon
        };

        public Func<int, VariableSpecification, ITerminationCriterion> BuildCriterion(TerminationSpecification spec, Func<int, VariableSpecification, FitnessFunction<T>> fitnessGenerator)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return (size, varSpec) =>
            {
                var epsilon = parameters(Parameter.Epsilon, size, null);
                var maxStagnatedIterations = (int) parameters(Parameter.MaxIterations, size, null);
                var fitnessFunction = fitnessGenerator(size, varSpec);
                return new StagnationTermination(epsilon, maxStagnatedIterations, fitnessFunction.Comparison);
            };

        }
    }
}
