using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Problem
{
    public class DeceptiveLeadingBlocksFactory : ProblemFactory<BitString>
    {
        private IParameterFactory _parameterFactory;

        public DeceptiveLeadingBlocksFactory(IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
        }

        public override ProblemDefinition GetDefinition(ProblemSpecification spec)
        {
            return new ProblemDefinition
            {
                CustomSizesAllowed = true,
                ExpressionParameters = new List<Parameter>
                {
                    Parameter.BlockSize
                },
                OptionParameters = new Dictionary<Parameter, IEnumerable<string>>()
            };
        }

        public override Func<int, FitnessFunction<BitString>> BuildProblem(ProblemSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return (size) =>
            {
                var blockSize = (int)parameters(Parameter.BlockSize, size, null);
                return new DeceptiveLeadingBlocks(size, blockSize);
            };
        }
    }
}
