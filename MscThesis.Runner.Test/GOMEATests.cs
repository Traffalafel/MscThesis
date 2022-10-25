using MscThesis.Core;
using MscThesis.Runner.Specification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace MscThesis.Runner.Test
{
    public class GOMEATests
    {
        [Fact]
        public async void SingleRunOneMax()
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
                        Algorithm = "GOMEA",
                        Seed = 1,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.PopulationSize] = "n"
                        }
                    },
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

            var provider = new TestProvider(SettingsProvider.Default);
            var test = provider.Run(spec);

            using var source = new CancellationTokenSource();
            await test.Execute(source.Token);
        }

        [Fact]
        public async void SingleRunJumpOffsetSpike()
        {
            var spec = new TestSpecification
            {
                NumRuns = 1,
                ProblemSizes = { 100 },
                MaxParallelization = 1,
                Problem = new ProblemSpecification
                {
                    Name = "JumpOffsetSpike",
                    Parameters = new Dictionary<Parameter, string>()
                    {
                        { Parameter.GapSize, "0.1*n" }
                    }
                },
                Optimizers = new List<OptimizerSpecification>
                {
                    new OptimizerSpecification
                    {
                        Algorithm = "GOMEA",
                        Seed = 1,
                        Parameters = new Dictionary<Parameter, string>
                        {
                            [Parameter.PopulationSize] = "n"
                        }
                    },
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

            var provider = new TestProvider(SettingsProvider.Default);
            var test = provider.Run(spec);

            using var source = new CancellationTokenSource();
            await test.Execute(source.Token);
        }

    }
}
