using MscThesis.Core;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class MIMICTests
    {
        [Fact]
        public async void SingleRun()
        {
            var spec = new TestSpecification
            {
                NumRuns = 1,
                ProblemSizes = { 100 },
                MaxParallelization = 1,
                Problem = new ProblemSpecification
                {
                    Name = "OneMax"
                },
                Optimizers = new List<OptimizerSpecification>
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Seed = 1,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.InitialPopulationSize] = "n"
                        }
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Seed = 2,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.InitialPopulationSize] = "0.5*n"
                        }
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Seed = 3,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.InitialPopulationSize] = "2*n"
                        }
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Seed = 4,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.InitialPopulationSize] = "sqrt(n)"
                        }
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Seed = 5,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.InitialPopulationSize] = "n*log(n)"
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

        [Fact]
        public async void MultipleRuns()
        {
            var spec = new TestSpecification
            {
                NumRuns = 100,
                ProblemSizes = new List<int>
                {
                    100
                },
                Problem = new ProblemSpecification
                {
                    Name = "JumpOffsetSpike",
                    Parameters = new Dictionary<Parameter, string>
                    {
                        { Parameter.GapSize, "0.1*n" }
                    }
                },
                MaxParallelization = 20,
                Optimizers = new List<OptimizerSpecification>()
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.InitialPopulationSize, "0.5*n" }
                        },
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.InitialPopulationSize, "1*n" }
                        },
                    },
                    new OptimizerSpecification
                    {
                        Algorithm = "MIMIC",
                        Parameters = new Dictionary<Parameter, string>
                        {
                            { Parameter.InitialPopulationSize, "2*n" }
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
