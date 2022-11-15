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
    public class StagnationFactory<T> : ITerminationFactory<T> where T : InstanceFormat
    {
        private IParameterFactory _parameterFactory;

        public StagnationFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter>
        {
            Parameter.MaxIterations,
            Parameter.Epsilon
        };

        public Func<int, TerminationCriterion<T>> BuildCriterion(TerminationSpecification spec, Func<int, FitnessFunction<T>> fitnessGenerator)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return (size) =>
            {
                var epsilon = parameters(Parameter.Epsilon, size);
                var maxStagnatedIterations = (int) parameters(Parameter.MaxIterations, size);
                var fitnessFunction = fitnessGenerator(size);
                return new StagnationTermination<T>(epsilon, maxStagnatedIterations, fitnessFunction.Comparison);
            };

        }
    }
}
