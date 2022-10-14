using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using TspLibNet;

namespace MscThesis.Runner.Factories.Problem
{
    public class TSPLibFactory : IProblemFactory<Permutation>
    {
        private IEnumerable<string> _tspNames;
        private TspLib95 _tspLib;

        public bool AllowsMultipleSizes => false;
        public ProblemDefinition Parameters => new ProblemDefinition
        {
            ExpressionParameters = new List<Parameter>(),
            OptionParameters = new Dictionary<Parameter, IEnumerable<string>>
            {
                [Parameter.ProblemName] = _tspNames
            }
        };

        public TSPLibFactory(TspLib95 tspLib)
        {
            _tspLib = tspLib;
            var tsps = tspLib.LoadAllTSP();
            _tspNames = tsps.Select(tsp => tsp.Problem.Name).ToList();
        }

        public Func<int, FitnessFunction<Permutation>> BuildProblem(ProblemSpecification spec)
        {
            var name = spec.Parameters[Parameter.ProblemName];
            var tsp = _tspLib.TSPItems().Where(item => item.Problem.Name == name).FirstOrDefault();

            if (tsp == null)
            {
                throw new Exception();
            }

            throw new NotImplementedException();
        }
    }
}
