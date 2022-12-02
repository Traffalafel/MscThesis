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
    public class FastTourMIMICFactory : IOptimizerFactory<Tour>
    {
        private static readonly double _selectionQuartile = 0.5d;

        private readonly IParameterFactory _parameterFactory;

        public FastTourMIMICFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.PopulationSize,
            Parameter.NumSampledPositions
        };

        public Func<FitnessFunction<Tour>, Optimizer<Tour>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);
            var selection = new QuartileSelection<Tour>(_selectionQuartile, SelectionMethod.Minimize);

            return problem =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problem.Size);
                var numSampledPositions = (int)parameters.Invoke(Parameter.NumSampledPositions, problem.Size);

                if (numSampledPositions > problem.Size)
                {
                    throw new Exception("Cannot sample more positions than problem size");
                }

                return new FastTourMIMIC(problem.Size, problem.Comparison, populationSize, numSampledPositions, selection);
            };
        }
    }

}
