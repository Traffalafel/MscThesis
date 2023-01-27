using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Core.TerminationCriteria;
using MscThesis.Framework.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.Framework.Factories
{
    public interface ITerminationFactory<T> where T : Instance
    {
        public IEnumerable<Parameter> Parameters { get; }
        public Func<int, VariableSpecification, TerminationCriterion> BuildCriterion(TerminationSpecification spec, Func<int, VariableSpecification, FitnessFunction<T>> fitnessCreator);
    }

}
