using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.FitnessFunctions.TSP;
using MscThesis.Core.Formats;
using MscThesis.Framework.Factories.Parameters;
using MscThesis.Framework.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using TspLibNet;
using TspLibNet.DistanceFunctions;
using TspLibNet.Graph.Edges;
using TspLibNet.Graph.EdgeWeights;
using TspLibNet.Graph.FixedEdges;
using TspLibNet.Graph.Nodes;

namespace MscThesis.Framework.Factories.Problem
{
    public class PerturbedTSPLibFactory : ProblemFactory<Tour>
    {
        private ParameterFactory _parameterFactory;
        private IEnumerable<string> _tspNames;
        private TspLib95 _tspLib;

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
                    Parameter.StdDeviationScale
                },
                OptionParameters = new Dictionary<Parameter, IEnumerable<string>>
                {
                    [Parameter.ProblemName] = _tspNames
                }
            };
        }

        public PerturbedTSPLibFactory(TspLib95 tspLib, ParameterFactory parameterFactory)
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
                var provider = problem.EdgeWeightsProvider as FunctionBasedWeightProvider;
                if (!(provider.DistanceFunction is Euclidean)) return false;
                return true;
            });

            tsps = tsps.OrderBy(tsp => tsp.Problem.NodeProvider.CountNodes());

            _tspNames = tsps.Select(tsp => tsp.Problem.Name).ToHashSet();
        }

        public override Func<int, VariableSpecification, FitnessFunction<Tour>> BuildProblem(ProblemSpecification spec)
        {
            var name = spec.Parameters[Parameter.ProblemName];

            var itemOriginal = _tspLib.TSPItems().Where(item => item.Problem.Name == name).FirstOrDefault();
            if (itemOriginal == null)
            {
                throw new Exception($"Could not find TSP item {name}");
            }

            var problemSize = itemOriginal.Problem.NodeProvider.CountNodes();

            var nodeProviderOriginal = itemOriginal.Problem.NodeProvider as NodeListBasedNodeProvider;
            if (nodeProviderOriginal.GetNodes().Any(node => !(node is Node2D)))
            {
                throw new Exception("Only 2D nodes supported for perturbed TSP");
            }
            var nodesOriginal = nodeProviderOriginal.GetNodes().Select(node => node as Node2D);

            var meanX = nodesOriginal.Select(node => node.X).Sum();
            var varianceX = nodesOriginal.Select(node => Math.Pow(node.X - meanX, 2)).Sum() / problemSize;
            var stdDevX = Math.Sqrt(varianceX);
            
            var meanY = nodesOriginal.Select(node => node.Y).Sum();
            var varianceY = nodesOriginal.Select(node => Math.Pow(node.Y - meanY, 2)).Sum() / problemSize;
            var stdDevY = Math.Sqrt(varianceX);

            var minX = nodesOriginal.Select(node => node.X).Min();
            var maxX = nodesOriginal.Select(node => node.X).Max();
            var minY = nodesOriginal.Select(node => node.Y).Min();
            var maxY = nodesOriginal.Select(node => node.Y).Max();

            return (problemSize, varSpec) =>
            {
                double stdDeviationScale;
                if (varSpec != null && varSpec.Variable == Parameter.StdDeviationScale)
                {
                    stdDeviationScale = varSpec.Value;
                }
                else
                {
                    var expressionParams = new Dictionary<Parameter, string> { 
                        [Parameter.StdDeviationScale] = spec.Parameters[Parameter.StdDeviationScale] 
                    };
                    var parameters = _parameterFactory.BuildParameters(expressionParams);
                    stdDeviationScale = parameters(Parameter.StdDeviationScale, problemSize, null);
                }

                var nodes = new List<Node2D>();
                var random = RandomUtils.BuildRandom().Value;
                foreach (var nodeOriginal in nodesOriginal)
                {
                    var x = nodeOriginal.X + RandomUtils.SampleStandard(random, 0, stdDevX * stdDeviationScale);
                    var y = nodeOriginal.Y + RandomUtils.SampleStandard(random, 0, stdDevY * stdDeviationScale);
                    x = WrapAround(x, minX, maxX);
                    y = WrapAround(y, minY, maxY);
                    var node = new Node2D(nodeOriginal.Id, x, y);
                    nodes.Add(node);
                }

                var nodeProvider = new NodeListBasedNodeProvider(nodes);
                var edgeProvider = new EdgeListBasedEdgeProvider(new List<Edge>());
                var edgeWeightProvider = new FunctionBasedWeightProvider(new Euclidean());
                var fixedEdgesProvider = new EdgeListBasedFixedEdgesProvider();

                var problemNew = new TravelingSalesmanProblem(
                    name,
                    itemOriginal.Problem.Comment,
                    ProblemType.TSP,
                    nodeProvider,
                    edgeProvider,
                    edgeWeightProvider,
                    fixedEdgesProvider);
                var itemNew = new TspLib95Item(problemNew, null, 0.0d);

                return new TSP(itemNew);
            };
        }

        private double WrapAround(double val, double min, double max)
        {
            var output = val;
            while (output < min || output > max)
            {
                if (output < min)
                {
                    var diff = Math.Abs(min - output);
                    output = max - diff;
                }
                if (output > max)
                {
                    var diff = Math.Abs(output - max);
                    output = min + diff;
                }
            }
            return output;
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
