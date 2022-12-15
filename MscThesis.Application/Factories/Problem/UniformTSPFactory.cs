using MscThesis.Core;
using MscThesis.Core.FitnessFunctions;
using MscThesis.Core.FitnessFunctions.TSP;
using MscThesis.Core.Formats;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using TspLibNet;
using TspLibNet.DistanceFunctions;
using TspLibNet.Graph.Edges;
using TspLibNet.Graph.EdgeWeights;
using TspLibNet.Graph.FixedEdges;
using TspLibNet.Graph.Nodes;

namespace MscThesis.Runner.Factories.Problem
{
    public class UniformTSPFactory : ProblemFactory<Tour>
    {
        public override Func<int, VariableSpecification, FitnessFunction<Tour>> BuildProblem(ProblemSpecification spec)
        {
            return (problemSize, _) =>
            {
                var nodes = new List<Node2D>();
                var random = RandomUtils.BuildRandom().Value;
                foreach (var id in Enumerable.Range(0, problemSize))
                {
                    var x = random.NextDouble();
                    var y = random.NextDouble();
                    var node = new Node2D(id+1, x, y);
                    nodes.Add(node);
                }

                var nodeProvider = new NodeListBasedNodeProvider(nodes);
                var edgeProvider = new EdgeListBasedEdgeProvider(new List<Edge>());
                var edgeWeightProvider = new FunctionBasedWeightProvider(new Euclidean());
                var fixedEdgesProvider = new EdgeListBasedFixedEdgesProvider();

                var problemNew = new TravelingSalesmanProblem(
                    string.Empty,
                    string.Empty,
                    ProblemType.TSP,
                    nodeProvider,
                    edgeProvider,
                    edgeWeightProvider,
                    fixedEdgesProvider);
                var itemNew = new TspLib95Item(problemNew, null, 0.0d);

                return new TSP(itemNew);
            };
        }
    }
}
