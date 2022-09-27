using MscThesis.Core;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;

namespace MscThesis.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var runner = new TestRunner();

            var spec = new TestSpecification
            {
                NumRuns = 1,
                ProblemSizes = new List<int>
                {
                    50
                },
                Optimizers = new List<OptimizerSpecification>()
                {
                    new OptimizerSpecification
                    {
                        Name = "HelloWorld",
                        Seed = 420,
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, double>
                        {
                            { Parameter.InitialPopulationSize, 100 },
                            { Parameter.SelectionQuartile, 0.5d }
                        },
                    }
                },
                Terminations = new List<TerminationSpecification>
                {
                    new TerminationSpecification
                    {
                        Name = "Stagnation",
                        Parameters = new Dictionary<Parameter, double>
                        {
                            { Parameter.MaxIterations, 5 },
                            { Parameter.Epsilon, 10E-6 }
                        }
                    }
                },
                Problem = new ProblemSpecification
                {
                    Name = "OneMax",
                    Parameters = new Dictionary<Parameter, double>
                    {
                        // No params for OneMax
                    }
                }
            };

            var result = runner.Run(spec);

            Console.WriteLine($"Fittest: {result.GetFittest()}");

            foreach (var testCase in result.GetOptimizerNames())
            {
                Console.WriteLine($"{testCase}:");

                foreach (var property in result.GetItemProperties(testCase))
                {
                    var value = result.GetItemValue(testCase, property);
                    Console.WriteLine($"{property}: {value}");
                }

                foreach (var property in result.GetSeriesProperties(testCase))
                {
                    var values = result.GetSeriesValues(testCase, property);
                    Console.WriteLine($"{property}: {string.Join(", ", values)}");
                }

                Console.Write("\n");
            }


        }
    }
}
