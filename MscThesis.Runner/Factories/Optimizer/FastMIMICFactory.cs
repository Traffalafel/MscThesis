﻿using MscThesis.Core;
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
    public class FastMIMICFactory : IOptimizerFactory<BitString>
    {
        private static readonly double _selectionPercentile = 0.5d;

        private readonly ParameterFactory _parameterFactory;

        public FastMIMICFactory(ParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.PopulationSize,
            Parameter.NumSampledPositions
        };

        public Func<FitnessFunction<BitString>, VariableSpecification, Optimizer<BitString>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            var selection = new PercentileSelection<BitString>(_selectionPercentile);

            return (problem, varSpec) =>
            {
                var populationSize = (int)parameters(Parameter.PopulationSize, problem.Size, varSpec);
                var numSampledPositions = (int)parameters(Parameter.NumSampledPositions, problem.Size, varSpec);
                return new FastMIMIC(problem.Size, populationSize, numSampledPositions, selection);
            };
        }
    }

}
