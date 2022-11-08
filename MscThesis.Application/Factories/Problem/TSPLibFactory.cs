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
    public class TSPLibFactory : ProblemFactory<Tour>
    {
        private static readonly string _tspLibDirectoryPath = "TSPLIB95";

        private IEnumerable<string> _tspNames;
        private TspLib95 _tspLib;

        public bool AllowsMultipleSizes => false;
        public override ProblemDefinition GetDefinition(ProblemSpecification spec)
        {
            int? size = null;
            if (spec.Parameters.ContainsKey(Parameter.ProblemName))
            {
                var problemName = spec.Parameters[Parameter.ProblemName];
                var item = _tspLib.GetItemByName(problemName, ProblemType.TSP);
                size = GetSize(item.Problem);
            }

            return new ProblemDefinition
            {
                CustomSizesAllowed = false,
                ProblemSize = size,
                ExpressionParameters = new List<Parameter>(),
                OptionParameters = new Dictionary<Parameter, IEnumerable<string>>
                {
                    [Parameter.ProblemName] = _tspNames
                }
            };
        }

        public TSPLibFactory(TspLib95 tspLib)
        {
            _tspLib = tspLib;
            var tsps = _tspLib.LoadAllTSP();
            _tspNames = tsps.Select(tsp => tsp.Problem.Name).ToList();
        }

        public override Func<int, FitnessFunction<Tour>> BuildProblem(ProblemSpecification spec)
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
            var problemSize = GetSize(problem);

            var description = problem.Comment + $"\nProblem size:{problemSize}";
            
            if (item.OptimalTour != null)
            {
                description += $"\nOptimal tour distance: {item.OptimalTourDistance}";
            } 
            else
            {
                description += "\nOptimal tour not known";
            }

            return new ProblemInformation(problem.Name, description);
        }

        private int GetSize(IProblem problem)
        {
            var nodes = problem.NodeProvider.GetNodes();
            return nodes.Count;
        }
    }
}
