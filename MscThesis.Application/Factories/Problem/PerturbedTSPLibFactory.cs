﻿using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.FitnessFunctions.TSP;
using MscThesis.Core.Formats;
using MscThesis.Runner.Factories.Parameters;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using TspLibNet;
using TspLibNet.Graph.EdgeWeights;
using TspLibNet.Graph.Nodes;

namespace MscThesis.Runner.Factories.Problem
{
    public class PerturbedTSPLibFactory : ProblemFactory<Tour>
    {
        private IParameterFactory _parameterFactory;
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
                ExpressionParameters = new List<Parameter>
                {
                    Parameter.StdDeviation
                },
                OptionParameters = new Dictionary<Parameter, IEnumerable<string>>
                {
                    [Parameter.ProblemName] = _tspNames
                }
            };
        }

        public PerturbedTSPLibFactory(TspLib95 tspLib, IParameterFactory parameterFactory)
        {
            _parameterFactory = parameterFactory;
            _tspLib = tspLib;
            var tsps = _tspLib.LoadAllTSP();

            tsps = tsps.Where(tsp =>
            {
                // Only choose TSP instances where nodes weights are computed
                var problem = tsp.Problem;
                if (!(problem.NodeProvider is NodeListBasedNodeProvider)) return false;
                if (!(problem.EdgeWeightsProvider is FunctionBasedWeightProvider)) return false;
                return true;
            });

            _tspNames = tsps.Select(tsp => tsp.Problem.Name).ToList();
        }

        public override Func<int, FitnessFunction<Tour>> BuildProblem(ProblemSpecification spec)
        {
            var name = spec.Parameters[Parameter.ProblemName];
            var item = _tspLib.TSPItems().Where(item => item.Problem.Name == name).FirstOrDefault();

            var size = item.Problem.NodeProvider.CountNodes();
            var expressionParams = new Dictionary<Parameter, string> { [Parameter.StdDeviation] = spec.Parameters[Parameter.StdDeviation] };
            var parameters = _parameterFactory.BuildParameters(expressionParams);
            var stdDeviation = parameters(Parameter.StdDeviation, size);

            if (item == null)
            {
                throw new Exception();
            }

            var random = RandomUtils.BuildRandom().Value;
            var provider = item.Problem.NodeProvider as NodeListBasedNodeProvider;
            foreach (var node in provider.Nodes.ToList())
            {
                if (!(node is Node2D))
                {
                    throw new Exception("Only 2D nodes supported for perturbed TSP");
                }

                var node2d = node as Node2D;

                node2d.X += RandomUtils.SampleStandard(random, 0, stdDeviation);
                node2d.Y += RandomUtils.SampleStandard(random, 0, stdDeviation);
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