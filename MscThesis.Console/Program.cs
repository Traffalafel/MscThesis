using MscThesis.Core;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MscThesis.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var runner = new TestProvider();

            var spec = new TestSpecification
            {
                NumRuns = 30,
                ProblemSizes = new List<int>
                {
                    10,
                    20,
                    30,
                    40,
                    50,
                    60,
                    70,
                    80,
                    90,
                    100
                },
                Optimizers = new List<OptimizerSpecification>()
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, double>
                        {
                            { Parameter.InitialPopulationSize, 10 }
                        },
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, double>
                        {
                            { Parameter.InitialPopulationSize, 20 }
                        },
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, double>
                        {
                            { Parameter.InitialPopulationSize, 30 }
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
                            { Parameter.MaxIterations, 10 },
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

            var timer = new Stopwatch();
            timer.Start();

            var test = runner.Run(spec);
            var task = Task.Run(async () => await test.Execute());
            task.Wait();

            timer.Stop();
            var time = timer.Elapsed;

            Console.WriteLine($"Time elapsed: {time.TotalSeconds} seconds");
            
            Console.WriteLine($"Fittest: {test.Fittest.Value}");

            foreach (var item in test.Items)
            {
                var value = item.Observable.Value;
                Console.WriteLine($"{item.Property}: {value}");
            }

            Console.Write("\n");

            foreach (var series in test.Series)
            {
                var points = series.Points;
                Console.WriteLine($"{series.Property}: {string.Join(", ", points)}");
            }

        }

    }
}
