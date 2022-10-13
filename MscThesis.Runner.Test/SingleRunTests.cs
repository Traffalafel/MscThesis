using MscThesis.Core;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class SingleRunTests
    {
        [Fact]
        public async void SingleRunSingleSize()
        {
            var spec = new TestSpecification
            {
                NumRuns = 1,
                ProblemSizes = { 10 },
                MaxParallelization = 2,
                Problem = new ProblemSpecification
                {
                    Name = "OneMax",
                    Parameters = new Dictionary<Parameter, string>
                    {
                    }
                },
                Optimizers = new List<OptimizerSpecification>
                {
                    new OptimizerSpecification
                    {
                        Name = "Opt1",
                        Algorithm = "MIMIC",
                        Seed = 1,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.PopulationSize] = "10"
                        }
                    }
                },
                Terminations = new List<TerminationSpecification>
                {
                    new TerminationSpecification
                    {
                        Name = "Stagnation",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.MaxIterations] = "10",
                            [Parameter.Epsilon] = "10E-6"
                        }
                    }
                }
            };

            var provider = new TestProvider();
            var test = provider.Run(spec);

            using (var source = new CancellationTokenSource())
            {
                await test.Execute(source.Token);
            }
        }
    }
}
