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
    public class MIMICTSPFactory : OptimizerFactory<Tour>
    {
        private static readonly double _selectionQuartile = 0.5d;

        private readonly IParameterFactory _parameterFactory;

        public MIMICTSPFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public override IEnumerable<Parameter> Parameters => new List<Parameter> 
        { 
            Parameter.PopulationSize
        };

        public override Func<FitnessFunction<Tour>, Optimizer<Tour>> BuildCreator(OptimizerSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);
            var random = BuildRandom(spec.Seed);
            var selection = new QuartileSelection<Tour>(_selectionQuartile, SelectionMethod.Minimize);

            return problem =>
            {
                var populationSize = (int)parameters.Invoke(Parameter.PopulationSize, problem.Size);
                return new MIMICTSP(random, problem.Size, problem.ComparisonStrategy, populationSize, selection);
            };
        }
    }

}
