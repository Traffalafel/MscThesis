using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using System;
using System.Collections.Generic;

namespace MscThesis.Core.Selection
{
    public interface ISelectionOperator<T> where T : Instance
    {
        public List<T> Select(Random random, List<T> population, FitnessFunction<T> fitnessFunction);
    }
}
