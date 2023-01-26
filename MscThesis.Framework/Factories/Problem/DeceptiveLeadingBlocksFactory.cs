using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Framework.Factories.Parameters;
using MscThesis.Framework.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Framework.Factories.Problem
{
    public class DeceptiveLeadingBlocksFactory : ProblemFactory<BitString>
    {
        private ParameterFactory _parameterFactory;

        public DeceptiveLeadingBlocksFactory(ParameterFactory parameterFactory)
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

        public override Func<int, VariableSpecification, FitnessFunction<BitString>> BuildProblem(ProblemSpecification spec)
        {
            var parameters = _parameterFactory.BuildParameters(spec.Parameters);

            return (size, varSpec) =>
            {
                int blockSize;
                if (varSpec != null && varSpec.Variable == Parameter.BlockSize)
                {
                    blockSize = (int)varSpec.Value;
                }
                else
                {
                    blockSize = (int)parameters(Parameter.BlockSize, size, null);
                }

                return new DeceptiveLeadingBlocks(size, blockSize);
            };
        }
    }
}
