using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories
{
    public class TourMIMICFactory : IOptimizerFactory<Tour>
    {
        private static readonly double _selectionPercentile = 0.5d;

        private readonly IParameterFactory _parameterFactory;

        public TourMIMICFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.PopulationSize
        };

        public Func<FitnessFunction<Tour>, VariableSpecification, Optimizer<Tour>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);
            var selection = new PercentileSelection<Tour>(_selectionPercentile, SelectionMethod.Minimize);

            return (problem, varSpec) =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problem.Size, varSpec);
                return new TourMIMIC(problem.Size, problem.Comparison, populationSize, selection);
            };
        }
    }

}
