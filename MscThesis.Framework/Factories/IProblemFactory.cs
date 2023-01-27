﻿using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Framework.Factories.Problem;
using MscThesis.Framework.Specification;
using System;

namespace MscThesis.Framework.Factories
{
    public interface IProblemFactory<T> where T : Instance
    {
        public ProblemDefinition GetDefinition(ProblemSpecification spec);
        public ProblemInformation GetInformation(ProblemSpecification spec);
        public Func<int, VariableSpecification, FitnessFunction<T>> BuildProblem(ProblemSpecification spec);
    }

}
