using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Runner.Factories.Problem
{
    internal class JumpOffsetSpikeFactory : IProblemFactory<BitString>
    {
        public IEnumerable<Parameter> RequiredParameters => new List<Parameter> 
        { 
            Parameter.GapSize
        };

        public FitnessFunction<BitString> BuildProblem(ProblemSpecification spec)
        {
            throw new NotImplementedException();
        }
    }
}
