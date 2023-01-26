using MscThesis.Core;
using MscThesis.Core.Algorithms;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.Selection;
using MscThesis.Framework.Factories.Parameters;
using MscThesis.Framework.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Framework.Factories
{
    public class FastTourMIMICFactory : IOptimizerFactory<Tour>
    {
        private static readonly double _selectionPercentile = 0.5d;

        private readonly ParameterFactory _parameterFactory;

        public FastTourMIMICFactory(ParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.PopulationSize,
            Parameter.NumSampledPositions
        };

        public Func<FitnessFunction<Tour>, VariableSpecification, Optimizer<Tour>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);
            var selection = new PercentileSelection<Tour>(_selectionPercentile, SelectionMethod.Minimize);

            return (problem, varSpec) =>
            {
                var populationSize = (int)parameters(Parameter.PopulationSize, problem.Size, varSpec);
                var numSampledPositions = (int)parameters(Parameter.NumSampledPositions, problem.Size, varSpec);

                if (numSampledPositions > problem.Size)
                {
                    throw new Exception("Cannot sample more positions than problem size");
                }

                return new FastTourMIMIC(problem.Size, problem.Comparison, populationSize, numSampledPositions, selection);
            };
        }
    }

}
