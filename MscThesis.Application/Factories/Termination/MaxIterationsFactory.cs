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
    public class MaxIterationsFactory<T> : ITerminationFactory<T> where T : InstanceFormat
    {
        private IParameterFactory _parameterFactory;

        public MaxIterationsFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter>
        {
            Parameter.MaxIterations
        };

        public Func<int, VariableSpecification, TerminationCriterion<T>> BuildCriterion(TerminationSpecification spec, Func<int, VariableSpecification, FitnessFunction<T>> _)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return (size, _) =>
            {
                var maxIterations = (int) parameters(Parameter.MaxIterations, size, null);
                return new MaxIterationsTermination<T>(maxIterations);
            };

        }
    }
}
