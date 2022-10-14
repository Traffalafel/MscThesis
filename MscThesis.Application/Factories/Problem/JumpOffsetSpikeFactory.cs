using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Problem
{
    public class JumpOffsetSpikeFactory : IProblemFactory<BitString>
    {
        private IParameterFactory _parameterFactory;

        public JumpOffsetSpikeFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public ProblemDefinition Parameters => new ProblemDefinition
        {
            AllowsMultipleSizes = true,
            ExpressionParameters = new List<Parameter>
            {
                Parameter.GapSize
            },
            OptionParameters = new Dictionary<Parameter, IEnumerable<string>>()
        };

        public Func<int, FitnessFunction<BitString>> BuildProblem(ProblemSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return (size) =>
            {
                var gapSize = (int)parameters.Invoke(Parameter.GapSize, size);
                return new JumpOffsetSpike(gapSize);
            };
        }
    }
}
