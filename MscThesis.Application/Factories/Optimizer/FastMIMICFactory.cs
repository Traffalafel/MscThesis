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
    public class FastMIMICFactory : OptimizerFactory<BitString>
    {
        private static readonly double _selectionQuartile = 0.5d;

        private readonly IParameterFactory _parameterFactory;

        public FastMIMICFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public override IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.PopulationSize,
            Parameter.NumSampledPositions
        };

        public override Func<FitnessFunction<BitString>, Optimizer<BitString>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            var selection = new QuartileSelection<BitString>(_selectionQuartile, SelectionMethod.Maximize);

            return problem =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problem.Size);
                var numSampledPositions = (int)parameters.Invoke(Parameter.NumSampledPositions, problem.Size);
                return new FastMIMIC(problem.Size, problem.Comparison, populationSize, numSampledPositions, selection);
            };
        }
    }

}
