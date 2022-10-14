using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.FitnessFunctions.TSP;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using TspLibNet;

namespace MscThesis.Runner.Factories.Problem
{
    public class TSPLibFactory : ProblemFactory<Permutation>
    {
        private IEnumerable<string> _tspNames;
        private TspLib95 _tspLib;

        public bool AllowsMultipleSizes => false;
        public override ProblemDefinition Definition => new ProblemDefinition
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

        public override Func<int, FitnessFunction<Permutation>> BuildProblem(ProblemSpecification spec)
        {
            var name = spec.Parameters[Parameter.ProblemName];
            var item = _tspLib.TSPItems().Where(item => item.Problem.Name == name).FirstOrDefault();

            if (item == null)
            {
                throw new Exception();
            }

            return _ => new TSP(item);
        }

        public override ProblemInformation GetInformation(ProblemSpecification spec)
        {
            var problemName = spec.Parameters[Parameter.ProblemName];
            var item = _tspLib.TSPItems().Where(item => item.Problem.Name == problemName).FirstOrDefault();
            var problem = item.Problem;

            return new ProblemInformation(problem.Name, problem.Comment);
        }
    }
}
