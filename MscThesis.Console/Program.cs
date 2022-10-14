using MscThesis.Core;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = new Settings
            {
                TSPLibDirectoryPath = string.Empty
            };

            var runner = new TestProvider(settings);

            var spec = new TestSpecification
            {
                NumRuns = 100,
                ProblemSizes = new List<int>
                {
                    100
                },
                MaxParallelization = 20,
                Optimizers = new List<OptimizerSpecification>()
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.PopulationSize, "0.5*n" }
                        },
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.PopulationSize, "1*n" }
                        },
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.PopulationSize, "2*n" }
                        },
                    },
                },
                Terminations = new List<TerminationSpecification>
                {
                    new TerminationSpecification
                    {
                        Name = "Stagnation",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.MaxIterations, "10" },
                            { Parameter.Epsilon, "10E-6" }
                        }
                    }
                },
                Problem = new ProblemSpecification
                {
                    Name = "JumpOffsetSpike",
                    Parameters = new Dictionary<Parameter, string>
                    {
                        { Parameter.GapSize, "0.1*n" }
                    }
                }
            };

            var timer = new Stopwatch();
            timer.Start();

            var test = runner.Run(spec);

            using (var source = new CancellationTokenSource())
            {
                var task = Task.Run(async () => await test.Execute(source.Token));
                task.Wait();
            }

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
